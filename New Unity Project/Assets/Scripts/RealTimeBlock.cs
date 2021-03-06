﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Blockを変更したくなかったから新しく作り直す・形はBlockをあまりくずさないようにする
public class RealTimeBlock : MonoBehaviour
{
    //変数は変更なし
    #region GamePlayManager
    private GamePlayManager gameManager;
    private InArray[,] inBlocks;
    private int width;
    private int height;
    #endregion

    #region 処理スピード
    private int downSpeed = -1;  //落ちるスピード
    private int upSpeed = 1;
    #endregion

    public Transform[] childen;
    private List<Transform> childPos = new List<Transform>();

    private Vector3[] previos;

    public CurrentState currentState;        //現在の自分の情報
    public bool isIns;

    private RealTimeManager timeManager;
    private FryCount[] fryCount;
    //移動量
    Vector3 velocity;

    private bool noFry;                     //落ちてきた直後かどうか
    private bool isDelete;                  //消すかどうか

    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();
        timeManager = GameObject.Find("GamePlayManager").GetComponent<RealTimeManager>();
        width = gameManager.bWidth;
        height = gameManager.bHeight;

        //自分のブロックの形を覚える
        inBlocks = new InArray[width, height];
        inBlocks = gameManager.inSpaceBlocks(inBlocks);

        currentState = CurrentState.None;              //最初は何もなし
        fryCount = GetComponentsInChildren<FryCount>();//フライカウント

        childPos = new List<Transform>();              
        for (int i = 0; i < childen.Length; i++)
        {
            childPos.Add(childen[i]);
        }
        previos = new Vector3[childPos.Count];
        velocity = Vector3.zero;                       //0で初期化
        noFry = false;
        isDelete = false;                              //最初は消せないようにする
    }

    // Update is called once per frame
    void Update()
    {
        BlockDestroy();
        if (currentState == CurrentState.None) return;
        MyBlockDesign(childPos);//毎回更新
        Move();
    }

    void Move()
    {
        velocity = Vector3.zero;    //毎回0で初期化

        switch (currentState)
        {
            case CurrentState.DownStop:
                DownStopState();
                break;
            case CurrentState.Ready:
                ReadyState();
                break;
        }
        MoveOk();
        ChildPosCheck();
        transform.position += velocity;     //最後に代入する
    }

    #region 状態管理
    //下に止まった瞬間
    void DownStopState()
    {
        //自分がゼロじゃなく 下にゼロかCがあったら cにする
        if (!CheckFryCount() && gameManager.DownReadyOrZero(inBlocks, childPos))
        {
            for (int i = 0; i < childPos.Count; i++)
            {
                gameManager.SelfReady(childPos[i].position);
            }
            currentState = CurrentState.Ready;
        }
        //自分がゼロだったら  zeroにする
        else if (CheckFryCount())
        {
            for (int i = 0; i < childPos.Count; i++)
            {
                gameManager.SelfZero(childPos[i].position);
            }
            currentState = CurrentState.Ready;
        }
    }
    //準備状態
    void ReadyState()
    {
        //上にspaceがあったら
        if (!gameManager.NotOnSpace(inBlocks, childPos))
        {
            if (!CheckFryCount())
                currentState = CurrentState.SelfReady;
            else
                currentState = CurrentState.SelfZero;
        }
    }

    //子供たちの場所から確認
    void ChildPosCheck()
    {
        for (int i = 0; i < childPos.Count; i++)
        {
            if (childPos[i] == null) break;
            //移動中何かに当たったら止める
            switch (currentState)
            {
                case CurrentState.Down:           //落ちてるとき
                    if(gameManager.OnCheckCollision(childPos[i].position, childPos[i].position + new Vector3(0, -1, 0)))
                    {
                        currentState = CurrentState.DownStop;
                        CheckNoFry();
                        return;
                    }
                    if (gameManager.OnBlockCheck(childPos[i].position, childPos[i].position + new Vector3(0, -1, 0)))  //一マス先に何かのブロックがあるかどうか　か　2マス先に死んでいるブロックがあるかどうか
                    {
                        currentState = CurrentState.DownStop;
                        CheckNoFry();
                    }
                    break;
                case CurrentState.SelfReady:       //共通
                case CurrentState.SelfZero:
                    if (gameManager.OnBlockCheck(childPos[i].position, childPos[i].position + new Vector3(0, 1, 0)))
                    {
                        currentState = CurrentState.UpStop;
                        if (CheckUpSpaceArray(i))
                        {
                            timeManager.SetIsDelete(true);
                        }
                    }
                    break;
                case CurrentState.UpStop:         //上昇が止まって上のがどこかいってスペースに変わったら上昇するよう切り替える
                    if (!gameManager.OnBlockCheck(childPos[i].position, childPos[i].position + new Vector3(0, 1, 0)))
                    {
                        currentState = CurrentState.SelfReady;
                    }
                    break;
            }
        }
    }
    //自分の上にスペースがあるか調べる　あったらfalse　なかったらtrue
    bool CheckUpSpaceArray(int childNum)
    {
        Vector3 pos = childPos[childNum].position;
       
        int Y = (int)pos.y;
        //自分の位置から上を調べる
        for (int y = Y; y < 17; y++)
        {
            //自分の上がSpaceならfalse
            if (gameManager.SelfState(new Vector3(pos.x,y,pos.z)) == InArray.Space)
            {
                return false;
            }
        }
        return true;
    }
    //移動許可
    void MoveOk()
    {
        //移動許可が出ていなかったら早期リターン
        if (!gameManager.moveOk) return;

        switch (currentState)
        {
            case CurrentState.Down:
                velocity = new Vector3(0, downSpeed, 0);    //下に移動し続ける
                break;
            case CurrentState.SelfZero:
            case CurrentState.SelfReady:
                velocity = new Vector3(0, upSpeed, 0);      //上に移動し続ける
                break;
            case CurrentState.DownStop:
            case CurrentState.UpStop:
                velocity = new Vector3(0, 0, 0);            //とまる
                break;
        }
    }

    #endregion


    //一度下についたかどうかを検査
    void CheckNoFry()
    {
        //一度下についた物は揚げカウントを操作できない
        if (noFry) return;
        noFry = true;
        timeManager.SetIsFryCountDown(true);       //下に止まった時変える
        
    }


    //揚げられているかどうかを調べる
    public bool CheckFryCount()
    {
        //どれか一つでも0なら上げる
        for (int i = 0; i < fryCount.Length; i++)
        {
            if (fryCount[i].GetFryCount() <= 0)
                return true;
        }
        return false;
    }
    //自分を消す
    private void BlockDestroy()
    {
        int count = childPos.Count;
        for (int i = 0; i < childPos.Count; i++)
        {
            if (childPos[i] == null)
            {
                childPos.Remove(childPos[i]);
                count--;
            }
        }
        if (count <= 0)
        {
            //子供が消えたら自分も消す
            gameManager.ReMoveUseList(gameObject);
            Destroy(gameObject);
            return;
        }
    }

    //生成しなおした際に呼ばれる関数
    public void InsCube()
    {
        for (int i = 0; i < childPos.Count; i++)
        {
            childPos[i].GetComponent<RealTimeCube>().enabled = true;
        }
    }
    //自分の型を設計する
    private void MyBlockDesign(List<Transform> t)
    {
        //いったん真っ白にしてから
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                inBlocks[x, y] = InArray.Space;
            }
        }
        //自分の情報を入れる
        for (int i = 0; i < childPos.Count; i++)
        {
            //nullだったらリターン
            if (childPos[i] == null) return;
            int cx = (int)childPos[i].position.x;
            int cy = (int)childPos[i].position.y;

            inBlocks[cx, cy] = gameManager.SelfState(childPos[i].position);
        }
    }
    //フライカウントを一個下げる
    public void FryCountDown()
    {
        for (int i = 0; i < childPos.Count; i++)
        {
            childPos[i].GetComponentInChildren<FryCount>().FryCountDown();
        }
    }
    //揚げ状態を確認する
    public bool GetNoFry()
    {
        return noFry;
    }
    //死亡状態を確認する
    public bool GetisDelete()
    {
        return isDelete;
    }
}

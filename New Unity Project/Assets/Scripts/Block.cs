﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//他で参照するからクラスの外に出した。
public enum CurrentState
{
    Down,//落ちてくる
    DownStop,//落ちてきて止める
    Up,//上に上がる
    UpStop,//上に上がって止める
    OutOil,//油の外

    Ready,//準備

    SelfZero,   //自分がゼロ(UP)
    SelfReady,  //自分がゼロじゃない(UP)

    DownReSpawn,    //生成しなおして順番を変える
    Re, //復活した

    None,
}

public class Block : MonoBehaviour
{
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

    private TurnChange turn;
    private FryCount[] fryCount;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();
        width = gameManager.bWidth;
        height = gameManager.bHeight;

        //自分のぶろっくの形を覚える
        inBlocks = new InArray[width, height];
        inBlocks = gameManager.inSpaceBlocks(inBlocks);

        currentState = CurrentState.None;
        //フライカウント
        fryCount = GetComponentsInChildren<FryCount>();

        childPos = new List<Transform>();
        for (int i = 0; i < childen.Length; i++)
        {
            childPos.Add(childen[i]);
        }

        previos = new Vector3[childPos.Count];
    }

    // Update is called once per frame
    void Update()
    {
        BlockDestroy();

        //下で再生成されたとき
        //if (isIns)
        //{
        //    currentState = CurrentState.Re;
        //}

        if (currentState == CurrentState.None)
        {
            return;
        }

        //毎回更新
        MyBlockDesign(childPos);

        Move();
    }

    //移動
    void Move()
    {
        //移動量
        Vector3 velocity = Vector3.zero;    //毎回0で初期化

        #region 順序入れ替えが難しいから隠す
        //ProOrder順序替え
        //if(currentState == CurrentState.DownStop && gameManager.NotOnSpace(inBlocks, childPos))
        //{
        //    currentState = CurrentState.DownReSpawn;
        //}

        //if (Input.GetKeyDown(KeyCode.N) && currentState == CurrentState.DownStop || currentState == CurrentState.Re
        //    && !gameManager.NotOnSpace(inBlocks, childPos))
        //{
        //    currentState = CurrentState.Up;
        //    if(isIns)
        //    {
        //        isIns = false;
        //    }
        //}
        #endregion

        //とりあえずcにする
        //下で止まっているとき　自分がゼロじゃなく  下にゼロかCがあったら cにする
        if (currentState == CurrentState.DownStop && !CheckFryCount() && gameManager.DownReadyOrZero(inBlocks, childPos))
        {
            for (int i = 0; i < childPos.Count; i++)
            {
                gameManager.SelfReady(childPos[i].position);
            }
            currentState = CurrentState.Ready;
        }
        //下で止まっているとき　自分がゼロだったら  zeroにする リザルトの時
        else if (currentState == CurrentState.DownStop && CheckFryCount())
        {
            for (int i = 0; i < childPos.Count; i++)
            {
                gameManager.SelfZero(childPos[i].position);
            }
            currentState = CurrentState.Ready;
        }

        //リザルトだったら  //上にspaceがあったら
        if (currentState == CurrentState.Ready && !gameManager.NotOnSpace(inBlocks, childPos))//自分が
        {
            if (!CheckFryCount())
            {
                currentState = CurrentState.SelfReady;
            }
            else if (CheckFryCount())
            {
                currentState = CurrentState.SelfZero;
            }
        }

        if (gameManager.moveOk)
        {
            //下に行くなら移動量を下に
            if (currentState == CurrentState.Down)
            {
                //下に移動し続ける
                velocity = new Vector3(0, downSpeed, 0);
            }
            //
            else if (currentState == CurrentState.SelfZero || currentState == CurrentState.SelfReady)
            {
                //上に移動し続ける
                velocity = new Vector3(0, upSpeed, 0);
            }
            else if (currentState == CurrentState.DownStop && currentState == CurrentState.UpStop)
            {
                //とまる
                velocity = new Vector3(0, 0, 0);
            }
        }

        //子供たちの場所から確認
        for (int i = 0; i < childPos.Count; i++)
        {
            if (childPos[i] == null) break;

            //落ちているとき
            if (gameManager.OnBlockCheck(childPos[i].position, childPos[i].position + velocity) &&
                currentState == CurrentState.Down)
            {
                currentState = CurrentState.DownStop;
            }
            //上がっているとき  数字１以上ブロック 　上に来たら止める
            else if (gameManager.OnBlockCheck(childPos[i].position, childPos[i].position + velocity)  &&
                currentState == CurrentState.SelfReady)
            {
                currentState = CurrentState.UpStop;
            }
            //上がっているとき  数字0ブロック 　上に来たら止める
            else if (gameManager.OnBlockCheck(childPos[i].position, childPos[i].position + velocity) &&
                currentState == CurrentState.SelfZero)
            {
                currentState = CurrentState.UpStop;
            }

            if (gameManager.CheckOil(childPos[i].position, childPos[i].position + velocity))
            {
                //currentState = CurrentState.OutOil;
            }

            if (!gameManager.OnCubeOfWallCheck(childPos[i].position, childPos[i].position + velocity))
            {
                return;
            }
        }
        transform.position += velocity;
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
            childPos[i].GetComponent<Cube>().enabled = true;
        }
    }

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

    //下にゼロが無かったら
    public bool DownZeroBlock()
    {
        if (gameManager.DownZeroBlock(inBlocks, childPos))
        {
            return true;
        }
        return false;
    }
    //一個だけ上にあげる
    public void PositionUp()
    {
        int y = 0;
        for (int i = 0; i < childPos.Count; i++)
        {
            if (gameManager.DownZeroBlockChild(inBlocks, childPos[i].position))
            {
                if ((int)childPos[i].position.y > y)
                {
                    y = (int)childPos[i].position.y + 1;
                }
            }
        }
        transform.position = new Vector3(transform.position.x, y, 0);
    }
}
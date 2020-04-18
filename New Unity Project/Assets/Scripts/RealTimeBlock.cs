using System.Collections;
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

        currentState = CurrentState.None;              //最初は何もなし
        fryCount = GetComponentsInChildren<FryCount>();//フライカウント

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
        MyBlockDesign(childPos);//毎回更新
        Move();
    }

    void Move()
    {
        //移動量
        Vector3 velocity = Vector3.zero;    //毎回0で初期化




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

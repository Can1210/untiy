using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Block_test : MonoBehaviour
{
    //神クラス Mapクラス
    private GamePlayManager gameManager;

    //Turnがあったらいいな
    private TurnChange turn;

    private BlockMove_test[] childrenMove;

    public Transform[] childPos;

    //BlockMove_testから移植
    private float time;
    private int currenTime;
    public bool isStop;
    private bool moveOk;
    private int downSpeed = -1;  //落ちるスピード
    private int upSpeed = 1;

    private bool isFry;          //揚げられているか

    private Vector3[] previos;

    //フライカウント
    private FryCount[] f;

    private CurrntBlockState blockState;

    private enum CurrntBlockState
    {
        DownBlock,//落ちてくる
        StopBlock,//FixedBLOCK
        UpBlock,//上に上がる

        None,
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();
        turn = gameManager.GetComponent<TurnChange>();

        childrenMove = transform.GetComponentsInChildren<BlockMove_test>();

        blockState = CurrntBlockState.DownBlock;

        //移植
        isStop = false;
        moveOk = false;
        time = 0;
        currenTime = 0;

        //フライカウント
        f = GetComponentsInChildren<FryCount>();

        previos = new Vector3[childPos.Length];
        isFry = false;    //最初は挙げられていない
    }

    // Update is called once per frame
    void Update()
    {
        //↓落ちるスピード
        time += Time.deltaTime * 3.5f;
        if (time >= 1)
        {
            currenTime += (int)time;
            time = 0;
            moveOk = true;
        }

        Move();
    }

    //移動
    void Move()
    {
        //移動量
        Vector3 d = Vector3.zero;    //毎回0で初期化

        //リザルト中　かつ　止まっている　かつフライカウントが0以下の時　上がる
        if (turn.GetTurn() == Turn.Results &&
           blockState == CurrntBlockState.StopBlock &&
           CheckFryCount())
        {
            for (int i = 0; i < childPos.Length; i++)
            {
                gameManager.NotFixedBlock(childPos[i].position);
            }
            blockState = CurrntBlockState.UpBlock;
            isStop = false;
        }
        //上に上がる処理
        //if (Input.GetKeyDown(KeyCode.N) && blockState == CurrntBlockState.StopBlock)
        //{
        //    for (int i = 0; i < childPos.Length; i++)
        //    {
        //        gameManager.NotFixedBlock(childPos[i].position);
        //    }
        //    blockState = CurrntBlockState.UpBlock;
        //    isStop = false;
        //}
        if (isStop)
        {
            //最初だけ呼ばれる
            if (blockState == CurrntBlockState.StopBlock)
                return;
            blockState = CurrntBlockState.StopBlock;

            turn.SetTurnChange();    //下についたらターンを切り替える
            Debug.Log("地面についた");

            return;
        }

        if (moveOk)
        {
            //下に行くなら移動量を下に
            if (blockState == CurrntBlockState.DownBlock)
            {
                //下に移動し続ける
                d = new Vector3(0, downSpeed, 0);
            }
            //下に行くなら移動量を下に
            else if (blockState == CurrntBlockState.UpBlock)
            {
                //上に移動し続ける
                d = new Vector3(0, upSpeed, 0);
            }
            else if (blockState == CurrntBlockState.StopBlock)
            {              
                //とまる
                d = new Vector3(0, 0, 0);
            }

            moveOk = false;
        }

        //ダウンの時だけ移動できる
        if (blockState == CurrntBlockState.DownBlock)
        {
            //横移動
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                d = new Vector3(-1f, downSpeed, 0);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                d = new Vector3(1f, downSpeed, 0);
            }
        }

        //子供たちの場所から確認
        for (int i = 0; i < childPos.Length; i++)
        {
            if (gameManager.OnBlockCheck(childPos[i].position, childPos[i].position + d) &&
                blockState != CurrntBlockState.StopBlock)
            {
                isStop = true;
            }

            if (!gameManager.OnCubeOfWallCheck(childPos[i].position, childPos[i].position + d))
            {
                return;
            }
        }


        transform.position += d;

        gameManager.PreviousArray();
    }

    //揚げられているかどうかを調べる
    bool CheckFryCount()
    {
        //どれか一つでも0なら上げる
        for (int i = 0; i < f.Length; i++)
        {
            if (f[i].GetFryCount() <= 0)
                return true;
        }
        return false;
    }


    public void ChildrenStop()
    {
        for (int i = 0; i < childrenMove.Length; i++)
        {
            childrenMove[i].isStop = true;
        }
    }

    public void ChildrenMove()
    {
        for (int i = 0; i < childrenMove.Length; i++)
        {
            childrenMove[i].isStop = false;
        }
    }

}
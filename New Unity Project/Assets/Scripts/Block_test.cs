using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        turn = GameObject.Find("GamePlayManager").GetComponent<TurnChange>();

        childrenMove = transform.GetComponentsInChildren<BlockMove_test>();

        blockState = CurrntBlockState.DownBlock;

        //移植
        isStop = true;
        moveOk = false;
        time = 0;
        currenTime = 0;

        //フライカウント
        f = GetComponentsInChildren<FryCount>();

        previos = new Vector3[childPos.Length];
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
        Vector3 d = Vector3.zero; 

        if(Input.GetKeyDown(KeyCode.N) && blockState == CurrntBlockState.StopBlock)
        {
            Debug.Log("上がる");
            blockState = CurrntBlockState.UpBlock;
            isStop = false;
        }

        if (isStop)
        {
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

        ////一番下だったら止める  worldPos[x,1]←y座標の1は0にwallが入っているから
        //if (gameManager.UnderMap(transform.position))
        //{
        //    isStop = true;
        //}

        //ダウンの時だけ移動できる
        if(blockState == CurrntBlockState.DownBlock)
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
        for (int i = 0;i < childPos.Length; i++)
        {
            if(gameManager.OnBlockCheck(childPos[i].position, childPos[i].position + d)
                && blockState == CurrntBlockState.DownBlock)
            {
                blockState = CurrntBlockState.StopBlock;
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
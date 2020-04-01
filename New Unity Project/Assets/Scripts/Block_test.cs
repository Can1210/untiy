using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//他で参照するからクラスの外に出した。
public enum CurrntBlockState
{
    DownBlock,//落ちてくる
    StopBlock,//FixedBLOCK
    UpBlock,//上に上がる

    None,
}

public class Block_test : MonoBehaviour
{
    //神クラス Mapクラス
    private GamePlayManager gameManager;

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

    public CurrntBlockState blockState;

    public int turnCount = 1;
    //オイル外に入ったかどうか
    public bool isOilOut = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();

        childrenMove = transform.GetComponentsInChildren<BlockMove_test>();

        blockState = CurrntBlockState.DownBlock;

        //移植
        isStop = false;
        moveOk = false;
        time = 0;
        currenTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //                       ↓落ちるスピード
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

        if (Input.GetKeyDown(KeyCode.N) && blockState == CurrntBlockState.StopBlock)
        {
            for (int i = 0; i < childPos.Length; i++)
            {
                turnCount = childPos[i].transform.GetComponentInChildren<FryCount>().fryCount;
                //Debug.Log(index);
                if (turnCount == 0)
                {
                    gameManager.Zero(childPos[i].position);
                }
                else if (turnCount != 0)
                {
                    //キューブに変える
                    gameManager.NotFixedBlock(childPos[i].position);
                }
            }
            blockState = CurrntBlockState.UpBlock;
            isStop = false;
        }

        if (isStop)
        {
            blockState = CurrntBlockState.StopBlock;

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

            if (gameManager.CheckOid(childPos[i].position, childPos[i].position + d))
            {
                isOilOut = true;
            }

            if (gameManager.OilOnCheckZero(childPos[i].position))
            {
                return;
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
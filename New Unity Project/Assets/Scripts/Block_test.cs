using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//他で参照するからクラスの外に出した。
public enum CurrentState
{
    Down,//落ちてくる
    DownStop,//落ちてきて止める
    Up,//上に上がる
    UpStop,//上に上がって止める

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

    public CurrentState currentState;

    public int turnCount = 1;
    //オイル外に入ったかどうか
    public bool isOilOut = false;
    //delet
    public bool isDel = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();

        childrenMove = transform.GetComponentsInChildren<BlockMove_test>();

        currentState = CurrentState.Down;

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

        if (Input.GetKeyDown(KeyCode.N) && currentState == CurrentState.DownStop)
        {
            for (int i = 0; i < childPos.Length; i++)
            {
                turnCount = childPos[i].transform.GetComponentInChildren<FryCount>().fryCount;

                if (turnCount == 0)
                {
                    //カウントが0ならゼロの情報にする
                    gameManager.Zero(childPos[i].position);
                }
                else if (turnCount != 0)
                {
                    //キューブに変える
                    gameManager.NotFixedBlock(childPos[i].position);
                }
            }
            currentState = CurrentState.Up;
            isStop = false;
        }

        //上で止まっているときの子供の確認
        if (currentState == CurrentState.UpStop)
        {
            for (int i = 0; i < childPos.Length; i++)
            {

            }
        }

        if (isStop && currentState == CurrentState.Up)
        {
            currentState = CurrentState.UpStop;
            return;
        }
        else if (isStop && currentState == CurrentState.Down)
        {
            currentState = CurrentState.DownStop;
            return;
        }


        if (moveOk)
        {
            //下に行くなら移動量を下に
            if (currentState == CurrentState.Down)
            {
                //下に移動し続ける
                d = new Vector3(0, downSpeed, 0);
            }
            //
            else if (currentState == CurrentState.Up)
            {
                //上に移動し続ける
                d = new Vector3(0, upSpeed, 0);
            }
            else if (currentState == CurrentState.DownStop)
            {
                //とまる
                d = new Vector3(0, 0, 0);
            }

            moveOk = false;
        }

        //ダウンの時だけ移動できる
        if (currentState == CurrentState.Down)
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
            //落ちているとき
            if (gameManager.OnBlockCheck(childPos[i].position, childPos[i].position + d) &&
                currentState == CurrentState.Down)
            {
                isStop = true;
            }
            //上がっているとき
            else if (gameManager.OnBlockCheck(childPos[i].position, childPos[i].position + d) &&
                currentState == CurrentState.Up)
            {
                isStop = true;
            }

            if (gameManager.CheckOil(childPos[i].position, childPos[i].position + d))
            {
                isOilOut = true;
            }

            if (!gameManager.OnCubeOfWallCheck(childPos[i].position, childPos[i].position + d))
            {
                return;
            }
        }

        transform.position += d;
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
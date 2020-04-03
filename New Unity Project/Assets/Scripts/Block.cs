using System.Collections;
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

    None,
}

public class Block : MonoBehaviour
{
    //神クラス Mapクラス
    private GamePlayManager gameManager;
    private InArray[,] inBlocks;
    private int width;
    private int height;

    public Transform[] childPos;

    //BlockMove_testから移植
    private float time;
    private int currenTime;
    private bool moveOk;
    private int downSpeed = -1;  //落ちるスピード
    private int upSpeed = 1;

    private Vector3[] previos;

    public CurrentState currentState;

    public int turnCount = 1;
    //オイル外に入ったかどうか
    public bool isOilOut = false;

    private TurnChange turn;
    private FryCount[] f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();
        turn = gameManager.GetComponent<TurnChange>();
        width = gameManager.bWidth;
        height = gameManager.bHeight;

        currentState = CurrentState.None;

        inBlocks = new InArray[width, height];
        inBlocks = gameManager.inSpaceBlocks(inBlocks);

        //移植
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
        //                       ↓落ちるスピード
        time += Time.deltaTime * 3.5f;
        if (time >= 1)
        {
            currenTime += (int)time;
            time = 0;
            moveOk = true;
        }

        if (currentState == CurrentState.None)
        {
            return;
        }

        //毎回更新
        InBlocks(childPos);

        Move();
    }

    //移動
    void Move()
    {
        //移動量
        Vector3 d = Vector3.zero;    //毎回0で初期化

        //リザルト中　かつ　止まっている　かつ　フライカウントが0以下の時　上がる
        //if (Input.GetKeyDown(KeyCode.N) && currentState == CurrentState.DownStop)
        if (turn.GetTurn() == Turn.Results &&
            currentState == CurrentState.DownStop &&
            CheckFryCount())
        {
            //上に移動し続ける
            d = new Vector3(0, upSpeed, 0);
            
            if (gameManager.NotOnSpace(inBlocks, childPos))
            {
                currentState = CurrentState.DownStop;
            }
            else
            {
                currentState = CurrentState.Up;
            }

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
                if (currentState == CurrentState.DownStop) return;  //一回しか呼ばれないようにする
                
                currentState = CurrentState.DownStop;
                turn.SetTurnChange();   //下についたらターンを変える

            }
            //上がっているとき
            else if (gameManager.OnBlockCheck(childPos[i].position, childPos[i].position + d) &&
                currentState == CurrentState.Up)
            {
                currentState = CurrentState.UpStop;
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


    private void InBlocks(Transform[] t)
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
        for (int i = 0; i < childPos.Length; i++)
        {
            int cx = (int)childPos[i].position.x;
            int cy = (int)childPos[i].position.y;

            inBlocks[cx, cy] = gameManager.SelfState(childPos[i].position);
        }
    }
}
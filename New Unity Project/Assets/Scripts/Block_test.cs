using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_test : MonoBehaviour
{
    private BlockMove_test[] childrenMove;

    private Transform[] childPos;

    //BlockMove_testから移植
    private Vector3 dic;
    private float time;
    private int currenTime;
    public bool isStop;
    private bool moveOk;
    private int xPos = 0;
    [SerializeField]
    private int downSpeed = -1;  //落ちるスピード


    //神クラス Mapクラス
    private GamePlayManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();

        childrenMove = transform.GetComponentsInChildren<BlockMove_test>();

        childPos = transform.GetComponentsInChildren<Transform>();


        //移植
        isStop = true;
        moveOk = false;
        time = 0;
        currenTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    transform.position = gameManager.ZeroPosition(transform.position);
        //}

        //↓落ちるスピード
        time += Time.deltaTime * 2.5f;
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
        dic = transform.position;
        xPos = 0;

        if (isStop)
        {
            return;
        }

        if (moveOk)
        {
            //下に移動し続ける
            dic += new Vector3(0, downSpeed, 0);
            moveOk = false;
        }

        //一番下だったら止める  worldPos[x,1]←y座標の1は0にwallが入っているから
        if (gameManager.UnderMap(transform.position))
        {
            isStop = true;
        }

        //if(gameManager.OnMoveOk(dic))
        //{
        //    isStop = true;
        //}

        //横移動
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            xPos = -1;
            dic += new Vector3(xPos, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            xPos = 1;
            dic += new Vector3(xPos, 0, 0);
        }

        transform.position = dic;
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
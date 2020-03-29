using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove_test : MonoBehaviour
{
    [SerializeField]
    private int downSpeed = -1;  //落ちるスピード
    [SerializeField]
    private int upSpeed;    //上昇スピード

    public bool isStop;

    GamePlayManager gameManager;

    private float time;
    private int currenTime;

    private bool moveOk;
    public bool nowMove{
        set { this.moveOk = value; }
        get { return this.moveOk; }
        }

    private int xPos = 0;
    private Vector3 dic;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();

        //isStop = true;
        moveOk = false;

        time = 0;
        currenTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
                               //↓落ちるスピード
        //time += Time.deltaTime * 2.5f;
        //if (time >= 1)
        //{
        //    currenTime += (int)time;
        //    time = 0;
        //    moveOk = true;
        //}

        //Move();
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

        if(moveOk)
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
            xPos =1;
            dic += new Vector3(xPos, 0, 0);
        }

        transform.position = dic;
    }
}

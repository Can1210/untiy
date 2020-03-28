using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove_test : MonoBehaviour
{

    private bool anyTouch;

    [SerializeField]
    private int downSpeed = -1;  //落ちるスピード
    [SerializeField]
    private int upSpeed;    //上昇スピード

    private Rigidbody rigid;

    public bool isStop;

    GamePlayManager gameManager;

    private float time;
    private int currenTime;

    private bool moveOk;

    private int xPos = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();

        isStop = true;
        moveOk = false;

        time = 0;
        currenTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time % 60 >= 1)
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

        xPos = 0;
        if (isStop)
        {
            return;
        } 

        if(moveOk)
        {
            //下に移動し続ける
            transform.position += new Vector3(0, downSpeed, 0);
            moveOk = false;
        }     

        if(gameManager.UnderMap(transform.position))
        {
            isStop = true;
        }

        //横移動
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            xPos = -1;
            transform.position += new Vector3(xPos, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            xPos =1;
            transform.position += new Vector3(xPos, 0, 0);
        }

    }
}

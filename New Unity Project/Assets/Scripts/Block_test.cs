using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_test : MonoBehaviour
{
    private BlockMove_test[] childrenMove;

    public Transform[] childPos;

    //BlockMove_testから移植
    private float time;
    private int currenTime;
    public bool isStop;
    private bool moveOk;
    private int downSpeed = -1;  //落ちるスピード

    private Vector3[] previos;

    //神クラス Mapクラス
    private GamePlayManager gameManager;

    private List<bool> b = new List<bool>();

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();

        childrenMove = transform.GetComponentsInChildren<BlockMove_test>();
        //childPos = transform.GetComponentsInChildren<Transform>();

        //移植
        isStop = true;
        moveOk = false;
        time = 0;
        currenTime = 0;

        previos = new Vector3[childPos.Length];
    }

    // Update is called once per frame
    void Update()
    {
        //↓落ちるスピード
        time += Time.deltaTime * 1.5f;
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

        if (isStop)
        {
            return;
        }

        if (moveOk)
        {
            //下に移動し続ける
            d = new Vector3(0, downSpeed, 0);
            moveOk = false;
        }

        ////一番下だったら止める  worldPos[x,1]←y座標の1は0にwallが入っているから
        //if (gameManager.UnderMap(transform.position))
        //{
        //    isStop = true;
        //}

        //横移動
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            d = new Vector3(-1f, downSpeed,0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            d = new Vector3(1f, downSpeed, 0);
        }

        //子供たちの場所から確認
        for (int i = 0;i < childPos.Length; i++)
        {
            if(gameManager.OnBlockCheck(childPos[i].position, childPos[i].position + d))
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
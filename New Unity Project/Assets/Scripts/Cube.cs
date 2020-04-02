using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

    //enumで上がるターンが来たらターンを減らすだけのスクリプト
    private GamePlayManager gameManager;

    private Block_test blocktest;

    private Vector3 previous;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();
        blocktest = GetComponentInParent<Block_test>();       

        previous = new Vector3(1,1,0);//ゼロだと都合が悪い
    }

    // Update is called once per frame
    void Update()
    {
        //毎回描画する奴ら

        //if(gameManager.Death(transform.position))
        //{
        //    //Destroy(gameObject);
        //}

        if (blocktest.isStop && blocktest.currentState == CurrentState.Down)
        {
            //落ちて止まったら下で止まったブロックにする
            gameManager.DownFixedBlock(transform.position);
        }
        else if (blocktest.isStop && blocktest.currentState == CurrentState.Up)
        {
            //上がったら上で止まったブロックにする
            gameManager.UpFixedBlock(transform.position);
        }

        //こっちは普通の
        else if (!blocktest.isStop &&  blocktest.currentState == CurrentState.Down || blocktest.currentState == CurrentState.Up &&  blocktest.turnCount != 0 && !blocktest.isOilOut)
        {
            gameManager.inCubeArray(transform.position, previous);
        }

        //オイル外
        else if (!blocktest.isStop && 
            blocktest.currentState == CurrentState.Down || blocktest.currentState == CurrentState.Up 
            && blocktest.turnCount != 0 && blocktest.isOilOut)
        {
            gameManager.inOilOutArray(transform.position, previous);
        }

        //ここでカウントが0のやつの情報を入れてる
        else if (!blocktest.isStop && 
            blocktest.currentState == CurrentState.Down || blocktest.currentState == CurrentState.Up 
            && blocktest.turnCount  == 0)
        {
            gameManager.inZeroArray(transform.position, previous);
        }

        previous = transform.position;//前のポジション
    }
}

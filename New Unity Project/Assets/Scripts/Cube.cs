using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public int turn;

    //enumで上がるターンが来たらターンを減らすだけのスクリプト
    private GamePlayManager gameManager;

    private Block_test blocktest;

    private Vector3 previous;

    //自分のところに行けない場所(WallとCube)があったらtrueを返す用
    public bool isMoveOMG;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();
        blocktest = GetComponentInParent<Block_test>();       

        previous = new Vector3(1,1,0);//ゼロだと都合が悪い
        isMoveOMG = false;
    }

    // Update is called once per frame
    void Update()
    {
        //3/31
        //毎回描画する奴ら

        if (blocktest.isStop && blocktest.blockState == CurrntBlockState.DownBlock)
        {
            gameManager.FixedBlock(transform.position);
        }
        else if (blocktest.isStop && blocktest.blockState == CurrntBlockState.UpBlock)
        {
            gameManager.UpFixedBlock(transform.position);
        }

        //こっちは普通の
        else if (!blocktest.isStop &&  blocktest.blockState != CurrntBlockState.StopBlock &&  blocktest.turnCount != 0 && !blocktest.isOilOut)
        {
            gameManager.inCubeArray(transform.position, previous);
        }
        //オイル外
        else if (!blocktest.isStop && blocktest.blockState != CurrntBlockState.StopBlock && blocktest.turnCount != 0 && blocktest.isOilOut)
        {
            gameManager.inOilOutArray(transform.position, previous);
        }

        //ここでカウントが0のやつの情報を入れてる
        else if (!blocktest.isStop && blocktest.blockState != CurrntBlockState.StopBlock && blocktest.turnCount  == 0)
        {
            gameManager.inZeroArray(transform.position, previous);
        }


        previous = transform.position;//前のポジション
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    //enumで上がるターンが来たらターンを減らすだけのスクリプト
    private GamePlayManager gameManager;

    private Block b;

    private Vector3 previous;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();
        b = GetComponentInParent<Block>();

        previous = new Vector3(1, 1, 0);//ゼロだと都合が悪い
    }

    // Update is called once per frame
    void Update()
    {
        //毎回描画する奴ら
        //
        if (gameManager.turn == Turn.Delete)
        {
            return;
        }
        //自分がデスエリアだったら
        else if (gameManager.ZeroDeathArea(transform.position) && gameManager.turn == Turn.Deleting)
        {
            Destroy(GetComponentInChildren<FryCount>().childObject);
            Destroy(gameObject);
            return;
        }

        if (b.currentState == CurrentState.DownStop && gameManager.SelfState(transform.position) != InArray.Zero)
        {
            //落ちて止まったら下で止まったブロックにする
            gameManager.DownFixedBlock(transform.position);
        }
        else if (b.currentState == CurrentState.UpStop && !b.CheckFryCount())
        {
            //上がったら上で止まったブロックにする
            gameManager.UpFixedBlock(transform.position);
        }
        //こっちは普通の
        else if (b.currentState == CurrentState.Down || b.currentState == CurrentState.Up)
        {
            gameManager.inCubeArray(transform.position, previous);
        }

        //オイル外  カウントが0のやつはいけない
        else if (b.currentState == CurrentState.OutOil)
        {
            gameManager.inOilOutArray(transform.position, previous);
        }

        //ここでカウントが0のやつの情報を入れてる
        else if (b.currentState == CurrentState.SelfZero )//&& gameManager.SelfState(transform.position) == InArray.Zero)
        {
            gameManager.inZeroArray(transform.position,previous);
        }
        else if(b.currentState == CurrentState.SelfReady)
        {
            gameManager.inReadyArray(transform.position,previous);
        }

        previous = transform.position;//前のポジション
    }
}

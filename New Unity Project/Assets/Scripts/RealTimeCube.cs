using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Cubeを変更したくなかったから新しく作り直す・形はCubeをあまりくずさないようにする
public class RealTimeCube : MonoBehaviour
{

    //enumで上がるターンが来たらターンを減らすだけのスクリプト
    private GamePlayManager gameManager;
    private RealTimeBlock block;
    private Vector3 previous;
    private RealTimeManager timeManager;

    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>(); //ゲームマネージャーを探して参照
        timeManager = GameObject.Find("GamePlayManager").GetComponent<RealTimeManager>(); //ゲームマネージャーを探して参照
        block = GetComponentInParent<RealTimeBlock>();                                    //親の持っているBlockクラスを参照
        previous = new Vector3(1, 1, 0);                                                  //ゼロだと都合が悪い
    }

    void Update()
    {
        BlockStateManager();            //Cuveの状態管理
    }
    //親のブロックの状態に応じてこちらを変更していく
    void BlockStateManager()
    {
        //自分が死亡場所にいる かつ　消されるときになったら　消す
        if(gameManager.ZeroDeathArea(transform.position) && timeManager.GetIsDelete())
        {
            Destroy(GetComponentInChildren<FryCount>().childObject);
            Destroy(gameObject);
            return;
        }
        //親ブロックの状態管理
        switch (block.currentState)
        {
            case CurrentState.DownStop:      //落ちてきて止める
                DownStopState();
                break;
            case CurrentState.UpStop:        //上に上がって止める
                if(!block.CheckFryCount())
                    //上がったら上で止まったブロックにする
                    gameManager.UpFixedBlock(transform.position);
                break;
            case CurrentState.Down:          //落ちてる         //こいつらは同じ処理
            case CurrentState.Up:            //上に上がる
                gameManager.inCubeArray(transform.position, previous);
                break;
            case CurrentState.OutOil:        //油の外
                gameManager.inOilOutArray(transform.position, previous);
                break;
            case CurrentState.SelfZero:      //自分がゼロ(UP)
                gameManager.inZeroArray(transform.position, previous);
                break;
            case CurrentState.SelfReady:     //自分がゼロじゃない(UP)
                gameManager.inReadyArray(transform.position, previous);
                break;
        }
        previous = transform.position;//前のポジション
    }
    //落ちてきて止める状態処理がかさばるので別に作る
    void DownStopState()
    {
        if (gameManager.SelfState(transform.position) != InArray.Zero)
        {
            //落ちて止まったら下で止まったブロックにする
            gameManager.DownFixedBlock(transform.position);
        }
    }
}

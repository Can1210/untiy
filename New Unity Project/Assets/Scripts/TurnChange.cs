using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Turn
{
    Thinking,//選ぶ状態
    PutIn,//落とした状態
    Results,//上に上がった状態
}
//ターン切り替えクラス
public class TurnChange : MonoBehaviour
{
    private bool turnChange;        //ターンを切り替えるかどうか
    public Turn nowTurn;            //現在のターン
    private bool roundEnd;          //1ローテーションの終了
    //private bool fryCountUp;        //揚げカウントを進めるかどうかの判断
    [SerializeField]
    private Text text;              //現在のターンのテキスト
    //[SerializeField]
    //public GameObject prefab;      //プレハブ立方体
    //private FryCount fryCount;
    private TurnManager turnManager;

    void Start()
    {
        //fryCount = prefab.transform.GetComponentInChildren<FryCount>();   //立方体の子オブジェクトについているFryCountにアクセス
        turnManager = transform.GetComponent<TurnManager>();
        nowTurn = Turn.Thinking;    //最初は考えるターン
        //最初はfalse
        turnChange = false;
        roundEnd = false;
    }

    void Update()
    {
        TurnManager();
    }

    //ターンの全体管理
    void TurnManager()
    {
        //Debug.Log("現在のカウント数" + fryCount.GetFryCount());
        switch (nowTurn)
        {
            case Turn.Thinking:               //選ばれたら切り替える
                roundEnd = false;
                CheckBool(Turn.PutIn);
                break;
            case Turn.PutIn:                  //選ばれたら落とす
                //CheckBool(Turn.Results);
                if (turnChange)
                {
                    turnChange = false;
                    ChangeTurn(Turn.Results);
                    turnManager.CountDown(); //カウントを進める
                    //fryCount.FryCountDown();
                    //Debug.Log("カウントを進める"+fryCount.GetFryCount());
                }
                break;
            case Turn.Results:                //上がるのがなければシンキングに戻るようにする
                
                //試しでの手動ここは削除されるときに呼ぶ
                if(Input.GetKeyDown(KeyCode.Z))
                {
                    turnChange = true;
                    roundEnd = true;
                }
                CheckBool(Turn.Thinking);
                //roundEnd = true;
                break;
            default:
                break;
        }
        text.text = "現在のターン：" + nowTurn;
        //Debug.Log("現在のターン：" + nowTurn);
    }
    //ターン切り替え
    public void ChangeTurn(Turn turn)
    {
        nowTurn = turn;
    }
    //ターン切り替え管理
    void CheckBool(Turn turn)
    {
        if (turnChange)
        {
            turnChange = false;
            ChangeTurn(turn);
        }
    }
    //現在のターンの取得
    public Turn GetTurn()
    {
        return nowTurn;
    }
    //1ローテーションが終わったかどうか
    public bool GetRoundEnd()
    {
        return roundEnd;
    }

    //ターンを切り替える
    public void SetTurnChange()
    {
        turnChange = true;
    }
    ////揚げられるかどうか
    //public bool GetFryCountUp()
    //{
    //    return fryCountUp;
    //}
    ////強制false
    //public void FalseFryCountUp()
    //{
    //    fryCountUp = false;
    //}

}

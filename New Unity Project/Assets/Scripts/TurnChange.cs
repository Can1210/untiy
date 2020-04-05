using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ターン
public enum Turn
{
    Thinking, //選ぶ状態
    PutIn,    //落とした状態
    Results,  //上に上がった状態
}
//ターン切り替えクラス
public class TurnChange : MonoBehaviour
{
    [SerializeField]
    private Text text;              //現在のターンのテキスト
    public Turn nowTurn;            //現在のターン
    private bool turnChange;        //ターンを切り替えるかどうか
    private bool roundEnd;          //1ローテーションの終了
    private TurnManager turnManager;

    private GamePlayManager gameManager;

    void Start()
    {
        turnManager = transform.GetComponent<TurnManager>();
        nowTurn = Turn.Thinking;    //最初は考えるターン
        turnChange = false;         //最初はfalse
        roundEnd = false;           //最初はfalse

        gameManager = GetComponent<GamePlayManager>();
    }

    void Update()
    {
        TurnManager();
    }
    //ターンの全体管理
    void TurnManager()
    {
        switch (nowTurn)
        {
            case Turn.Thinking:               //選ばれたら切り替える
                break;
            case Turn.PutIn:                  //選ばれたら落とす
                for(int j = 0;j < gameManager.useObjects.Count;j++)
                {
                    //  一個でも落ちている奴がいれば
                    if (gameManager.useObjects[j].GetComponent<Block>().currentState == CurrentState.Down)
                    {
                        text.text = "現在のターン：" + nowTurn;
                        return;
                    }
                }

                turnManager.CountDown(gameManager.useObjects);
                int putCount = 0;
                for(int i = 0;i < gameManager.useObjects.Count;i++)
                {

                    //一個でもゼロブロックがあるなら
                    if (gameManager.useObjects[i].GetComponent<Block>().CheckFryCount())
                    {
                        ChangeTurn(Turn.Results);
                    }
                    else
                    {
                        putCount++;
                    }
                }
                //カウントが0のブロックがなかった場合
                if(putCount == gameManager.useObjects.Count)
                {
                    //戻す
                    ChangeTurn(Turn.Thinking);
                }
                break;
            case Turn.Results:
                int resCount = 0;
                for (int i = 0; i < gameManager.useObjects.Count; i++)
                {
                    //止まっていたら
                    if (gameManager.useObjects[i].GetComponent<Block>().currentState == CurrentState.UpStop)
                    {
                        resCount++;
                    }
                }
                if(resCount == gameManager.useObjects.Count)
                {
                    //最初に戻る
                    ChangeTurn(Turn.Thinking);
                }
                break;
            default:
                break;
        }
        text.text = "現在のターン：" + nowTurn;
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

    //意味のないコミットを作る


}

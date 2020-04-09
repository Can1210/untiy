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
    OutOil, //油の外
    Delete, //削除する奴をdにする
    Deleting,//削除中
    Form,//形を合わせる

    Clear,//クリア
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

    private List<GameObject> l = new List<GameObject>();

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
        gameManager.Turn(nowTurn);
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
                for (int j = 0; j < gameManager.useObjects.Count; j++)
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
                for (int i = 0; i < gameManager.useObjects.Count; i++)
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
                if (putCount == gameManager.useObjects.Count)
                {
                    //戻す
                    ChangeTurn(Turn.Thinking);
                }
                break;
            case Turn.Results:
                int resCount = 0;

                if (l.Count <= gameManager.useObjects.Count)
                {
                    for (int c = 0; c < gameManager.useObjects.Count; c++)
                    {
                        if (gameManager.useObjects[c].GetComponent<Block>().currentState == CurrentState.SelfReady ||
                            gameManager.useObjects[c].GetComponent<Block>().currentState == CurrentState.SelfZero)
                        {
                            l.Add(gameManager.useObjects[c]);
                        }
                    }
                }
                for (int i = 0; i < l.Count; i++)
                {
                    //止まっていたら
                    if (l[i].GetComponent<Block>().currentState == CurrentState.UpStop)
                    {
                        resCount++;
                    }
                }
                if (resCount == l.Count && resCount != 0)
                {
                    l = new List<GameObject>();
                    ChangeTurn(Turn.Delete);
                }
                break;

            case Turn.OutOil:
                #region O
                //if (gameManager.InArrayZero())
                //{
                //    for (int i = 0; i < gameManager.useObjects.Count; i++)
                //    {
                //        if (gameManager.useObjects[i].GetComponent<Block>().DownZeroBlock() &&
                //            !gameManager.useObjects[i].GetComponent<Block>().CheckFryCount())
                //        {
                //            Debug.Log(gameManager.useObjects[i].name);
                //            gameManager.useObjects[i].GetComponent<Block>().PositionUp();
                //        }
                //    }
                //}
                #endregion
                break;

            case Turn.Delete:
                #region D
                //if (gameManager.InArrayZero())
                //{
                //    for (int i = 0; i < gameManager.useObjects.Count; i++)
                //    {
                //        if (gameManager.useObjects[i].GetComponent<Block>().DownZeroBlock())
                //        {
                //            gameManager.useObjects[i].GetComponent<Block>().PositionUp();
                //        }
                //    }
                //}
                #endregion

                //ゼロがなかった場合
                gameManager.ZeroOrCube();

                //if (!gameManager.IsZero())
                //{
                //    ChangeTurn(Turn.Deleting);
                //}
                if (gameManager.IsDeath())
                {
                    ChangeTurn(Turn.Deleting);
                }
                break;

            case Turn.Deleting:
                if (!gameManager.IsDesth())
                {
                    
                    for (int i = 0; i < gameManager.useObjects.Count; i++)
                    {
                        gameManager.useObjects[i].GetComponent<Block>().currentState = CurrentState.Down;
                    }
                    ChangeTurn(Turn.Form);
                }
                if(gameManager.moveOk)
                {
                    gameManager.ZeroDesthChangeSpace();
                }
                break;

            //形を判定
            case Turn.Form:
                int forCount = 0;
                for (int i = 0; i < gameManager.useObjects.Count; i++)
                {
                    //止まっていたら
                    if (gameManager.useObjects[i].GetComponent<Block>().currentState == CurrentState.DownStop)
                    {
                        forCount++;
                    }
                }
                if (forCount == gameManager.useObjects.Count)
                {
                    //最初に戻る
                    ChangeTurn(Turn.Thinking);
                }
                //もしも形がで来てなかったら      ChangeTurn(Turn.Thinking);
                //もしも形ができてたら            ChangeTurn(Turn.Clear);
                break;

            case Turn.Clear:
                //終了
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn
{
    Thinking,//選ぶ状態
    PutIn,//落とした状態
    Results,//上に上がった状態
}



//ターン切り替えクラス
public class TurnChange : MonoBehaviour
{
    //現在のターン
    static bool s_turnChange;        //外部から帰らる
    public Turn nowTurn;            //現在のターン

    private bool roundEnd;           //ターンの終了


    // Start is called before the first frame update
    void Start()
    {
        nowTurn = Turn.Thinking;    //最初は考えるターン
        s_turnChange = false;
        roundEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        TurnManager();
    }

    //ターンの管理
    void TurnManager()
    {
        switch (nowTurn)
        {
            case Turn.Thinking://選ばれたら切り替える

                roundEnd = false;
                CheckBool(Turn.PutIn);
                break;
            case Turn.PutIn://選ばれたら落とす

                CheckBool(Turn.Results);
                break;
            case Turn.Results://上がるのがなければシンキングに戻るようにする

                CheckBool(Turn.Thinking);
                roundEnd = true;
                break;
            default:
                break;
        }
    }

    public void ChangeTurn(Turn turn)
    {
        nowTurn = turn;
    }
    void CheckBool(Turn turn)
    {
        if (s_turnChange)
        {
            s_turnChange = false;
            ChangeTurn(turn);
        }
    }

    public Turn GetTurn()
    {
        return nowTurn;
    }
    public bool GetRoundEnd()
    {
        return roundEnd;
    }

}

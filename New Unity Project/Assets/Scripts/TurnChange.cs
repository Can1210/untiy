using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn
{
    Thinking,
    PutIn,
    Results,
}



//ターンを制御するクラス
public class TurnChange : MonoBehaviour
{

    //現在のターン
    static bool s_turnChange;        //外部から帰らる
    private Turn nowTurn;            //現在のターン

    // Start is called before the first frame update
    void Start()
    {
        nowTurn = Turn.Thinking;    //最初は考えるターン
        s_turnChange = false;
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
            case Turn.Thinking:
                CheckBool(Turn.PutIn);
                break;
            case Turn.PutIn:
                CheckBool(Turn.Results);
                break;
            case Turn.Results:
                CheckBool(Turn.Thinking);
                break;
            default:
                break;
        }
    }

    void ChangeTurn(Turn turn)
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
}

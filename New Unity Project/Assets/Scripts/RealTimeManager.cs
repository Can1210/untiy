using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ターン制の廃止のためのリアルタイムでやる  TurnChangeの代わりをここでやる
public class RealTimeManager : MonoBehaviour
{
    [SerializeField]
    private Text conboText;              //コンボのテキスト
    private int nowConbo;
    private GamePlayManager gameManager; //ゲームのマネージャークラス
    private Conbo conbo;                 //コンボ
    private List<GameObject> gameObjectsList = new List<GameObject>();

    private TurnManager fryCountManager; //ターンというより、カウントマネージャーになってる

    private bool isDeleteBlock;          //ブロックを消すかどうか
    private bool isFryCount;             //フライカウントを減らす

    // Start is called before the first frame update
    void Start()
    {
        gameManager     = GetComponent<GamePlayManager>();
        fryCountManager = GetComponent<TurnManager>();
        conbo           = GetComponent<Conbo>();
        isFryCount    = false;           //最初はfalseで初期化
        isDeleteBlock = false;           //最初はfalseで初期化
    }

    // Update is called once per frame
    void Update()
    {
        conboText.text = "現在のコンボ" + nowConbo;
        DeleteBlocks();                  //常に呼ぶが外部で変更されない限りreturnされる
        CountDown();                     //常に呼ぶが外部で変更されない限りreturnされる
    }

    //とりあえず殴り書きする・形になったら名前を変える
    void Scribble()
    {

    }
    //ブロックを消す
    void DeleteBlocks()
    {
        //falseなら早期リターン
        if (!isDeleteBlock) return;
        gameManager.ZeroOrCube();        //自分がゼロじゃなくてspaceでもwallでもない場合   ゼロ以外デスにする
        //消されたものがあるならスコア加算して
        if (gameManager.IsDeath())
        {
            conbo.RemoveCubeCount();     //消された数を計算して、スコアに加算
            if (conbo.GetIsConbo())      //コンボしてたら加算・してなかったら0に戻す
                nowConbo++;
            else
                nowConbo = 0;
            conbo.SetFalseIsConbo();     //コンボを確認したらfalseにする
        }
        isDeleteBlock = false;           //処理が終わったら元に戻す
    }
    //揚げカウントを減らす
    void CountDown()
    {
        //falseなら早期リターン
        if (!isFryCount) return;
        fryCountManager.CountDown(gameManager.useObjects);   //カウントを減らす
        isFryCount = false;              //処理が終わったら元に戻す
    }

    #region GetSet
    //isDeleteBlockの状況を返す
    public bool GetIsDelete()
    {
        return isDeleteBlock;
    }
    //isDeleteBlockを変更する
    public void SetIsDelete(bool delete)
    {
        isDeleteBlock = delete;
    }

    //isFryCountの状況を返す
    public bool GetIsFryCountDown()
    {
        return isFryCount;
    }
    //isFryCountを変更する
    public void SetIsFryCountDown(bool down)
    {
        isFryCount = down;
    }

    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//得点になる形
public enum ScoreModel
{
    VERTICAL,      //1by5
    HORIZONTAL,    //5by1
    BUTA,          //ブタ なんの型にも当てはまらない形  こいつを絶対に一番最後に置く
}

//揚げられたモデルのチェック
public class FryModel : MonoBehaviour
{
    private int modelSquare = 5;      //型のマス数（正方形）

    private int[,,] scoreModels;  //型の数、マス、マス（何かけ何)
    
    private GamePlayManager gameManader;      //gameManager

    const int width = 12;                     //配列のｘの数
    const int height = 22;                    //配列のｙの数
    private InArray[,] inArrays = new InArray[width, height];  //この配列左下が0,0なことを注意する
    private InArray[,] reAtrrays = new InArray[width, height];
    [SerializeField]
    private Text scoreText;     //スコアテキスト
    private int nowScore;

    // Start is called before the first frame update
    void Start()
    {
        scoreModels[0, 0, 0] = 0;
        gameManader = GetComponent<GamePlayManager>();

        ModleRegister();      //完成モデルを設計
        nowScore = 0;          //0で初期化
    }


    void Update()
    {
        ReturnArray_X();    //二次元配列のX軸反転
        scoreText.text = "Scoretext：" + nowScore;   //常に表示
    }

    //配列のX軸反転
    void ReturnArray_X()
    {
        inArrays = gameManader.GetInArrays();   //常に更新
        //逆から入れていていく（ｙだけ）
        for (int y = height; y < 0 ; y--)
        {
            for(int x = 0; x <width;x++)
            {
                reAtrrays[y, x] = inArrays[y, x];
            }
        }
    }


    //揚げられたモデルを調べる
    void ArrayCheck()
    {
        //Y軸
        for (int y = 0; y < height; y++)
        {
            //X軸
            for (int x = 0; x < width; x++)
            {
                //マスが「空白/無し/壁類」なら早期リターン
                if (reAtrrays[y, x] == InArray.Space ||
                    reAtrrays[y, x] == InArray.None ||
                    reAtrrays[y, x] == InArray.Wall ||
                    reAtrrays[y, x] == InArray.UpWall) return;

                //スコアを調べて加算する
                switch (ModelCheck(x,y))
                {
                    case ScoreModel.VERTICAL:
                        nowScore += 100;
                        break;
                    case ScoreModel.HORIZONTAL:
                        nowScore += 100;
                        break;
                    case ScoreModel.BUTA:
                        //今のところ加点無し
                        break;
                    default://それ以外
                        break;
                }
            }
        }
    }

    //当てはまる方を調べる
    ScoreModel ModelCheck(int X, int Y)
    {
        //ブタの一個前まで回す
        for (int id = 0; id < (int)ScoreModel.BUTA - 1; id++)
        {
            //これが通ったら通った時のid = 型を返す
            if (Check(id, X, Y))
            {
                return (ScoreModel)id;
            }
        }
        return ScoreModel.BUTA;        //何も当てはまらないときはブタ
    }

    //型を調べる
    private bool Check(int id,int X,int Y)
    {
        //Y軸
        for (int y = 0; y < modelSquare; y++)
        {
            //X軸
            for (int x = 0; x < modelSquare; x++)
            {
                //配列サイズを越したらcontinueでスキップ
                if ((Y + y > height) || (X + x > width)) continue;

                //型の中身が1かつそれが何かしらのブロックでないとき
                if (!(scoreModels[id, y, x] == 1 &&
                    (
                    reAtrrays[Y + y, X + x] == InArray.Zero ||
                    reAtrrays[Y + y, X + x] == InArray.Cube ||
                    reAtrrays[Y + y, X + x] == InArray.Death ||
                    reAtrrays[Y + y, X + x] == InArray.DownFixedBlock ||
                    reAtrrays[Y + y, X + x] == InArray.Ready ||
                    reAtrrays[Y + y, X + x] == InArray.UpFixedBlock
                    )))
                {
                    return false;
                }

            }
        }
        return true;
    }


    //型の登録
    void ModleRegister()
    {
        //0か1で判断する5*5の中で形を作る
        //1by5
        for (int x = 0; x < modelSquare; x++)
        {
            Debug.Log("今なんかイメ");
            scoreModels[0, x, 0] = 1;
        }
        //5by1
        for (int y = 0; y < modelSquare; y++)
        {
            scoreModels[1, 0, y] = 1;
        }
        //ブタは何もしない
    }
}

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
    const int modelSquare = 5;      //型のマス数（正方形）

    private int[,,] scoreModels;  //型の数、マス、マス（何かけ何)
    
    private GamePlayManager gameManader;      //gameManager

    const int width = 12;                     //配列のｘの数
    const int height = 22;                    //配列のｙの数
    private InArray[,] inArrays = new InArray[width, height];  //この配列左下が0,0なことを注意する
    private InArray[,] reAtrrays = new InArray[width, height];

    private List<List<InArray>> removeList = new List<List<InArray>>();  //これでいいのかわからんけれど一応上のものの動的用
  
    [SerializeField]
    private Text scoreText;     //スコアテキスト
    private int nowScore;
    private bool isConbo;

    // Start is called before the first frame update
    void Start()
    {
        gameManader = GetComponent<GamePlayManager>();

        ModleRegister();       //完成モデルを設計
        nowScore = 0;          //0で初期化
        removeList.Clear();    //空で初期化
        

        //最初にAddしとく
        //Y軸
        for (int y = 0; y < height; y++)
        {
            //X軸
            for (int x = 0; x < width; x++)
            {
                removeList.Add(new List<InArray>());
                removeList[x].Add(InArray.Space);
            }
        }
    }


    void Update()
    {
        //ReturnArray_X();    //二次元配列のX軸反転
        SetRemoveList();
        scoreText.text = "Scoretext：" + nowScore;   //常に表示
    }

    //配列のX軸反転
    void ReturnArray_X()
    {
        inArrays = gameManader.GetInArrays();   //常に更新
        int a = 0;  //助け船
        //逆から入れていていく（ｙだけ）
        for (int y = height-1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                reAtrrays[x, a] = inArrays[x, y];
            }
            a++;  //最後に足す
        }
    }

    
    //消されるやつのリストの制作     一応反転しとく
    void SetRemoveList()
    {
        inArrays = gameManader.GetInArrays();   //常に更新
        int a = 0;  //助け船
        for (int y = height - 1; y >= 0; y--)
        {
            //消される列を調べて、trueなら現在のy軸を保存する          
            if (getDeathAreaY(y))
            {
                for (int x = 0; x < width; x++)
                {
                    removeList[x][a] = inArrays[x, y];  //これは反転してない
                }
                    
            }
            else
            {
                for (int x = 0; x < width; x++)
                {
                    removeList[x][a] = InArray.Space;    //falseだとスペースを入れる
                }
            }
            a++;
        }
    }

    //デスエリアの列を調べる
    bool getDeathAreaY(int y)
    {

        //デスエリアになるものは、「InArray.Zero、InArray.Death」のy軸なのでそれがtureなら何か、その段の数を返し、新しい配列にいれる、それ以外の配列は0かスペースを入れる

        //X軸
        for (int x = 0; x < width; x++)
        {
            if (inArrays[x, y] == InArray.Zero || inArrays[x, y] == InArray.Death)
            {
                return true;
            }
        }

        return false;
    }


    //揚げられたモデルを調べる
    public void ArrayCheck()
    {
        //Y軸
        for (int y = 0; y < height; y++)
        {
            //X軸
            for (int x = 0; x < width; x++)
            {
                ////マスが「空白/無し/壁類」なら早期リターン
                //if (reAtrrays[x, y] == InArray.Space ||
                //    reAtrrays[x, y] == InArray.None ||
                //    reAtrrays[x, y] == InArray.Wall ||
                //    reAtrrays[x, y] == InArray.UpWall) continue;
                //マスが「空白/無し/壁類」なら早期リターン
                if (removeList[x] [y] == InArray.Space ||
                    removeList[x] [y] == InArray.None ||
                    removeList[x] [y] == InArray.Wall ||
                    removeList[x] [y] == InArray.UpWall) continue;

                //スコアを調べて加算する
                switch (ModelCheck(x,y))
                {
                    case ScoreModel.VERTICAL:
                        nowScore += 100;
                        Debug.Log("横棒");
                        break;
                    case ScoreModel.HORIZONTAL:
                        nowScore += 100;
                        Debug.Log("縦棒");
                        break;
                    case ScoreModel.BUTA:
                        Debug.Log("ブタ");
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
                if ((Y + y >= height) || (X + x >= width)) continue;
                
                ////型の中身が1かつそれが何かしらのブロックでないとき
                //if ((scoreModels[id, x, y] == 0 &&
                //    (
                //    reAtrrays[X + x, Y + y] == InArray.Zero ||
                //    reAtrrays[X + x, Y + y] == InArray.Cube ||
                //    reAtrrays[X + x, Y + y] == InArray.Death ||
                //    reAtrrays[X + x, Y + y] == InArray.DownFixedBlock ||
                //    reAtrrays[X + x, Y + y] == InArray.Ready ||
                //    reAtrrays[X + x, Y + y] == InArray.UpFixedBlock
                //    )))
                //{
                //    return false;
                //}

                //型の中身が1かつそれが何かしらのブロックでないとき
                if ((scoreModels[id, x, y] == 0 &&
                    (
                    removeList[X + x] [Y + y] == InArray.Zero ||
                    removeList[X + x] [Y + y] == InArray.Cube ||
                    removeList[X + x] [Y + y] == InArray.Death ||
                    removeList[X + x] [Y + y] == InArray.DownFixedBlock ||
                    removeList[X + x] [Y + y] == InArray.Ready ||
                    removeList[X + x] [Y + y] == InArray.UpFixedBlock
                    )) ||
                    (scoreModels[id, x, y] == 1 && !(
                    removeList[X + x][Y + y] == InArray.Zero ||
                    removeList[X + x][Y + y] == InArray.Cube ||
                    removeList[X + x][Y + y] == InArray.Death ||
                    removeList[X + x][Y + y] == InArray.DownFixedBlock ||
                    removeList[X + x][Y + y] == InArray.Ready ||
                    removeList[X + x][Y + y] == InArray.UpFixedBlock
                    )))
                {
                    return false;
                }

            }
        }
        //一つでも型があったらコンボをtrueに返す
        isConbo = true;
        return true;
    }
    //コンボをしているかどうか
    public bool GetIsConbo()
    {
        return isConbo;
    }
    //コンボをfalseに変える
    public void SetFalseIsConbo()
    {
        isConbo = false;
    }


    //型の登録
    void ModleRegister()
    {
        scoreModels = new int[3, modelSquare, modelSquare];
        //0か1で判断する5*5の中で形を作る
        //1by5
        for (int x = 0; x < 4; x++)
        {
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

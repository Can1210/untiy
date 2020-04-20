using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//消されたCubuの数に応じてコンボ加算
public class Conbo : MonoBehaviour
{
    private GamePlayManager gameManager;       //ゲームマネージャー
    [SerializeField]
    private Text scoreText;                    //コンボのテキストUI
    private int nowScore;                      //現在のコンボ

    private bool isConbo;
    const int width = 12;                      //配列のｘの数
    const int height = 22;                     //配列のｙの数
    private InArray[,] inArrays = new InArray[width, height];               //ゲームマネージャーの配列をコピー
    private List<List<InArray>> removeList = new List<List<InArray>>();     //これでいいのかわからんけれど一応上のものの動的用

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GamePlayManager>();
        Initialize();
    }
    //初期化
    private void Initialize()
    {
        nowScore = 0;
        removeList.Clear();    //空で初期化
        //最初にAddしとく  サイズの確保
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

    // Update is called once per frame
    void Update()
    {
        SetRemoveList();
        scoreText.text = "現在のコンボ：" + nowScore;    //常に表示
    }

    //消されるやつのリストの制作     一応反転しとく
    void SetRemoveList()
    {
        inArrays = gameManager.GetInArrays();   //常に更新
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
    //消されたCubeの数を数える
    public void RemoveCubeCount()
    {

        int count = 0;   //Cube数を数えるカウント（ローカル）
        //Y軸
        for (int y = 0; y < height; y++)
        {
            //X軸
            for (int x = 0; x < width; x++)
            {
                //マスが「空白/無し/壁類」ならスキップ
                if (removeList[x][y] == InArray.Space ||
                    removeList[x][y] == InArray.None ||
                    removeList[x][y] == InArray.Wall ||
                    removeList[x][y] == InArray.UpWall) continue;
                
                count++;   //ブロックなら加算
            }
        }
        Debug.Log("今のコンボ"+ count);
        nowScore += count*100;     //スコア加算
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
}

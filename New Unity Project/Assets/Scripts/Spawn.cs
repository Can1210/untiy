using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//選択制＋選択リストにランダムに追加されていく
public class Spawn : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;     //登録するオブジェクト
    private TurnManager turnManager;  //揚げカウントの管理
    //他から干渉できるようにpublic
    public List<GameObject> InstansObject;
    //とりあえず6種類の中から使えるオブジェクトを選ぶランダムで6個
    private List<GameObject> randObjects = new List<GameObject>();
    private int choiceCount;     //選択制の廃止によりこれはずっと0
    [SerializeField]
    private int showNum;         //待機オブジェクトを見せる数
    private GamePlayManager playManager;
    private int moveSide;

    void Awake()
    {
        InstansObject = new List<GameObject>();
        choiceCount = 0;     //最初は一番上を選択（上から順番に選択される）
        moveSide = 0;        //最初は動かない
        turnManager = GameObject.Find("GamePlayManager").GetComponent<TurnManager>();
        playManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();

        for (int i = 0; i < showNum; i++)
        {
            //ランダムで生成
            int rndObj = Random.Range(0, objects.Length);   //ランダムで選ぶ
            randObjects.Add(objects[rndObj]);//こいつは何のオブジェクトが選ばれたか、わかるforぶんで出せばね
            //横に生成
            Vector3 v = new Vector3(15, 18 - (3 * i), 0);    //上から順に生成
            GameObject g = Instantiate(randObjects[i], v, Quaternion.identity);
            InstansObject.Add(g);
        }
    }
    void Update()
    {
        SpawnBlock();
        CountMove();
        Alignment();   //整列する
    }

    //ブロックの生成
    void SpawnBlock()
    {

        //Aボタンを押されてたらpositionを変更    Thinking時しか押せない
        if (Input.GetKeyDown(KeyCode.A))
        {
            //指定制に変更
            int index = choiceCount;
            turnManager.Add(InstansObject[index].GetComponentsInChildren<Transform>());
            //  ここから下はべつのところでやったほうがいい気がする
            InstansObject[index].transform.localScale = new Vector3(1, 1, 1);  //大きさを元に戻す
            InstansObject[index].transform.position = transform.position;
            //落ちるようにする
            InstansObject[index].GetComponent<RealTimeBlock>().currentState = CurrentState.Down;
            //使っているオブジェクトを格納

            //GamePlayManagerに登録
            playManager.UseObj(InstansObject[index]);
            InstansObject.Remove(InstansObject[index]);

            
            //一個追加
            int rndObj = Random.Range(0, objects.Length);   //ランダムで選ぶ
            randObjects.Add(objects[rndObj]);//こいつは何のオブジェクトが選ばれたか、わかるforぶんで出せばね
            //横に生成
            Vector3 v = new Vector3(15, (18 - 3), 0);    //上から順に生成
            int listBack = randObjects.Count()-1;
            GameObject g = Instantiate(randObjects[listBack], v, Quaternion.identity);
            InstansObject.Add(g);
        }
    }
    //整列
    void Alignment()
    {

        for (int i = 0; i < showNum; i++)
        {
            //InstansObject[i].transform.position = new Vector3(15, 18 - (3 * i), 0);  //上から古い順に整列
            if (i == choiceCount)
            {
                //InstansObject[i].transform.localScale = new Vector3(2, 2, 1);  //選択されている
                InstansObject[i].transform.position = transform.position;        //選択されている
            }
            else
            {
                if(i<choiceCount) InstansObject[i].transform.position = new Vector3(15, 18 - (3 * i+1), 0);  //選択されていない
                InstansObject[i].transform.position = new Vector3(15, 18 - (3 * i), 0);  //選択されていない
            }
        }
    }

    //数字の制御
    void CountMove()
    {
        moveSide = 0;    //毎回初期化する

        if (Input.GetKeyDown(KeyCode.UpArrow)) choiceCount--;
        if (Input.GetKeyDown(KeyCode.DownArrow)) choiceCount++;

        //左右移動（生成位置自体）
        if (Input.GetKeyDown(KeyCode.LeftArrow)) moveSide = -1;
        if (Input.GetKeyDown(KeyCode.RightArrow)) moveSide = 1;
        transform.position += new Vector3(moveSide, 0, 0);

        if (choiceCount <= 0) choiceCount = 0;  //0以下にはしない
        if (choiceCount >= InstansObject.Count - 1) choiceCount = InstansObject.Count - 1; //オブジェクトの数を超えない
    }
}

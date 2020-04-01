using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;


//指定されたものを落とすスポーンクラス
public class Spawn : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;     //登録するオブジェクト
    private TurnChange gameManager;
    private TurnManager turnManager;  //揚げカウントの管理
    //他から干渉できるようにpublic
    public List<GameObject> InstansObject;
    //とりあえず6種類の中から使えるオブジェクトを選ぶランダムで6個
    private List<GameObject> randObjects = new List<GameObject>();
    private int objIndex = 0;
    private int choiceCount;     //オブジェクト選択画面

    // Start is called before the first frame update
    void Awake()
    {
        InstansObject = new List<GameObject>();
        choiceCount = 0;     //最初は一番上を選択（上から順番に選択される）
        gameManager = GameObject.Find("GamePlayManager").GetComponent<TurnChange>();
        turnManager = GameObject.Find("GamePlayManager").GetComponent<TurnManager>();
        for (int i = 0; i < objects.Length; i++)
        {
            //int rnd = Random.Range(0, objects.Length);

            //randObjects.Add(objects[rnd]);//こいつは何のオブジェクトが選ばれたか、わかるforぶんで出せばね
            randObjects.Add(objects[i]);//こいつは何のオブジェクトが選ばれたか、わかるforぶんで出せばね

            //横に生成
            Vector3 v = new Vector3(15, 3 + (3 * i), 0);

            GameObject g = Instantiate(randObjects[i], v, Quaternion.identity);
            InstansObject.Add(g);
            //InstansObject[i].GetComponent<Block>().ChildrenStop();

            objIndex += 1;//何個オブジェクトを使えるか
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnBlock();
        CountMove();
    }

    //ブロックの生成
    void SpawnBlock()
    {
        if (objIndex <= 0)
        {
            //もう使えるオブジェクトがなければreturn
            //Debug.Log("使えるオブジェクトがない");
            return;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {

        }

        //Aボタンを押されてたらpositionを変更
        if (Input.GetKeyDown(KeyCode.A))
        {
            //指定制に変更
            int index = choiceCount;
            turnManager.Add(InstansObject[index].GetComponentsInChildren<Transform>());
            //何を出すかランダムにする
            //int index = Random.Range(0, InstansObject.Count);
            //  ここから下はべつのところでやったほうがいい気がする
            InstansObject[index].transform.localScale = new Vector3(1, 1, 1);  //大きさを元に戻す
            InstansObject[index].transform.position = transform.position;
            //InstansObject[index].GetComponent<Block_test>().ChildrenMove(); //動くようにする
            //InstansObject[index].GetComponent<Block_test>().isStop = false; //ここだけ変えた
            InstansObject.Remove(InstansObject[index]);
            objIndex--;
            gameManager.SetTurnChange();  //ターンを切り替える
        }
    }
    //数字の制御
    void CountMove()
    {
        for (int i = 0; i < InstansObject.Count; i++)
        {
            if (i == choiceCount)
                InstansObject[i].transform.localScale = new Vector3(2, 2, 1);  //選択されている
            else
                InstansObject[i].transform.localScale = new Vector3(1, 1, 1);  //選択されていない
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) choiceCount++;    //上に選択を変更
        if (Input.GetKeyDown(KeyCode.DownArrow)) choiceCount--;  //下に選択を変更

        if (choiceCount <= 0) choiceCount = 0;  //0以下にはしない
        if (choiceCount >= InstansObject.Count - 1) choiceCount = InstansObject.Count - 1; //オブジェクトの数を超えない
        //Debug.Log("今の数字" + choiceCount);
    }
}

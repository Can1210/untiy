using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;


//指定されたものを落とすスポーンクラス
public class Spawn : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;    //登録するオブジェクト

    //他から干渉できるようにpublic
    public List<GameObject> InstansObject;

    //とりあえず6種類の中から使えるオブジェクトを選ぶランダムで6個
    private List<GameObject> randObjects = new List<GameObject>(); 

    private Rigidbody rigidbody;

    private int objIndex = 0;

    // Start is called before the first frame update
    void Start()
    { 
        InstansObject = new List<GameObject>();

        for (int i = 0;i < objects.Length; i++)
        {
            int rnd = Random.Range(0, objects.Length);

            randObjects.Add(objects[rnd]);//こいつは何のオブジェクトが選ばれたか、わかるforぶんで出せばね

            //横に生成
            Vector3 v = new Vector3(10, 3 + (3 * i), 0);

            GameObject g = Instantiate(randObjects[i], v, Quaternion.identity);
            InstansObject.Add(g);
            InstansObject[i].GetComponent<Block>().ChildrenStop();

            objIndex += 1;//何個オブジェクトを使えるか
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnBlock();
    }   

    //ブロックの生成
    void SpawnBlock()
    {
        if(objIndex <= 0)
        {
            //もう使えるオブジェクトがなければreturn
            //Debug.Log("使えるオブジェクトがない");
            return;
        }

        //Aボタンを押されてたらpositionを変更
        if(Input.GetKeyDown(KeyCode.A))
        {
            //何を出すかランダムにする
            int index = Random.Range(0, InstansObject.Count);

            //  ここから下はべつのところでやったほうがいい気がする

            InstansObject[index].transform.position = transform.position;
            InstansObject[index].GetComponent<Block>().ChildrenMove(); //動くようにする

            InstansObject.Remove(InstansObject[index]);

            objIndex--;
        }
    }
}

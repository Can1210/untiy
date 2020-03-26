using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//指定されたものを落とすスポーンクラス
public class Spawn : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;    //登録するオブジェクト

    private GameObject[] randObjects = new GameObject[6]; //とりあえず6種類の中から使えるオブジェクトを選ぶランダムで6個

    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {

        for(int i = 0;i < objects.Length; i++)
        {
            int rnd = Random.Range(0, objects.Length);

            randObjects[i] = objects[rnd];

            //横に生成
            Vector3 v = new Vector3(10, 3 + (3 * i), 0);

            GameObject g = Instantiate(randObjects[i], v , Quaternion.identity);
            g.GetComponent<BlockMove>().isStop = true;
        }

        foreach(var i in randObjects)
        {
            //確認なにが出てくるか
            Debug.Log(i);
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


        //Aボタンを押されてたら生成
        if(Input.GetKeyDown(KeyCode.A))
        {
            //何を出すかランダムにする
            int a = Random.Range(0, objects.Length);


            //オブジェクトの生成
            Instantiate(objects[a], transform.position, Quaternion.identity);
        }
    }
}

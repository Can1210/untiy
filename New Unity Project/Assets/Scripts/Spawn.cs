using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//指定されたものを落とすスポーンクラス
public class Spawn : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;    //登録するオブジェクト
    [SerializeField]
    private Vector3 spwanPos;        //生成位置

    
    // Start is called before the first frame update
    void Start()
    {
        
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
            int a = Random.Range(0, objects.Length-1);
            //オブジェクトの生成
            Instantiate(objects[a], transform.position, Quaternion.identity);
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//じゅんばんを変えるスクリプト
public class ProOrder : MonoBehaviour
{
    private GamePlayManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GamePlayManager>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i < gameManager.useObjects.Count; i++)
        {

            if(gameManager.useObjects[i].GetComponent<RealTimeBlock>().currentState == CurrentState.DownReSpawn)
            {
                //オブジェクトを削除
                Destroy(gameManager.useObjects[i]);
                //生成
                GameObject instance = Instantiate(gameManager.useObjects[i]);

                instance.GetComponent<RealTimeBlock>().enabled = true;
                instance.GetComponent<RealTimeBlock>().InsCube();
                instance.GetComponent<RealTimeBlock>().isIns = true;

                //生成したオブジェクトをりすとに追加
                gameManager.UseObj(instance);

                //Destroyしたオブジェクトをリストから消す
                gameManager.useObjects.Remove(gameManager.useObjects[i]);              
            }
        }
    }
}

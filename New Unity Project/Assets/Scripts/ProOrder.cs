using System.Collections;
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
            //リセット依頼で追加
            if (gameManager.useObjects[i] == null) return;

            if(gameManager.useObjects[i].GetComponent<Block>().currentState == CurrentState.DownReSpawn)
            {
                //オブジェクトを削除
                Destroy(gameManager.useObjects[i]);
                //生成
                GameObject instance = Instantiate(gameManager.useObjects[i]);

                instance.GetComponent<Block>().enabled = true;
                instance.GetComponent<Block>().InsCube();
                instance.GetComponent<Block>().isIns = true;

                //生成したオブジェクトをりすとに追加
                gameManager.UseObj(instance);

                //Destroyしたオブジェクトをリストから消す
                gameManager.useObjects.Remove(gameManager.useObjects[i]);              
            }
        }
    }
}

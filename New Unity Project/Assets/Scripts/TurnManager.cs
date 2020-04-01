using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ターンの管理
public class TurnManager : MonoBehaviour
{
    private List<FryCount> objList;

    void Start()
    {
        objList = new List<FryCount>();
        objList.Clear(); //最初は空にする
    }

    //オブジェクトの追加
    public void Add(Transform[] gameObject)
    {
        //現状0～8    0368
        for (int i =0;i <gameObject.Length;i++)
        {
            if (i == 0 || i==3 || i== 6|| i ==8)
            {
                var kari = gameObject[i].GetComponentInChildren<FryCount>();
                objList.Add(kari);   //子供の子供
            }
        }
        
    }
    //一斉カウントを進める
    public void CountDown()
    {
        foreach (var obj in objList)
        {
            obj.FryCountDown();
            //Debug.Log("今のカウント" + obj.GetFryCount());
        }
    }
    //削除
    public void RemoveObj()
    {

    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ターンの管理というかなんかFryCountManagerになっている気がする名前変えた方が絶対委員会
public class TurnManager : MonoBehaviour
{
    private List<FryCount> objList;     //生成されたオブジェクトのリスト

    void Start()
    {
        objList = new List<FryCount>();
        objList.Clear();                //最初は空にする
    }

    //オブジェクトの追加
    public void Add(Transform[] gameObject)
    {
        //現状0～8    0368  決め打ちだけどスンマソ
        for (int i =0;i <gameObject.Length;i++)
        {
            if (i == 0 || i==3 || i== 6|| i ==8)
            {
                var kari = gameObject[i].GetComponentInChildren<FryCount>();
                objList.Add(kari);   //孫
            }
        }
    }
    //一斉にカウントを進める
    public void CountDown(List<GameObject> list)
    {
        foreach(var l in list)
        {
            if (!(l.GetComponent<RealTimeBlock>().GetNoFry())) continue;    //入ってきたばかりの物はカウントを進めない

            l.GetComponent<RealTimeBlock>().FryCountDown();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ターンの管理
public class TurnManager : MonoBehaviour
{
    private GameObject turnChangeObj;     //ターンを管理しているクラス
    private TurnChange turnChange;

    private List<GameObject> objList;


    // Start is called before the first frame update
    void Start()
    {
        turnChange = GetComponent<TurnChange>();
        objList.Clear(); //最初は空にする
    }

    // Update is called once per frame
    void Update()
    {

        AdvanceTurn();
    }

    //オブジェクトの追加
    public void Add(GameObject gameObject)
    {
        objList.Add(gameObject);
    }



    void RemoveObj()
    {
        foreach(var obj in objList)
        {
            //オブジェクトがないなら  ここisEndFlagを基底クラスで作る
            if (obj)
                objList.Remove(obj);
        }
    }

    //全体確認
    void OverallConfirmation()
    {

        //bool check = objList.TrueForAll(isActiveAndEnabled);
    }

    //ターンを経過させる
    void AdvanceTurn()
    {
        if(turnChange.GetRoundEnd())
        {
            Debug.Log("ターンを一つ進める");
        }
    }
}

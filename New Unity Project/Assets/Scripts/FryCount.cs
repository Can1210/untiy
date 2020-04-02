using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FryCount : MonoBehaviour
{
    [SerializeField]
    private Text text;      //テキスト
    private Text childObject;
    //private TurnChange gameManager;
    [SerializeField]
    private int fryNum;    //テスト用

    void Start()
    {
        //gameManager = GameObject.Find("GamePlayManager").GetComponent<TurnChange>();
        Vector3 a = gameObject.transform.position;
        childObject = Instantiate(text, RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(a.x, a.y, 0)), Quaternion.identity);
        
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        childObject.transform.SetParent(canvas.transform);
    }
    
    void Update()
    {
        if (fryNum <= 0) fryNum = 0;  //0以下にはならない
        childObject.text = fryNum.ToString();

        Vector3 a = gameObject.transform.position;
        childObject.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(a.x, a.y, 0));

    }
    public void FryCountDown()
    {
        fryNum--;
        ////フライカウントを下げる
        //if (gameManager.GetFryCountUp())
        //{
            
        //    Debug.Log("カウントダウン"+ gameManager.GetFryCountUp());
        //    gameManager.FalseFryCountUp();  //強制falseにする
        //}
    }

    public int fryCount
    {
        set { this.fryNum = value; }
        get { return this.fryNum; }
    }

    public int GetFryCount()
    {
        return fryNum;
    }
}

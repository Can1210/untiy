using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//揚げカウント
public class FryCount : MonoBehaviour
{
    [SerializeField]
    private Text text;      //テキスト
    public Text childObject;
    [SerializeField]
    private int fryNum;    //揚げカウント

    void Start()
    {
        Vector3 a = gameObject.transform.position;
        childObject = Instantiate(text, RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(a.x, a.y, 0)), Quaternion.identity);
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        childObject.transform.SetParent(canvas.transform);           //SetParent()　大事！
    }
    
    void Update()
    {
        //リセット依頼で追加
        if (Input.GetKeyDown(KeyCode.R))
        {
            Destroy(childObject);
        }
        if (fryNum <= 0) fryNum = 0;                                 //0以下にはならない
        childObject.text = fryNum.ToString();
        Vector3 a = gameObject.transform.position;
        childObject.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(a.x, a.y, 0));
    }
    //カウントを減らす
    public void FryCountDown()
    {
        fryNum--;
    }

    public int fryCount
    {
        set { this.fryNum = value; }
        get { return this.fryNum; }
    }
    //現在のカウント
    public int GetFryCount()
    {
        return fryNum;
    }
}

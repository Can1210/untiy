using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FryCount : MonoBehaviour
{
    [SerializeField]
    private Text text;      //テキスト
    private Text childObject;

    [SerializeField]
    private int fryNum;    //テスト用

    // Start is called before the first frame update
    void Start()
    {
        Vector3 a = gameObject.transform.position;
        childObject = Instantiate(text, RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(a.x, a.y, 0)), Quaternion.identity);
        
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        childObject.transform.parent = canvas.transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (fryNum <= 0) fryNum = 0;  //0以下にはならない
        childObject.text = fryNum.ToString();
        Vector3 a = gameObject.transform.position;

        childObject.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(a.x, a.y, 0));
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

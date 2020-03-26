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
    private int testNum;    //テスト用

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
        childObject.text = testNum.ToString();
        Vector3 a = gameObject.transform.position;

        childObject.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(a.x, a.y, 0));
    }
}

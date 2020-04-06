using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelClear : MonoBehaviour
{
    private GamePlayManager playManager;
    

    //個数は合わせる
    [SerializeField]
    private int[] x_Model;     //x軸情報
    [SerializeField]
    private int[] y_Model;     //x軸情報

    const int width = 12;
    const int height = 22;

    private InArray[,] inArrays = new InArray[width, height];  //GamePlayManagerに合わせる



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ModelArray();
    }
    //型の情報をつける
    void ModelArray()
    {
        for (int i = 0; i < x_Model.Length; ++i)
        {
            Debug.Log(x_Model[i] + "," + y_Model[i]);
        }
    }
}

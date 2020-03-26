using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private GameObject[] children;

    //なんのBLOCKか
    public enum BlockType {
        O_BLOCK = 4,//Cubeの数が4
        T_BLOCK = 4,
        J_BLOCK = 4,
        L_BLOCK = 4,
        S_BLOCK = 4,
        Z_BLOCK = 4    };

    // Start is called before the first frame update
    void Start()
    {
        //children = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

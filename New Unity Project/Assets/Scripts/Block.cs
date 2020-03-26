using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private BlockMove[] childrenMove;
    //なんのBLOCKか
    public enum BlockType {
        O_BLOCK = 4,//Cubeの数が4
        T_BLOCK = 4,
        J_BLOCK = 4,
        L_BLOCK = 4,
        S_BLOCK = 4,
        Z_BLOCK = 4    };

    // Start is called before the first frame update
    void Awake()
    {
        childrenMove = transform.GetComponentsInChildren<BlockMove>();
        //children = 
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChildrenStop()
    {
        for (int i = 0; i < childrenMove.Length; i++)
        {
            childrenMove[i].isStop = true;
        }
    }

    public void ChildrenMove()
    {
        for (int i = 0; i < childrenMove.Length; i++)
        {
            childrenMove[i].isStop = false;
        }
    }
}

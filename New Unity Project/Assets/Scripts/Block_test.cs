using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_test : MonoBehaviour
{
    private BlockMove_test[] childrenMove;
    //なんのBLOCKか
    public enum BlockType
    {
        
    };

    GamePlayManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();

        childrenMove = transform.GetComponentsInChildren<BlockMove_test>();
        //children = 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.position = gameManager.ZeroPosition(transform.position);
        }
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

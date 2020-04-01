using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//処理順
public enum ProcessingOrder
{
    First = 1,
    Sceond = 2,
    Third = 3,
    Fourth = 4,
    Fifth = 5,
    Sixth = 6,
    Seventh = 7,
    Eigth = 8,
    Ninth = 9,
    Tenth = 10,
}
public class BlockManager : MonoBehaviour
{
    //SpawnクラスのInstanObjectが入る
    private List<GameObject> usableBlocks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}

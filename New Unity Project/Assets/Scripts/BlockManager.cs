using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    //public int turn;

    //SpawnクラスのInstanObjectが入る
    //使えるブロック達List
    public List<GameObject> usableBlocks;

    //使えるブロックとターン数
    public Dictionary<List<GameObject>, int> blockState;

    // Start is called before the first frame update
    void Start()
    {
        usableBlocks = GetComponent<Spawn>().InstansObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(usableBlocks.Count);
    }
}

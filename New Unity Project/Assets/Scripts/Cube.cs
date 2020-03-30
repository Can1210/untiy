using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public int turn;

    //enumで上がるターンが来たらターンを減らすだけのスクリプト
    private GamePlayManager gameManager;

    private Block_test blocktest;

    private Vector3 previous;

    //自分のところに行けない場所(WallとCube)があったらtrueを返す用
    public bool isMoveOMG;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();
        blocktest = GetComponentInParent<Block_test>();       

        previous = new Vector3(1,1,0);//ゼロだと都合が悪い
        isMoveOMG = false;
    }

    // Update is called once per frame
    void Update()
    {
        //3/31
        gameManager.inCubeArray(transform.position,previous);

        if(blocktest.isStop)
        {
            gameManager.FixedBlock(transform.position);
        }

        previous = transform.position;//前のポジション
    }
}

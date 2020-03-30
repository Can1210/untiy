using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public int turn;

    //enumで上がるターンが来たらターンを減らすだけのスクリプト
    private GamePlayManager gameManager;

    private BlockMove_test blockMove_t;

    private Vector3 previous;

    //自分のところに行けない場所(WallとCube)があったらtrueを返す用
    public bool isMoveOMG;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();
        blockMove_t = GetComponent<BlockMove_test>();//まだ使ってない使ったら消すコメ

        previous = new Vector3(1,1,0);//ゼロだと都合が悪い
        isMoveOMG = false;
    }

    // Update is called once per frame
    void Update()
    {
        gameManager.inCubeArray(transform.position,previous);


        previous = transform.position;//前のポジション
    }
}

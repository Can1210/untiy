using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public int turn;

    //enumで上がるターンが来たらターンを減らすだけのスクリプト
    private GamePlayManager gameManager;

    private BlockMove_test blockMove_t;

    Vector3 previous;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();
        blockMove_t = GetComponent<BlockMove_test>();//まだ使ってない使ったら消すコメ

        previous = new Vector3(1,1,0);//ゼロだと都合が悪い
    }

    // Update is called once per frame
    void Update()
    {
        gameManager.inCubeArray(transform.position,previous);

        previous = transform.position;//前のポジション
    }
}

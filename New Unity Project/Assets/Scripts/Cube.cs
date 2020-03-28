using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public int turn;

    //enumで上がるターンが来たらターンを減らすだけのスクリプト
    GamePlayManager gameManager;

    Vector3 previous;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();
        previous = new Vector3(1,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        gameManager.inCubeArray(transform.position,previous);

        previous = transform.position;//前のポジション
    }
}

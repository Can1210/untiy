using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove_test : MonoBehaviour
{

    private bool anyTouch;

    [SerializeField]
    private int downSpeed = -1;  //落ちるスピード
    [SerializeField]
    private int upSpeed;    //上昇スピード

    private Rigidbody rigid;

    public bool isStop;

    GamePlayManager gameManager;

    private float stayTime;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();

        isStop = true;

        stayTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        stayTime++;

        Move();
    }

    //移動
    void Move()
    {
        if (isStop)
        {
            return;
        }
            //下に移動し続ける
        transform.position += new Vector3(0, downSpeed, 0) * Time.deltaTime;
        Debug.Log(transform.position);

        if(gameManager.UnderMap(transform.position))
        {
            isStop = true;
        }
    }
}

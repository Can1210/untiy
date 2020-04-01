using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove_test : MonoBehaviour
{

    //移植したところは消してもいい使ってない
    [SerializeField]
    private int downSpeed = -1;  //落ちるスピード
    [SerializeField]
    private int upSpeed;    //上昇スピード

    public bool isStop;

    GamePlayManager gameManager;

    private float time;
    private int currenTime;

    private bool moveOk;
    public bool nowMove{
        set { this.moveOk = value; }
        get { return this.moveOk; }
        }

    private int xPos = 0;
    private Vector3 dic;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

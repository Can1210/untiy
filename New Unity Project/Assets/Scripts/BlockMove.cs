using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{

    private bool anyTouch;

    [SerializeField]
    private float downSpeed;  //落ちるスピード
    [SerializeField]
    private float upSpeed;    //上昇スピード

    private Rigidbody rigid;

    public bool isStop;

    private float stayTime;
    // Start is called before the first frame update
    void Start()
    {
        isStop = true;

        anyTouch = false;
        rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = false;

        stayTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        stayTime++;

        Move();
    }

    private void OnCollisionEnter(Collision col)
    {
        if(stayTime < 0.1f)
        {
            return;
        }

        //if (col.gameObject.tag == "Wall")
        //{
        //    isStop = true;      
        //}

        if (anyTouch) return;
        rigid.isKinematic = true;
        anyTouch = true;
    }

    //移動
    void Move()
    {
        if (isStop)
        {
            return;
        }

        if (!anyTouch)
        {
            //下に移動し続ける
            transform.position += new Vector3(0, downSpeed, 0)* Time.deltaTime;
        }
        
    }

}

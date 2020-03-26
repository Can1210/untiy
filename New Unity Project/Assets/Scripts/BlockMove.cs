using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{

    private bool anyTouch;

    [SerializeField]
    private float downSpeed;  //落ちるスピード


    private Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        anyTouch = false;
        rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (anyTouch) return;
        rigid.isKinematic = true;
        anyTouch = true;

    }

    //移動
    void Move()
    {
        if(!anyTouch)
        {
            //下に移動し続ける
            transform.position += new Vector3(0, downSpeed, 0)* Time.deltaTime;
        }
        
    }




}

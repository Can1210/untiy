using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//自分を消すためだけの物テクストをリセットするためにつく多
public class DesText : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Destroy(this.gameObject);
        }
    }
}

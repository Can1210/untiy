using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnText : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private GameObject obj;
    private Turn nowTurn;

    // Start is called before the first frame update
    void Start()
    {
        nowTurn = obj.GetComponent<TurnChange>().GetTurn();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "現在のターン："+nowTurn;
    }
}

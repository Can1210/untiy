using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance = null;

    const int width = 10;
    const int height = 20;

    public Vector3[,] worldPos = new Vector3[width,height];

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);  //シーンを切り替えても消えない
        }
        else
        {
            Destroy(this.gameObject);
        }

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                worldPos[x, y] = new Vector3( x, y, 0);
                //Debug.Log(worldPos[x, y]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CurrentPos()
    {
       
    }

    public Vector3 MapOn(Vector3 p)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(p == worldPos[x, y])
                {
                    return worldPos[x, y];
                }
                //Debug.Log(worldPos[x, y]);
            }
        }
        return p;
    }

    public bool UnderMap(Vector3 p)//下にいるとき
    {
        for (int x = 0; x < width; x++)
        {
            if (p.y <= worldPos[x, 0].y)
            {
                return true;
            }
        }

        return false;
    }

    public Vector3 ZeroPosition(Vector3 p)
    {
        return worldPos[0,0];
    }
}

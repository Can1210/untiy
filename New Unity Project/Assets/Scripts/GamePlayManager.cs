using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance = null;

    const int width = 10;
    const int height = 20;

    public Vector3[,] worldPos = new Vector3[width,height];

    string s = "";//デバッグ用

    public enum InArray
    {
        Space,
        Cube,
    }

    private InArray[,] inArrays = new InArray[width, height];

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
            }
        }

        inSpaceArray();
    }

    // Update is called once per frame
    void Update()
    {
        arrayDebug();
    }

    public void CurrentArray()
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

    public Vector3 inCubeArray(Vector3 p, Vector3 Previous)
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if(p == worldPos[x,y])
                {
                    inArrays[x, y] = InArray.Cube;
                }

                if(Previous != p && Previous == worldPos[x,y])
                {
                    inArrays[x, y] = InArray.Space;
                }
            }
        }

        return p;
    }

    public void inSpaceArray()
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                inArrays[x, y] = InArray.Space;
            }
        }
    }

    public void arrayDebug()
    {

        s = "\n";

        for (int y = height -1 ;0 <= y && y < height; y--)
        {
            for (int x = 0;x < width; x++)
            {
                switch (inArrays[x, y])
                {
                    case InArray.Space:
                        s += "#";
                        break;
                    case InArray.Cube:
                        s += "O";
                        break;
                }
            }
            s = s + "\n";
        }

        Debug.Log(s);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance = null;

    const int width = 12;
    const int height = 22;

    public Vector3[,] worldPos = new Vector3[width,height];

    public float time;
    private int currenTime;//時間

    string s = "";//デバッグ用
    public Text debugText;

    public enum InArray
    {
        Space,
        Cube,
        Wall,

        None,
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
        WallArray();

        time = 0;
        currenTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        NowTime();//    ほぼデバッグ用

        arrayDebug();
    }

     private void NowTime()
    {                            //↓Blockmove_testとスピードを合わせれば常にわかる
        time += Time.deltaTime;//  *  0.5f;
        if (time % 60 >= 1)
        {
            currenTime += (int)time;
            time = 0;

            //arrayDebug();//ほぼデバッグ用
        }
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

    public bool UnderMap(Vector3 p)//下にいるときブロックが留まる
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (p == worldPos[x, y])
                {
                    return true;
                }
            }
        }
        return false;
    }


    //自分の座標にCubeがあった場合trueを返す  正直いらん
    public bool OnCubeCheck(Vector3 p)
    {
        if(inArrays[(int)p.x,(int)p.y] == InArray.Cube)
        {
            return true;
        }

        return false;
    }

    //入れた座標のさきが動ける(space)ならGo
    public bool OnMoveOk(Vector3 p)
    {
        if(worldPos[(int)p.x,(int)p.y] == null)
        {
            return false;
        }

        return false;
    }

    //その場所に何の情報があるか調べる
    public InArray OnAllCheck(Vector3 p)
    {
        return inArrays[(int)p.x, (int)p.y];
    }

    public Vector3 ZeroPosition(Vector3 p)
    {
        return worldPos[0,0];
    }

    //入れ替えcube  space
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

    public void inSpaceArray()//とりあえずすべてをspaceの情報にした。
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                inArrays[x, y] = InArray.Space;
            }
        }
    }

    public void WallArray()//壁の情報を付ける
    {

        for (int x = 0; x < width ; ++x)
        {
            inArrays[x, 0] = InArray.Wall;//一番下

            inArrays[x, height - 1] = InArray.Wall;//一番上
        }

        for (int y = 0; y < height; ++y)
        {
            inArrays[0, y] = InArray.Wall;//一番左

            inArrays[width - 1, y] = InArray.Wall;//一番右
        }
    }

    public void arrayDebug()
    {
        //s = "\n";//最初改行
        s = "";

        for (int y = height -1 ;0 <= y && y < height; y--)
        {
            for (int x = 0;x < width; x++)
            {
                switch (inArrays[x, y])
                {
                    case InArray.Space:
                        s += "  ";
                        break;
                    case InArray.Cube:
                        s += "o";
                        break;

                    case InArray.Wall:
                        s += "a";
                        break;

                    default:
                        s += "G";
                        break;
                }
            }
            s = s + "\n";
        }

        debugText.text = s;

        //Debug.Log(s);
    }
}

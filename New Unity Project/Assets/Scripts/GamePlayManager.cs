using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InArray
{
    Space,//オイル内
    OutOfOil,//オイル外
    UpWall,//一番上の壁

    Cube,
    Wall,
    DownFixedBlock,//下で固定されたBlock
    UpFixedBlock,
    Zero,

    None,
}

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance = null;

    const int width = 12;
    const int height = 22;

    //blockクラスが参照するため
    public int bWidth = width;
    public int bHeight = height;

    public Vector3[,] worldPos = new Vector3[width, height];

    public float time;
    private int currenTime;//時間
    public bool moveOk;

    string s = "";//デバッグ用
    public Text debugText;

    private InArray[,] inArrays = new InArray[width, height];

    //前の情報を記録するための
    private InArray[,] previousArrays = new InArray[width, height];

    private InArray[,] wallAndSpaceArrays = new InArray[width, height];

    private int dySize = 0;
    private int dySizeMax = 0;

    public List<GameObject> useObjects;

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
                worldPos[x, y] = new Vector3(x, y, 0);
            }
        }

        inSpaceArray();
        WallArray();
        wallAndSpaceArrays = inArrays;

        //オイルの外エリアを追加    12,22
        //world[x = 1,y = 18] widht - 1壁から一個前 height - 1 壁から一個前
        OutOfOil(1, 18, width - 1, height - 1);

        previousArrays = wallAndSpaceArrays;

        time = 0;
        currenTime = 0;
        moveOk = false;

        useObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        NowTime();
        //デスエリア更新
        DeathArea();

        arrayDebug();
    }

    public void NowTime()
    {
        moveOk = false;
        time += Time.deltaTime * 3.5f;
        if (time >= 1)
        {
            currenTime += (int)time;
            time = 0;
            moveOk = true;
        }
    }

    //使われているゲームオブジェクトを登録
    public void UseObj(GameObject obj)
    {
        useObjects.Add(obj);
    }

    public void DeathArea()
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (inArrays[x, y] == InArray.Zero)
                {
                    dySizeMax = y;
                }
            }
        }

        for (int x = width - 1; x >= 0; --x)
        {
            for (int y = height - 1; y >= 0; --y)
            {
                if (inArrays[x, y] == InArray.Zero)
                {
                    dySize = y;
                }
            }
        }
    }


    //自分が死ぬエリアにあったら
    public bool InDeathArea(Vector3 p)
    {
        for (int y = dySize; y <= dySizeMax; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                if (p == worldPos[x, y])
                {
                    //デスエリア内にいたら    
                    if (p.y == y)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }


    //自分の情報を返す
    public InArray SelfState(Vector3 p)
    {
        InArray i = InArray.Space;

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (p == worldPos[x, y])
                {
                    i = inArrays[x, y];
                }
            }
        }
        return i;
    }

    //前の情報を入れる
    public void PreviousArray()
    {
        previousArrays = inArrays;
    }

    public void OutOfOil(int ax, int ay, int w, int h)
    {
        for (int x = ax; x < w; x++)
        {
            for (int y = ay; y < h; y++)
            {
                inArrays[x, y] = InArray.OutOfOil;
            }
        }
    }

    //オイルの外かどうか
    public bool OutOfOilChack(int x, int y, int w, int h, Vector3 p)
    {
        if (p.x >= x && p.x < w && p.y >= y && p.y < h)
        {
            return true;
        }
        return false;
    }

    public void DownFixedBlock(Vector3 p)
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (p == worldPos[x, y])
                {
                    inArrays[(int)p.x, (int)p.y] = InArray.DownFixedBlock;
                }
            }
        }
    }

    public void UpFixedBlock(Vector3 p)
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (p == worldPos[x, y])
                {
                    if (inArrays[x, y] == InArray.Zero)
                    {
                        return;
                    }
                    else
                    {
                        inArrays[(int)p.x, (int)p.y] = InArray.UpFixedBlock;
                    }
                }
            }
        }
    }

    public void NotFixedBlock(Vector3 p)
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (p == worldPos[x, y])
                {
                    inArrays[(int)p.x, (int)p.y] = InArray.Cube;
                }
            }
        }
    }

    public bool OnBlockCheck(Vector3 p, Vector3 next)
    {
        int nx = (int)next.x;
        int ny = (int)next.y;

        int dx = nx - (int)p.x;
        int dy = ny - (int)p.y;
        //下
        if (dy < 0 && dx == 0)
        {
            for (int y = ny; y >= 0; y--)
            {
                //今だけ
                for (int xb = 0; xb < width; ++xb)
                {
                    for (int yb = 0; yb < height; ++yb)
                    {
                        if (next == worldPos[xb, yb])
                        {
                            if (inArrays[nx, y] == InArray.Space)
                            {
                                return false;
                            }
                            else if (inArrays[nx, y] == InArray.Wall)
                            {
                                return true;
                            }
                            else if (inArrays[nx, y] == InArray.DownFixedBlock)//すでに固定された
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        //上 
        else if (dy > 0 && dx == 0)
        {
            for (int y = ny; y < height; y++)
            {
                //今だけ
                for (int xb = 0; xb < width; ++xb)
                {
                    for (int yb = 0; yb < height; ++yb)
                    {
                        if (next == worldPos[xb, yb])
                        {
                            //一個でも上にブロックがあればいけない
                            if (inArrays[nx, y] == InArray.Space)
                            {
                                return false;
                            }
                            else if (inArrays[nx, y] == InArray.UpFixedBlock)
                            {
                                return true;
                            }
                            else if (inArrays[nx, y] == InArray.DownFixedBlock)//すでに固定された
                            {
                                return true;
                            }
                            else if (inArrays[nx, y] == InArray.OutOfOil)
                            {
                                return true;
                            }
                            else if (inArrays[nx, y] == InArray.UpWall)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    public bool OnCubeOfWallCheck(Vector3 p, Vector3 next)
    {
        int nx = (int)next.x;
        int ny = (int)next.y;

        int dx = nx - (int)p.x;
        int dy = ny - (int)p.y;

        //移動量がゼロの時
        if(dx == 0 && dy == 0)
        {
            return true;
        }

        //右
        if (dx > 0)
        {
            for (int x = nx; x < width; x++)
            {
                //今だけ
                for (int xb = 0; xb < width; ++xb)
                {
                    for (int yb = 0; yb < height; ++yb)
                    {
                        if (next == worldPos[xb, yb])
                        {
                            if (inArrays[x, ny] == InArray.Space)
                            {
                                return true;
                            }
                            else if (inArrays[x, ny] == InArray.Wall)
                            {
                                return false;
                            }
                            else if (inArrays[x, ny] == InArray.DownFixedBlock)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        //左
        else if (dx < 0)
        {
            for (int x = nx; x >= 0; x--)
            {
                //今だけ
                for (int xb = 0; xb < width; ++xb)
                {
                    for (int yb = 0; yb < height; ++yb)
                    {
                        if (next == worldPos[xb, yb])
                        {
                            if (inArrays[x, ny] == InArray.Space)
                            {
                                return true;
                            }
                            else if (inArrays[x, ny] == InArray.Wall)
                            {
                                return false;
                            }
                            else if (inArrays[x, ny] == InArray.DownFixedBlock)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }

        #region 下とうえ
        //下
        else if (dy < 0)
        {
            for (int y = ny; y >= 0; y--)
            {
                //今だけ
                for (int xb = 0; xb < width; ++xb)
                {
                    for (int yb = 0; yb < height; ++yb)
                    {
                        if (next == worldPos[xb, yb])
                        {

                            if (inArrays[nx, y] == InArray.Space)
                            {
                                return true;
                            }
                            else if (inArrays[nx, y] == InArray.Wall)
                            {
                                return false;
                            }
                            else if (inArrays[nx, y] == InArray.DownFixedBlock)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        //上
        else if (dy > 0)
        {
            for (int y = ny; y < height; y++)
            {
                //今だけ  ここの二十for分むし
                for (int xb = 0; xb < width; ++xb)
                {
                    for (int yb = 0; yb < height; ++yb)
                    {
                        if (next == worldPos[xb, yb])
                        {
                            //ここまで気にすんな
                            //spaceがあった場合動ける
                            if (inArrays[nx, y] == InArray.Space)
                            {
                                return true;
                            }
                            //Mapの上に固定されたブロックがある場合
                            else if (inArrays[nx, y] == InArray.UpFixedBlock)
                            {
                                return false;
                            }
                            else if (inArrays[nx, y] == InArray.DownFixedBlock)
                            {
                                return false;
                            }
                            else if (inArrays[nx, y] == InArray.OutOfOil)
                            {
                                return false;
                            }
                            else if (inArrays[nx, y] == InArray.Wall)
                            {
                                return false;
                            }
                            else if (inArrays[nx, y] == InArray.UpWall)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        return false;
    }

    //次行く場所がオイルなら
    public bool CheckOil(Vector3 p, Vector3 next)
    {
        int nx = (int)next.x;
        int ny = (int)next.y;

        int dx = nx - (int)p.x;
        int dy = ny - (int)p.y;
        //上
        if (dy > 0)
        {
            for (int y = ny; y < height; y++)
            {
                //今だけ  ここの二十for分むし
                for (int xb = 0; xb < width; ++xb)
                {
                    for (int yb = 0; yb < height; ++yb)
                    {
                        if (next == worldPos[xb, yb])
                        {
                            //ここまで気にすんな
                            if (inArrays[nx, y] == InArray.OutOfOil)
                            {
                                return true;
                            }
                            else if (inArrays[nx, y] == InArray.Space)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    public void Zero(Vector3 p)
    {
        //今だけ
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (p == worldPos[x, y])
                {
                    inArrays[x, y] = InArray.Zero;
                }
            }
        }
    }

    //入れ替えcube  space
    public void inCubeArray(Vector3 p, Vector3 Previous)
    {
        //for分で回す必要めっちゃある
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (p == worldPos[x, y])
                {
                    inArrays[x, y] = InArray.Cube;
                }

                if (Previous != p && Previous == worldPos[x, y])
                {
                    inArrays[x, y] = InArray.Space;
                }
            }
        }
    }

    //上がspaceじゃなかったら
    public bool NotOnSpace(InArray[,] ina, Transform[] t)
    {

        for (int i = 0; i < t.Length; i++)
        {
            Vector3 p = t[i].position;
            int px = (int)p.x;
            int py = (int)p.y;

            if (inArrays[px, py + 1] != InArray.Space && ina[px, py + 1] == InArray.Space)
            {
                //ブロックがあった場合他のやつのブロック
                return true;
            }
        }
        return false;
    }

    public void inOilOutArray(Vector3 p, Vector3 Previous)
    {
        //for分で回す必要めっちゃある
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                InArray ins = inArrays[x, y];

                if (p == worldPos[x, y])
                {
                    inArrays[x, y] = InArray.Cube;
                }

                if (Previous != p && Previous == worldPos[x, y])
                {
                    if (OutOfOilChack(1, 18, width - 1, height - 1, Previous))
                    {
                        inArrays[x, y] = InArray.OutOfOil;
                    }
                    else
                    {
                        inArrays[x, y] = InArray.Space;
                    }
                    //inArrays[x, y] = InArray.OutOfOil;
                }
            }
        }
    }
    public void inZeroArray(Vector3 p, Vector3 Previous)
    {
        //for分で回す必要めっちゃある
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (p == worldPos[x, y])
                {
                    inArrays[x, y] = InArray.Zero;
                }

                if (Previous != p && Previous == worldPos[x, y])
                {
                    inArrays[x, y] = InArray.Space;
                }
            }
        }
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
    //ブロッククラスの配列にスペースをコピー
    public InArray[,] inSpaceBlocks(InArray[,] inBlocks)
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (x == worldPos[x, y].x && y == worldPos[x, y].y)
                {
                    inBlocks[x, y] = InArray.Space;
                }
            }
        }

        return inBlocks;
    }

    public void WallArray()//壁の情報を付ける
    {

        for (int x = 0; x < width; ++x)
        {
            inArrays[x, 0] = InArray.Wall;//一番下

            inArrays[x, height - 1] = InArray.UpWall;//一番上
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

        for (int y = height - 1; 0 <= y && y < height; y--)
        {
            for (int x = 0; x < width; x++)
            {
                switch (inArrays[x, y])
                {
                    case InArray.Space:
                        s += " .";
                        break;
                    case InArray.Cube:
                        s += "o";
                        break;

                    case InArray.Wall:
                        s += "a";
                        break;

                    case InArray.DownFixedBlock:
                        s += "x";
                        break;

                    case InArray.OutOfOil:
                        s += "^";
                        break;

                    case InArray.UpFixedBlock:
                        s += "u";
                        break;

                    case InArray.UpWall:
                        s += "~";
                        break;

                    case InArray.Zero:
                        s += "p";
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

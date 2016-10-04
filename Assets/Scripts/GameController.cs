using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    #region +
    public GameObject pot1;//白色圆格子
    public GameObject pot2;//黄色圆格子
    public GameObject WinBG;
    public GameObject end;
    #endregion

    #region  -
    private Text score;
    private int rowNum = 9;//行数量
    private int columnNum = 9; //列数量
    private ArrayList potArr;//所有圆格子集合
    private ArrayList temp;//一行的圆格子集合
    private GameObject cat;//AI猫
    private Item catItem;//猫上的Item脚本
    private int rIndexCat;//猫当前所在的行索引
    private int cIndexCat;//猫当前所在的列索引
    private Vector2 v = new Vector2();
    private Item itemPot;//从集合里取出的Item
    private Item itemMovable;//判断取出来的Item是否可以走
    private Item item;//创建的圆格子pot1
    private int countIndex;//猫可以走的圆格子随即出来的索引号
    #endregion

    #region 单例
    public static GameController _i;

    void Awake()
    {
        _i = this;
    }
    #endregion

    #region Unity内置函数
    void Start()
    {
        cat = this.transform.FindChild("Cat").gameObject;
        score = WinBG.transform.FindChild("Score").GetComponent<Text>();
        score.text = string.Empty;
        catItem = cat.GetComponent<Item>();
        end.gameObject.SetActive(false);
        WinBG.SetActive(false);
    }
    #endregion

    #region  +Init初始化
    public void Init()
    {
        catItem.GoTo(Random.Range(1, 8), Random.Range(1, 8));//随机生成猫的初始位置
        potArr = new ArrayList();//所有圆格子集合

        for (int rIndex = 0; rIndex < rowNum; rIndex++)
        {
            temp = new ArrayList();//一行的圆格子集合

            for (int cIndex = 0; cIndex < columnNum; cIndex++)
            {
                CeratePot(pot1, rIndex, cIndex);
                temp.Add(item);//将一行里面每一列的圆格子添加到行圆格子集合里面
            }

            potArr.Add(temp);//一行生成后添加到列圆格子集合，这样就成了一个二维集合，存储所有的圆格子
        }
    }
    #endregion

    #region  +Reset再来一次
    public void Reset()
    {
        //先清空集合
        temp.Clear();
        potArr.Clear();

        //删除场景上所有的圆格子
        GameObject[] p1 = GameObject.FindGameObjectsWithTag("p1");
        GameObject[] p2 = GameObject.FindGameObjectsWithTag("p2");

        foreach (GameObject go in p1)
        {
            Destroy(go);
        }

        foreach (GameObject go in p2)
        {
            Destroy(go);
        }

        //全部清理后再次创建圆格子
        Init();
    }
    #endregion

    #region +Select玩家点击圆格子触发的事件
    public void Select(Item item)
    {
        if (item.movable)
        {
            CeratePot(pot2, item.rIndex, item.cIndex);
            item.movable = false;
            Destroy(item.gameObject);

            ArrayList steps = FindSteps();
            Debug.Log(steps.Count);

            if (steps.Count > 0)//大于0表示猫还有可以走的圆格子
            {
                countIndex = Random.Range(0, steps.Count);
                Vector2 v = (Vector2)steps[countIndex];

                catItem.GoTo((int)v.y, (int)v.x);//猫随机移动到可以走的圆格子上

                if (Escaped())//表示猫逃走了
                {
                    end.gameObject.SetActive(true);
                    IsLos();
                }
            }
            else
            {
                WinBG.SetActive(true);
                score.text = GameObject.FindGameObjectsWithTag("p2").Length.ToString();
                IsLos();
            }
        }
    }
    #endregion

    #region  -IsLos用来设置赢了或输了之后不能再点击圆格子了
    void IsLos()
    {
        Item[] i = GameObject.FindObjectsOfType<Item>();

        foreach (Item go in i)
        {
            go.isLos = true;
        }
    }
    #endregion

    #region -CeratePot创建Pot
    void CeratePot(GameObject pot, int rIndex, int cIndex)
    {
        GameObject go = Instantiate(pot) as GameObject;
        go.transform.parent = this.transform;

        item = go.GetComponent<Item>();
        item.GoTo(rIndex, cIndex);
    }
    #endregion

    #region -GetPot从二维集合里面取出一个Item
    /// <summary>
    /// 取出Item
    /// </summary>
    /// <param name="rIndex">行索引</param>
    /// <param name="cIndex">列索引</param>
    /// <returns>Item</returns>
    Item GetPot(int rIndex, int cIndex)
    {
        //超出数组范围了
        if (rIndex < 0 || rIndex > rowNum - 1 || cIndex < 0 || cIndex > columnNum - 1)
        {
            return null;
        }

        ArrayList tmp = potArr[rIndex] as ArrayList;
        itemPot = tmp[cIndex] as Item;
        return itemPot;
    }
    #endregion

    #region -FindSteps返回猫所有可以走的圆格子的集合
    ArrayList FindSteps()
    {
        rIndexCat = catItem.rIndex;
        cIndexCat = catItem.cIndex;
        ArrayList steps = new ArrayList();//猫可以走的圆格子的集合

        #region  不管是奇数行还是偶数行猫走上下左右的一样的

        #region left---同一行的左边
        v.y = rIndexCat;//同一行Y轴不变
        v.x = cIndexCat - 1;//左边就是减1

        if (Movable(v))
        {
            steps.Add(v);//添加到集合里去
        }
        #endregion

        #region right---同一行的右边
        v.y = rIndexCat;//同一行Y轴不变
        v.x = cIndexCat + 1;//左边就是加1

        if (Movable(v))
        {
            steps.Add(v);
        }
        #endregion

        #region top---同一列上面一行无奇数偶数行区分
        v.y = rIndexCat + 1;//上面的话肯定就是加1
        v.x = cIndexCat;//列相同X轴不变

        if (Movable(v))
        {
            steps.Add(v);
        }
        #endregion

        #region bottom---同一列下面一行无奇数偶数行区分
        v.y = rIndexCat - 1;//往下减1
        v.x = cIndexCat;//列相同X轴不变

        if (Movable(v))
        {
            steps.Add(v);
        }
        #endregion

        #endregion

        #region 区分奇数行和偶数行的左上左下右上右下,这里执行完后最多只会添加2个可以走的圆格子

        #region 奇数行topLeft左上       偶数行topRight右上
        v.y = rIndexCat + 1;//因为这里都是上面的所以Y轴直接加1

        if (rIndexCat % 2 == 1)//如果是奇数行
        {
            //一行里面的X轴上的坐标对应的就是列索引
            v.x = cIndexCat - 1;//左边就是在X轴对应的列索引减1
        }
        else
        {
            v.x = cIndexCat + 1;//右边就是加1
        }

        if (Movable(v))
        {
            steps.Add(v);
        }
        #endregion

        #region 奇数行bottomLeft左下       偶数行bottomRight右下
        v.y = rIndexCat - 1;//因为这里都是下面的所以Y轴直接减1

        if (rIndexCat % 2 == 1)//如果是奇数行
        {
            v.x = cIndexCat - 1;//左边就是在X轴对应的列索引减1
        }
        else
        {
            v.x = cIndexCat + 1;//右边就是加1
        }

        if (Movable(v))
        {
            steps.Add(v);
        }
        #endregion

        #endregion

        return steps;//返回所有可以走的圆格子集合，最多6个

    }
    #endregion

    #region -Movable判断传入的位置的圆格子是否可以走
    bool Movable(Vector2 v)
    {
        itemMovable = GetPot((int)v.y, (int)v.x);

        if (itemMovable == null)
        {
            return false;
        }

        return itemMovable.movable;
    }
    #endregion

    #region -Escaped猫逃脱了
    bool Escaped()
    {
        rIndexCat = catItem.rIndex;
        cIndexCat = catItem.cIndex;

        //                  下                         上                                           左                         右
        if (rIndexCat == 0 || rIndexCat == rowNum - 1 || cIndexCat == 0 || cIndexCat == columnNum - 1)
        {
            return true;//猫逃走了
        }
        else
        {
            return false;//猫还没有逃走
        }
    }
    #endregion

}

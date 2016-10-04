using UnityEngine;

/// <summary>
/// 二维网格对象
/// </summary>
public class Item : MonoBehaviour
{
    #region +
    public bool movable = true;//是否可以移动，开始可以
    public float Xoff = -2.47f;
    public float Yoff = -3.7f;

    [HideInInspector]
    public int rIndex;
    [HideInInspector]
    public int cIndex;
    [HideInInspector]
    public bool isLos = false;
    #endregion

    #region -
    private float xDistance = 0.58f;//奇数行的x轴位置偏移
    private float xDistanceTwo = 0.586f;//偶数行的x轴位置偏移
    private float yDistance = 0.5f;//y轴的位置偏移
    private float xAgainDistance = 0.259f;//偶数行的整体一行的位置偏移
    private Vector3 v = Vector3.zero;
    #endregion

    #region +GoTo设置圆格子的位置
    /// <summary>
    /// 传入行索引和列索引调用更新圆格子位置的方法
    /// </summary>
    /// <param name="rIndex">行索引</param>
    /// <param name="cIndex">列索引</param>
    public void GoTo(int rIndex, int cIndex)
    {
        this.rIndex = rIndex;
        this.cIndex = cIndex;

        UpdatePosition();
    }
    #endregion

    #region -UpdatePosition更新圆格子的位置
    void UpdatePosition()
    {
        v.x = xDistance * cIndex + Xoff;

        if (rIndex % 2 == 0)//表示偶数行
        {
            v.x = xDistanceTwo * cIndex + Xoff + xAgainDistance;//再次向右偏移
        }

        v.y = yDistance * rIndex + Yoff;

        this.transform.position = v;
    }
    #endregion

    #region  -Unity内置函数鼠标点击回调方法
    void Start() { }
    void OnMouseDown()
    {
        if (!isLos)
        {
            GameController._i.Select(this);
        }
    }
    #endregion
}

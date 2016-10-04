using UnityEngine;

public class end : MonoBehaviour
{
    #region Unity内置函数
    void OnMouseDown()
    {
        GameController._i.Reset();
        this.transform.parent.gameObject.SetActive(false);
    }
    #endregion

}

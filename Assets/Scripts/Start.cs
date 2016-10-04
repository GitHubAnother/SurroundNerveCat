using UnityEngine;

public class Start : MonoBehaviour
{
    #region Unity内置函数
    void OnMouseDown()
    {
        GameController._i.Init();
        this.gameObject.SetActive(false);
    }
    #endregion
}

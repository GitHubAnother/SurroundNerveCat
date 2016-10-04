/*
作者名称:YHB

脚本作用:

建立时间:
*/

using UnityEngine;
using System.Collections;

public class UIReset : MonoBehaviour
{

    #region UI
    public void Reset()
    {
        GameController._i.Reset();
        this.transform.parent.gameObject.SetActive(false);
    }
    #endregion
}

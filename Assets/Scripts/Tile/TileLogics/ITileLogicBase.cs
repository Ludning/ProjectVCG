using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileLogicBase
{
    /// <summary>
    /// 상호작용 함수
    /// </summary>
    public void OnInteraction();
    /// <summary>
    /// 아이템 가져오기 함수
    /// </summary>
    public ItemBase OnPushItem();
    /// <summary>
    /// 아이템 내려놓기 함수
    /// </summary>
    public void OnPopItem();
    /// <summary>
    /// 값 변경될 시 알림 함수
    /// </summary>
    public void OnValueChangeNotify();
}

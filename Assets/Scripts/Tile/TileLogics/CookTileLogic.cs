using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookTileLogic : ITileLogicBase
{
    Stack<ItemBase> items = new Stack<ItemBase>();

    public void OnInteraction()
    {
        //모든 items의 아이템 검사, 레시피 검사후 음식 생성, 기존 스택 아이템은 삭제
    }


    public void OnPopItem()
    {

    }

    public ItemBase OnPushItem()
    {
        //하이어라키 계층구조 변경등 작업?
        return items.Pop();
    }

    public void OnValueChangeNotify()
    {

    }
}

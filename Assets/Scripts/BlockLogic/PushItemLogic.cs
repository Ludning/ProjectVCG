using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UIElements;

public class PushItemLogic : BlockLogicBase
{
    public override ErrorType IsExecutable(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;

        //타일 존재 체크
        if(owner.TableManager.PeekTile(position) == null)
            return ErrorType.NoTile; 
        
        //주울 수 있는 타일인지 체크
        if (!owner.TableManager.PeekTile(position).IsPushAble)
            return ErrorType.InvalidDropTile;
        
        //타일 인벤토리가 비어있는지 체크
        if (owner.TableManager.PeekTile(position).IsInventoryEmpty)
            return ErrorType.NoPickableItem;
        
        //캐릭터 인벤토리가 꽉 찼는지 확인
        if(owner.Inventory.IsInventoryOverflow)
            return ErrorType.InventoryOverflow;
        
        return ErrorType.NoError;
    }

    public override LogicState Execute(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        var item = owner.TableManager.PopTileItem(position);
        if(item == null)
            return LogicState.Failure;
        //owner.TableManager.PushTileItem(position, null);
        owner.Inventory.PushItem(item);
        return LogicState.Success;
    }
    public override void CheakClear(StageManager owner)
    {
    }
}

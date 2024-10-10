using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UIElements;

public class PushItemLogic : BlockLogicBase
{
    private float _runningTime = 1.5f;
    
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
        
        _runningTime = 1.5f;
        owner.Controller.SetAnimationState(AnimationState.IsLifting, true);
        return ErrorType.NoError;
    }

    public override LogicState Execute(StageManager owner)
    {
        if (_runningTime > 0)
        {
            _runningTime -= Time.deltaTime;
            return LogicState.Running;
        }

        var position = owner.Controller.PlayerForwardPosition;
        var item = owner.TableManager.PopTileItem(position);
        if(item == null)
            return LogicState.Failure;
        owner.Inventory.PushItem(item);
        
        owner.UIContainer.InventoryPopup.AddItem(item.ItemType);
        
        owner.Controller.SetAnimationState(AnimationState.IsLifting, false);
        owner.Controller.SetAnimationState(AnimationState.IsCarry, true);
        return LogicState.Success;
    }
    public override void CheakClear(StageManager owner)
    {
    }
}

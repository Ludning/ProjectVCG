using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopItemLogic : BlockLogicBase
{
    private float _runningTime = 1.5f;
    
    public override ErrorType IsExecutable(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;

        //타일 존재 체크
        if(owner.TableManager.PeekTile(position) == null)
            return ErrorType.NoTile;

        //인벤토리가 비어있는지 체크
        if (owner.Inventory.IsInventoryEmpty == true)
            return ErrorType.NoDropableItem;
        
        //내려놓을 수 있는 타일인지 체크
        TileBase tile = owner.TableManager.PeekTile(position);
        if (!owner.TableManager.PeekTile(position).IsPopAble)
            return ErrorType.InvalidDropTile;
        
        _runningTime = 1.5f;
        owner.Controller.SetAnimationState(AnimationState.IsPuttingDown, true);
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
        var item = owner.Inventory.PopItem(position);
        owner.TableManager.PushTileItem(position, item);
        
        owner.Controller.SetAnimationState(AnimationState.IsPuttingDown, false);
        owner.Controller.SetAnimationState(AnimationState.IsCarry, false);
        return LogicState.Success;
    }
    public override void CheakClear(StageManager owner)
    {
        var servingTilePosition = owner.Controller.PlayerForwardPosition;
        TileBase servingTile = owner.TableManager.PeekTile(servingTilePosition);
        if(owner.levelManager.CheckLevel(servingTile, BlockLogicType.PopItem) == true)
            owner.levelManager.CompleteCurrentLevel();
    }
}
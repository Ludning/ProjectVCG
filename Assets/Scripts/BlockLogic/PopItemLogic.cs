using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopItemLogic : BlockLogicBase
{
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
        
        return ErrorType.NoError;
    }

    public override LogicState Execute(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        var item = owner.Inventory.PopItem(position);
        owner.TableManager.PushTileItem(position, item);
        return LogicState.Success;
    }
    public override void CheakClear(StageManager owner)
    {
        var servingTilePosition = owner.Controller.PlayerForwardPosition;
        TileBase servingTile = owner.TableManager.PeekTile(servingTilePosition);
        if(owner.levelManager.CheckLevel(servingTile) == true)
            owner.levelManager.CompleteCurrentLevel();
    }
}
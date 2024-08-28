using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopItemLogic : BlockLogicBase
{
    public override bool IsExecutable(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        TileType tileType = owner.Table.GetTileType(position);
        ItemBase item = owner.Table.GetTileItem(position);

        return (tileType == TileType.CUSTOMER||tileType == TileType.COOKING) ? true : false;
    }

    public override LogicState Execute(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        var item = owner.Inventory.PopItem(position);
        item.transform.position = owner.Table.GetTilePosition(position);
        owner.Table.SetTileItem(position, item);
        return LogicState.Success;
    }
}
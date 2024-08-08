using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItemLogic : BlockLogicBase
{
    public override bool IsExecutable(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        TileType tileType = owner.Table.GetTileType(position);
        ItemBase item = owner.Table.GetTileItem(position);

        return (tileType == TileType.Customer) ? true : false;
    }

    public override bool Execute(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        var worldPosition = owner.Table.GetTilePosition(position);
        var item = owner.Inventory.PopItem(worldPosition);
        item.transform.position = owner.Table.GetTilePosition(position);
        return true;
    }
}
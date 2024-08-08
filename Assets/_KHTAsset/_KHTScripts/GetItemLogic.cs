using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UIElements;

public class GetItemLogic : BlockLogicBase
{
    public override bool IsExecutable(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        TileType tileType = owner.Table.GetTileType(position);
        ItemBase item = owner.Table.GetTileItem(position);

        return (tileType == TileType.food) ? true : false;
    }

    public override bool Execute(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        var item = owner.Table.GetTileItem(position);
        owner.Inventory.PushItem(item);
        return true;
    }
}

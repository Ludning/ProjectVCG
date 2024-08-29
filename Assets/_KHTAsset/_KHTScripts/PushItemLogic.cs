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
        TileType tileType = owner.Table.GetTileType(position);
        ItemBase item = owner.Table.GetTileItem(position);

        return (item != null) ? ErrorType.NoError : ErrorType.NoDropItem;
    }

    public override LogicState Execute(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        var item = owner.Table.GetTileItem(position);
        owner.Table.SetTileItem(position, null);
        owner.Inventory.PushItem(item);
        return LogicState.Success;
    }
}

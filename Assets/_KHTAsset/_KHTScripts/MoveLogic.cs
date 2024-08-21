using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLogic : BlockLogicBase
{
    public override bool IsExecutable(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        TileType tileType = owner.Table.GetTileType(position);
        ItemBase item = owner.Table.GetTileItem(position);

        if (tileType != TileType.Walk || item != null)
        {
            return false;
        }
        return true;
    }

    public override bool Execute(StageManager owner)
    {
        var targetPosition = owner.Controller.PlayerForwardPosition;

        Vector3 targetWorldPosition = owner.Table.GetTilePosition(targetPosition);
        Vector3 playerWorldPosition = owner.Controller.transform.position;
        owner.Controller.transform.position = Vector3.Lerp(owner.Controller.transform.position, targetWorldPosition, 0.1f);
        float distance = Vector3.Distance(playerWorldPosition, targetWorldPosition);
        
        if (distance < 0.1f)
        {
            owner.Controller.transform.position = targetWorldPosition;
            owner.Controller.PlayerPosition = targetPosition;
            Debug.Log("End Logic");
            return true;
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLogic : BlockLogicBase
{
    public override bool IsExecutable(StageManager owner)
    {
        //이동의 예시

        //앞으로 이동하면 캐릭터 기준 앞 위치를 매개변수로 넘겨줌
        var position = owner.Controller.PlayerForwardPosition;
        TileType tileType = owner.Table.GetTileType(position);
        ItemBase item = owner.Table.GetTileItem(position);

        if (tileType != TileType.Plane || item != null)
        {
            return false;
        }
        return true;
    }

    public override bool Execute(StageManager owner)
    {
        var targetPosition = owner.Controller.PlayerForwardPosition;

        owner.Controller.transform.position = Vector2.Lerp(owner.Controller.transform.position, targetPosition, 0.1f);

        var targetWorldPosition = owner.Table.GetTilePosition(targetPosition);

        var distance = Vector2.Distance((Vector2)owner.Controller.transform.position, targetWorldPosition);
        if (distance < 0.1f)
        {
            owner.Controller.transform.position = targetWorldPosition;
            return true;
        }
        return false;
    }
}

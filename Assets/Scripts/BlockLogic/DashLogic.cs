using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashLogic : BlockLogicBase
{
    public override ErrorType IsExecutable(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;

        //타일 존재 체크
        if (owner.TableManager.PeekTile(position) == null)
            return ErrorType.NoTile;

        //지형 체크
        if (!owner.TableManager.PeekTile(position).IsWalkAble)
            return ErrorType.NotMove;
        
        //장애물(재료등) 체크
        if (!owner.TableManager.PeekTile(position).IsInventoryEmpty)
            return ErrorType.NotMove;

        return ErrorType.NoError;
    }

    public override LogicState Execute(StageManager owner)
    {
        Debug.Log("Execute");
        var targetPosition = owner.Controller.PlayerForwardPosition;

        Vector3 targetWorldPosition = owner.TableManager.GetTilePosition(targetPosition, PositionType.Player);
        Vector3 playerWorldPosition = owner.Controller.transform.position;
        owner.Controller.transform.position =
            Vector3.Lerp(owner.Controller.transform.position, targetWorldPosition, 0.1f);
        float distance = Vector3.Distance(playerWorldPosition, targetWorldPosition);

        if (distance < 0.1f)
        {
            owner.Controller.transform.position = targetWorldPosition;
            owner.Controller.PlayerPosition = targetPosition;

            if (IsExecutable(owner) == ErrorType.NoError)
            {
                return LogicState.Running;
            }
            Debug.Log("End Logic");
            return LogicState.Success;
        }
        return LogicState.Running;
    }
    public override void CheakClear(StageManager owner)
    {
        var playerPosition = owner.Controller.PlayerPosition;
        TileBase tile = owner.TableManager.PeekTile(playerPosition);
        if(owner.levelManager.CheckLevel(tile, BlockLogicType.Dash) == true)
            owner.levelManager.CompleteCurrentLevel();
    }
}

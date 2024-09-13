using UnityEngine;

public class RotateLeftLogic : BlockLogicBase
{
    Quaternion playerTargetRot = Quaternion.identity;
	
    public override ErrorType IsExecutable(StageManager owner)
    {
        playerTargetRot = owner.Controller.transform.rotation * Quaternion.Euler(new Vector3(0, -90, 0));
        return ErrorType.NoError;
    }

    public override LogicState Execute(StageManager owner)
    {
        Quaternion playerCurrentRot = owner.Controller.transform.rotation;
        owner.Controller.transform.rotation = Quaternion.Lerp(playerCurrentRot, playerTargetRot, 0.1f);

        if (Quaternion.Angle(playerCurrentRot,playerTargetRot) < 0.1f)
        {
            switch (owner.Controller.PlayerForwardType)
            {
                case Direction.Up:
                    owner.Controller.PlayerForwardType = Direction.Left;
                    break;
                case Direction.Down:
                    owner.Controller.PlayerForwardType = Direction.Right;
                    break;
                case Direction.Left:
                    owner.Controller.PlayerForwardType = Direction.Down;
                    break;
                case Direction.Right:
                    owner.Controller.PlayerForwardType = Direction.Up;
                    break;
            }
            return LogicState.Success;
        }
		
        return LogicState.Running;
    }
}

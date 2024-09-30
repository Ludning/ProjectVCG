using UnityEngine;

public class RotateRightLogic : BlockLogicBase
{
    Quaternion playerTargetRot = Quaternion.identity;
	
    public override ErrorType IsExecutable(StageManager owner)
    {
        playerTargetRot = owner.Controller.transform.localRotation * Quaternion.Euler(new Vector3(0, 90, 0));
        
        owner.Controller.SetAnimationState(AnimationState.IsRightTurn, true);
        return ErrorType.NoError;
    }

    public override LogicState Execute(StageManager owner)
    {
        /*Quaternion playerCurrentRot = owner.Controller.transform.localRotation;
        owner.Controller.transform.localRotation = Quaternion.Lerp(playerCurrentRot, playerTargetRot, 0.1f);*/
        
        
        Quaternion playerCurrentRot = owner.Controller.transform.localRotation;
        owner.Controller.transform.localRotation = Quaternion.Lerp(playerCurrentRot, playerTargetRot, 0.1f);

        if (Quaternion.Angle(playerCurrentRot,playerTargetRot) < 0.1f)
        {
            owner.Controller.transform.localRotation = playerTargetRot;
            switch (owner.Controller.PlayerForwardType)
            {
                case Direction.Up:
                    owner.Controller.PlayerForwardType = Direction.Right;
                    break;
                case Direction.Down:
                    owner.Controller.PlayerForwardType = Direction.Left;
                    break;
                case Direction.Left:
                    owner.Controller.PlayerForwardType = Direction.Up;
                    break;
                case Direction.Right:
                    owner.Controller.PlayerForwardType = Direction.Down;
                    break;
            }
            
            owner.Controller.SetAnimationState(AnimationState.IsRightTurn, false);
            return LogicState.Success;
        }
		
        return LogicState.Running;
    }
    public override void CheakClear(StageManager owner)
    {
    }
}

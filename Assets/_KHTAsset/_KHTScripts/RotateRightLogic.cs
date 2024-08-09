using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRightLogic : BlockLogicBase
{
    Rotate rotate = Rotate.Right;

    Quaternion playerCurrentRot = Quaternion.identity;
    Quaternion playerTargetRot = Quaternion.identity;
    public override bool IsExecutable(StageManager owner)
    {
        Quaternion playerCurrentRot = owner.Controller.transform.rotation;
        Quaternion playerTargetRot = owner.Controller.transform.rotation * GetRotationQuaternion();
        return true;
    }

    public override bool Execute(StageManager owner)
    {
        owner.Controller.transform.rotation = Quaternion.Slerp(playerCurrentRot, playerTargetRot, 0.1f);

        owner.Controller.SetPLayerForward(Direction.Down);
        return true;
    }

    public Quaternion GetRotationQuaternion()
    {
        Quaternion result = Quaternion.identity;
        switch (rotate)
        {
            case Rotate.Left:
                result = Quaternion.Euler(new Vector3(0, -90, 0));
                break;
            case Rotate.Right:
                result = Quaternion.Euler(new Vector3(0, 90, 0));
                break;
        }
        return result;
    }
}

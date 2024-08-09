using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class RotateLeftLogic : BlockLogicBase
{
    Rotate rotate = Rotate.Left;

    Quaternion playerTargetRot = Quaternion.identity;
    public override bool IsExecutable(StageManager owner)
    {
        playerTargetRot = owner.Controller.transform.rotation * GetRotationQuaternion();
        return true;
    }

    public override bool Execute(StageManager owner)
    {
        Quaternion playerCurrentRot = owner.Controller.transform.rotation;
        owner.Controller.transform.rotation = Quaternion.Lerp(playerCurrentRot, playerTargetRot, 0.1f);

        if (Quaternion.Angle(playerCurrentRot,playerTargetRot)<0.1f)
        {
            Vector3 direction = playerTargetRot * Vector3.forward;
            Debug.Log($"direction{direction}");
            if (Vector3.Dot(direction, Vector3.forward) > 0.99f)
                owner.Controller.PlayerForward = Vector2Int.up;
            else if (Vector3.Dot(direction, Vector3.back) > 0.99f)
                owner.Controller.PlayerForward = Vector2Int.down;
            else if (Vector3.Dot(direction, Vector3.left) > 0.99f)
                owner.Controller.PlayerForward = Vector2Int.left;
            else if (Vector3.Dot(direction, Vector3.right) > 0.99f)
                owner.Controller.PlayerForward = Vector2Int.right;

            return true;
        }
        return false;
        
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

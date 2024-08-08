using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLeftLogic : BlockLogicBase
{
    public override bool IsExecutable(StageManager owner)
    {
        return true;
    }

    public override bool Execute(StageManager owner)
    {
        Vector3 rot = owner.TargetRotation + new Vector3(0, -90, 0);

        owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, Quaternion.Euler(rot), 0.1f);
        
        return true;
    }
}

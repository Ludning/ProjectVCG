using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBlockLogic : BlockLogicBase
{
    public int SheetIndex;
    
    public override ErrorType IsExecutable(StageManager owner)
    {
        return ErrorType.NoError;
    }
    public override LogicState Execute(StageManager owner)
    {
        return LogicState.Success;
    }
}

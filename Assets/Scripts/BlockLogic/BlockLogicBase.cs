using UnityEngine;


public class BlockLogicBase : MonoBehaviour
{
    public virtual ErrorType IsExecutable(StageManager owner)
    {
        return ErrorType.UnKnownError;
    }
    public virtual LogicState Execute(StageManager owner)
    {
        return LogicState.Success;
    }
}

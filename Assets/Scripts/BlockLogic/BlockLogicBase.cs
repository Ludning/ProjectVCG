using UnityEngine;


public class BlockLogicBase : MonoBehaviour
{
    public Sprite BlockIcon = null;
    public virtual ErrorType IsExecutable(StageManager owner)
    {
        return ErrorType.UnKnownError;
    }
    public virtual LogicState Execute(StageManager owner)
    {
        return LogicState.Success;
    }
    public virtual void CheakClear(StageManager owner)
    {
    }
}

using UnityEngine;


public class BlockLogicBase : MonoBehaviour
{
    public virtual ErrorType IsExecutable(StageManager owner)
    {
        return ErrorType.UnKnownError;
    }
    
    //return false는 미완료의 의미
    //return true는 완료의 의미
    public virtual LogicState Execute(StageManager owner)
    {
        return LogicState.Success;
    }
}

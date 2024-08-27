using UnityEngine;


public class BlockLogicBase : MonoBehaviour
{
    public virtual bool IsExecutable(StageManager owner)
    {
        return false;
    }
    
    //return false는 미완료의 의미
    //return true는 완료의 의미
    public virtual LogicState Execute(StageManager owner)
    {
        return LogicState.Success;
    }
}

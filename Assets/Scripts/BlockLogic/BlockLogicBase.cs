using UnityEngine;

public class BlockLogicBase : MonoBehaviour
{
    public virtual bool IsExecutable(StageManager owner)
    {
        return false;
    }
    
    public virtual bool Execute(StageManager owner)
    {
        return true;
    }
}

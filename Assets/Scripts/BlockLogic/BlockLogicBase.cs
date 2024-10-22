using Cysharp.Threading.Tasks;
using Oculus.Interaction;
using System;
using UnityEngine;


public class BlockLogicBase : MonoBehaviour
{
    public Sprite BlockIcon = null;
    public SheetBase SheetBase;
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


    /*
    public void OnTriggerExit(Collider other)
    {
        SheetBase sheet = other.GetComponent<SheetBase>();
        if (sheet != null && sheet == SheetBase)
        {
            SheetBase.RemoveBlockLogicAtIndex(this);
        }
    }*/

    public void Init(SheetBase sheet)
    {
        SheetBase = sheet;
        GetComponent<InteractableUnityEventWrapper>().WhenUnselect.AddListener(DestroySelf);
    }
    private void DestroySelf()
    {
        SheetBase.RemoveBlockLogicAtIndex(this);
        UniTask_DestroySelf().Forget();
    }
    private async UniTaskVoid UniTask_DestroySelf()
    {
        await UniTask.Yield();
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SheetManager : MonoBehaviour
{
    [SerializeField]
    private StageManager Stage;
    private List<BlockLogicBase> _blockLogicBases = new List<BlockLogicBase>();

    //private bool IsRun = true;

    public void AddBlockLogic(BlockLogicBase blockLogic)
    {
        _blockLogicBases.Add(blockLogic);
    }
    public void ClearBlockLogic()
    {
        foreach (var blockLogicBase in _blockLogicBases)
        {
            Destroy(blockLogicBase.gameObject);
        }
        _blockLogicBases.Clear();
    }
    
    public void RunSheetBlock()
    {
        StartBlockLogics().Forget();
    }
    async UniTask<bool> StartBlockLogics()
    {
        foreach (var blockLogic in _blockLogicBases)
        {
            bool result = await BlockLogic(blockLogic);
            if (result == false)
            {
                //TODO
                //실패로직 이벤트
                Debug.Log("로직 실패!!");
                return false;
            }
        }
        return true;
    }
    async UniTask<bool> BlockLogic(BlockLogicBase blockLogic)
    {
        bool result = blockLogic.IsExecutable(Stage);
        if (result == false)
            return false;

        while (true)
        {
            bool isComplete = blockLogic.Execute(Stage);
            if (isComplete)
                return true;
            await UniTask.NextFrame();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetManager : MonoBehaviour
{
    [SerializeField]
    private StageManager Stage;
    private List<BlockLogicBase> _blockLogicBases = new List<BlockLogicBase>();

    private bool IsRun = true;
    
    IEnumerator StartBlockLogics()
    {
        IsRun = true;
        foreach (var blockLogic in _blockLogicBases)
        {
            yield return BlockLogic(blockLogic);
            if (IsRun == false)
            {
                //TODO
                //실패로직 이벤트
                yield break;
            }
        }
    }
    IEnumerator BlockLogic(BlockLogicBase blockLogic)
    {
        bool result = true;
        //TODO
        Vector2Int targetPosition = new Vector2Int();
        //TODO
        result = blockLogic.IsExecutable(Stage);

        if (result == false)
        {
            IsRun = false;
            yield break;
        }

        while (true)
        {
            bool isComplete = blockLogic.Execute(Stage);
            if(isComplete)
                yield break;
            yield return null;
        }
    }
}

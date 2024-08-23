using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetManager : MonoBehaviour
{
    [SerializeField]
    private StageManager Stage;
    private List<BlockLogicBase> _blockLogicBases = new List<BlockLogicBase>();

    private bool IsRun = true;

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
        StartCoroutine(StartBlockLogics());
    }
    IEnumerator StartBlockLogics()
    {
        IsRun = true;
        foreach (var blockLogic in _blockLogicBases)
        {
            //Debug.Log(blockLogic);
            yield return BlockLogic(blockLogic);
            if (IsRun == false)
            {
                //TODO
                //실패로직 이벤트
                Debug.Log("로직 실패!!");
                yield break;
            }
        }
    }
    IEnumerator BlockLogic(BlockLogicBase blockLogic)
    {
        bool result = true;
        
        result = blockLogic.IsExecutable(Stage);
        SimulatorManager.Instance.CheckAnswer(result);// To Do

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

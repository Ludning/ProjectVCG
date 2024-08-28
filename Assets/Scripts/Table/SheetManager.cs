using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class SheetManager : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private StageManager Stage;
    [SerializeField] private InteractableManager InteractableManager;
    
    [Header("MainSheet")]
    [SerializeField] private Transform SheetParent;
    private List<BlockLogicBase> _blockLogicBases = new List<BlockLogicBase>();

    [Header("SubSheet")]
    private List<Transform> repeatSheetParents = new List<Transform>();

    public void Init()
    {
        foreach (var interactableButton in InteractableManager.InteractableButtons)
        {
            Debug.Log($"BlockLogicType : {interactableButton.Key}");
            interactableButton.Value.WhenSelect.AddListener(()=>OnClick_SetLogic(interactableButton.Key));
            if( interactableButton.Key == BlockLogicType.Repeat)
            {
                GameObject sheetPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(BlockLogicType.Repeat.ToString());
                GameObject repeatSheet = Instantiate(sheetPrefab);
                //TODO 위치 조정 스크립트도 작성해야함
                repeatSheetParents.Add(repeatSheet.transform);
            }
        }
    }
    private void OnClick_SetLogic(BlockLogicType type)
    {
        switch (type)
        {
            case BlockLogicType.Start:
                RunSheetBlock();
                return;
            case BlockLogicType.Clear:
                ClearBlockLogic();
                return;
        }
        GameObject tempPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>($"{type.ToString()}BlockLogic");
        BlockLogicBase logicBase = Instantiate(tempPrefab).GetComponent<BlockLogicBase>();
        
        if (logicBase != null)
        {
            AddBlockLogic(logicBase);
        }
    }
    public void AddBlockLogic(BlockLogicBase blockLogic)
    {
        blockLogic.transform.SetParent(SheetParent, false);
        _blockLogicBases.Add(blockLogic);
        Debug.Log("AddBlockLogic");
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
        RunBlockLogics().Forget();
    }
    async UniTask<LogicState> RunBlockLogics()
    {
        foreach (var blockLogic in _blockLogicBases)
        {
            LogicState result = await RunBlockLogic(blockLogic);
            if (result == LogicState.Failure)
            {
                Debug.Log("로직 실패!!");
                return LogicState.Failure;
            }
        }
        return LogicState.Success;
    }
    async UniTask<LogicState> RunBlockLogic(BlockLogicBase blockLogic)
    {
    	bool result = blockLogic.IsExecutable(Stage);
        //SimulatorManager.Instance.CheckAnswer(result);// To Do
        if (result == false)
            return LogicState.Failure;

        while (true)
        {
            LogicState logicState = blockLogic.Execute(Stage);
            if (logicState == LogicState.Success)
                return LogicState.Success;
            await UniTask.NextFrame();
        }
    }
}

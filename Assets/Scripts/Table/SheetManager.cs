using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class SheetManager : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private StageManager Stage;
    [SerializeField] private InteractableManager InteractableManager;
    [SerializeField] private NPCManager NPCManager;
    
    [Header("MainSheet")]
    [SerializeField] private Transform SheetParent;
    private List<BlockLogicBase> _blockLogicBases = new List<BlockLogicBase>();

    [Header("SubSheet")]
    private List<Transform> repeatSheetParents = new List<Transform>();
    //임시 코드
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnClick_SetLogic(BlockLogicType.Start);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnClick_SetLogic(BlockLogicType.Cook);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            OnClick_SetLogic(BlockLogicType.Move);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnClick_SetLogic(BlockLogicType.PushItem);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnClick_SetLogic(BlockLogicType.RotateLeft);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnClick_SetLogic(BlockLogicType.RotateRight);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnClick_SetLogic(BlockLogicType.PopItem);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnClick_SetLogic(BlockLogicType.Clear);
        }
    }


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
    async UniTask<ErrorType> RunBlockLogics()
    {
        foreach (var blockLogic in _blockLogicBases)
        {
            ErrorType result = await RunBlockLogic(blockLogic);
            if (result != ErrorType.NoError)
            {
                NPCManager.GetMessage(result);
                Debug.LogError($"ErrorType : {result} ErrorMessage : {DataManager.Instance.GetGameData<ErrorData>(((int)result).ToString()).Context}");
                return result;
            }
        }
        return ErrorType.NoError;
    }
    async UniTask<ErrorType> RunBlockLogic(BlockLogicBase blockLogic)
    {
        ErrorType result = blockLogic.IsExecutable(Stage);
        //SimulatorManager.Instance.CheckAnswer(result);// To Do
        if (result != ErrorType.NoError)
            return result;

        while (true)
        {
            LogicState logicState = blockLogic.Execute(Stage);
            if (logicState == LogicState.Success)
                return ErrorType.NoError;
            await UniTask.NextFrame();
        }
    }


}

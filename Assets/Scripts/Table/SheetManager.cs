using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
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

    [Header("연습장")]
    [SerializeField] private Transform ExSheetParent;
    private List<BlockLogicBase> _exBlockLogicBases = new List<BlockLogicBase>();


    private Dictionary<int, List<BlockLogicBase>> _sheetDictionary = new Dictionary<int, List<BlockLogicBase>>();
    private Dictionary<int, Transform> _sheetParentDictionary = new Dictionary<int, Transform>();
    private List<BlockLogicBase> _currentSheets;
    private Transform _currentSheetParent;


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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            OnClick_SetLogic(BlockLogicType.Reset);
        }
        if (Input.GetKeyDown(KeyCode.O)) OnClick_OpenExercise(); // VR 터치 대신 키입력으로 임시 코드

    }
    public void Init()
    {
        InteractableManager.ExerciseButton.WhenSelect.AddListener(OnClick_OpenExercise);


        foreach (var interactableButton in InteractableManager.InteractableButtons)
        {
            //Debug.Log($"BlockLogicType : {interactableButton.Key}");
            interactableButton.Value.WhenSelect.AddListener(()=>OnClick_SetLogic(interactableButton.Key));
            if( interactableButton.Key == BlockLogicType.Repeat)
            {
                GameObject sheetPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(BlockLogicType.Repeat.ToString());
                GameObject repeatSheet = Instantiate(sheetPrefab);
                //TODO 위치 조정 스크립트도 작성해야함
                repeatSheetParents.Add(repeatSheet.transform);
            }
        }

        SetSheet(0, _blockLogicBases, SheetParent);
        SetSheet(-1, _exBlockLogicBases, ExSheetParent);

        _currentSheets = _sheetDictionary[0];
        _currentSheetParent = _sheetParentDictionary[0];
    }
    private void SetSheet(int index, List<BlockLogicBase> blockLogicList, Transform sheetParent)
    {
        _sheetDictionary.TryAdd(index, blockLogicList);
        _sheetParentDictionary.TryAdd(index, sheetParent);
    }
    private void OnClick_SetLogic(BlockLogicType type)
    {
        switch (type)
        {
            case BlockLogicType.Start:
                RunSheetBlock();
                return;
            case BlockLogicType.Reset: 
                ResetSheetBlock();
                return;
            case BlockLogicType.Clear:
                ClearAllBlockLogic();
                return;
        }
        GameObject tempPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>($"{type.ToString()}BlockLogic");

        AddOutlineComponent(tempPrefab);

        BlockLogicBase logicBase = Instantiate(tempPrefab).GetComponent<BlockLogicBase>();
        
        if (logicBase != null)
        {
            AddBlockLogic(logicBase);
        }
    }

    private void OnClick_OpenExercise()
    {
        //TODO
        //연습장 오픈 기능 구현

        // 연습장 오픈 버튼에 대한 참조를 가지고 있는지 Null Check
        /*if(InteractableManager != null && InteractableManager.ExerciseButton)
        {
            InteractableManager.ExerciseButton.transform.parent.FindChild("Answer");
        }*/
        Stage.UIContainer.ExSheetPopup.gameObject.SetActive(true);
        ExSheetParent.gameObject.SetActive(true);
        _currentSheets = _sheetDictionary[-1];
        _currentSheetParent = _sheetParentDictionary[-1];
    }
    //연습장의 내용이 지워지는지에 따라 내용구현 달라짐
    //TODO
    private void OnClick_ExitExercise()
    {
        Stage.UIContainer.ExSheetPopup.gameObject.SetActive(false);
        ExSheetParent.gameObject.SetActive(false);
        _currentSheets = _sheetDictionary[0];
        _currentSheetParent = _sheetParentDictionary[0];
    }


    public void AddBlockLogic(BlockLogicBase blockLogic)
    {
        blockLogic.transform.SetParent(_currentSheetParent, false);
        _currentSheets.Add(blockLogic);
        Debug.Log("AddBlockLogic");
    }
    public void ClearAllBlockLogic()
    {
        ClearMainBlockLogic();
        ClearRepeatBlockLogic();
        ClearExBlockLogic();
    }
    public void ClearMainBlockLogic()
    {
        foreach (var blockLogicBase in _blockLogicBases)
        {
            Destroy(blockLogicBase.gameObject);
        }
        _blockLogicBases.Clear();
    }
    public void ClearRepeatBlockLogic()
    {
    }
    public void ClearExBlockLogic()
    {
        foreach(var blockLogicBase in _exBlockLogicBases)
        {
            Destroy(blockLogicBase.gameObject);
        }
        _exBlockLogicBases.Clear();
    }


    public void ResetSheetBlock()
    {
        //기능 답안지  유지, 스테이지 원래상태 복귀
        //RunBlockLogic를 중지해야함

        Stage.InteractableManager.InteractableButtons[BlockLogicType.Start].gameObject.SetActive(true);
        Stage.InteractableManager.InteractableButtons[BlockLogicType.Reset].gameObject.SetActive(false);
        Stage.TableManager.ClearTable();
    }
    public void RunSheetBlock()
    {
        Stage.InteractableManager.InteractableButtons[BlockLogicType.Start].gameObject.SetActive(false);
        Stage.InteractableManager.InteractableButtons[BlockLogicType.Reset].gameObject.SetActive(true);
        RunBlockLogics().Forget();
    }
    async UniTask<ErrorType> RunBlockLogics()
    {
        int tempLenght = 0;
        foreach (var blockLogic in _blockLogicBases)
        {

            ErrorType result = await RunBlockLogic(blockLogic);
            if (result != ErrorType.NoError)
            {
                ActiveError(SheetParent, tempLenght);
                NPCManager.GetMessage(result);

                Debug.LogError($"ErrorType : {result} ErrorMessage : {DataManager.Instance.GetGameData<ErrorMessageData>(((int)result).ToString()).Context}");
                return result;
            } else
            {
                ActiveHint(SheetParent,tempLenght);
                NPCManager.GetMessage(result);
            }

            tempLenght++;

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




    public void AddOutlineComponent(GameObject tempPrefab)
    {
        if (!tempPrefab.TryGetComponent<Outline>(out Outline outline))
        {
            // The Outline component doesn't exist, so add it
            outline = tempPrefab.AddComponent<Outline>();
            outline.OutlineWidth = 23f;
            outline.OutlineColor = new Color(0f, 0.87f, 1f); // Adjusted color value

            Debug.Log($"Outline added. Width: {outline.OutlineWidth}, Color: {outline.OutlineColor}");
        }
        else
        {
            outline.OutlineWidth = 23f;
            outline.OutlineColor = new Color(0f, 0.87f, 1f); // Adjusted color value
        }
        outline.enabled = false;
    }
    public void ActiveHint(Transform trs, int len)
    {
        for (int i = 0; i < trs.childCount; i++)
        {
            Transform child = trs.GetChild(i);
            Outline outline = child.GetComponent<Outline>();
            // 자식 오브젝트가 있는 경우 삭제
            if (child.childCount > 0)
            {
                for (int j = 0; j < child.childCount; j++)
                {
                    Destroy(child.GetChild(j).gameObject);
                }
            }

            if (outline != null)
            {
                // Enable the Outline component on the child at index 'len'
                outline.enabled = (i == len);
            }
        }
    }
    public void ActiveError(Transform trs, int len)
    {
        for (int i = 0; i < trs.childCount; i++)
        {
            Transform child = trs.GetChild(i);
            Outline outline = child.GetComponent<Outline>();

            if (outline != null)
            {
                // Enable the Outline component on the child at index 'len'
                outline.enabled = false;

                GameObject errorWarningPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("ErrorWarning");
                if (i == len && errorWarningPrefab && child.childCount < 1)
                {

                    Instantiate(errorWarningPrefab, child);
                }
            }
        }
    }
    public void Clear()
    {
        foreach (var blockLogic in _blockLogicBases)
        {
            Destroy(blockLogic.gameObject);
        }
        _blockLogicBases.Clear();
        foreach (var repeatSheetParent in repeatSheetParents)
        {
            Destroy(repeatSheetParent.gameObject);
        }
        repeatSheetParents.Clear();
    }
}

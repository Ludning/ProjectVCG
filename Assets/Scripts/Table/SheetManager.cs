
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEditor.SceneManagement;
using Oculus.Interaction;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class SheetManager : MonoBehaviour
{
    [SerializeField, ReadOnly] private bool _isLogicRunning = false;
    [SerializeField, ReadOnly] private bool _isExBlockLogic = false;

    [FormerlySerializedAs("Stage")] [Header("Manager")] [SerializeField]
    private StageManager StageManager;

    [SerializeField] private InteractableManager InteractableManager;
    [SerializeField] private NPCManager NPCManager;
    [SerializeField] private RepeatFunctionSheetContainer RepeatFunctionSheetContainer;


    //MainSheet
    [Header("MainSheet")] [SerializeField] private MainSheet MainSheet;
    private List<BlockLogicBase> _mainBlockLogicBases = new List<BlockLogicBase>();

    //SubSheet
    private Dictionary<int, RepeatSheet> RepeatFunctionSheets = new Dictionary<int, RepeatSheet>();
    private Dictionary<int, List<BlockLogicBase>> _repeatBlockLogicBasesDictionary =
        new Dictionary<int, List<BlockLogicBase>>();

    //ExSheet
    [Header("연습장")] [SerializeField] private ExSheet ExSheet;
    private List<BlockLogicBase> _exBlockLogicBases = new List<BlockLogicBase>();

    //함수블럭의 반복횟수를 저장
    private Dictionary<int, int> _repeatCountDictionary = new Dictionary<int, int>();
    
    
    private Dictionary<int, List<BlockLogicBase>> _sheetDictionary = new Dictionary<int, List<BlockLogicBase>>();
    private Dictionary<int, Transform> _sheetParentDictionary = new Dictionary<int, Transform>();

    //현재 활성화된 시트
    private List<BlockLogicBase> _currentSheet;
    private Transform _currentSheetParent;

    //private int _sheetIndex;
    private Dictionary<InteractableUnityEventWrapper, int> _sheetIndexDictionary =
        new Dictionary<InteractableUnityEventWrapper, int>();

    //임시 코드
    private void Update()
    {
        Dictionary<BlockLogicType, InteractableUnityEventWrapper> InteractableButtons =
            InteractableManager.InteractableButtons;
        Dictionary<int, InteractableUnityEventWrapper> FunctionInteractableButtons =
            InteractableManager.FunctionInteractableButtons;
        Dictionary<int, InteractableUnityEventWrapper> RepeatInteractableButtons =
            InteractableManager.RepeatInteractableButtons;

        if (Input.GetKeyDown(KeyCode.S))
            RunSheetBlock();
        
        if (Input.GetKeyDown(KeyCode.X))
            ClearAllBlockLogic();

        if (Input.GetKeyDown(KeyCode.Z))
            ResetSheetBlock();
        
        
        if (Input.GetKeyDown(KeyCode.C))
            ClickLogic(BlockLogicType.Cook, InteractableButtons[BlockLogicType.Cook]);

        if (Input.GetKeyDown(KeyCode.M))
            ClickLogic(BlockLogicType.Move, InteractableButtons[BlockLogicType.Move]);

        if (Input.GetKeyDown(KeyCode.P))
            ClickLogic(BlockLogicType.PushItem, InteractableButtons[BlockLogicType.PushItem]);

        if (Input.GetKeyDown(KeyCode.L))
            ClickLogic(BlockLogicType.RotateLeft, InteractableButtons[BlockLogicType.RotateLeft]);

        if (Input.GetKeyDown(KeyCode.R))
            ClickLogic(BlockLogicType.RotateRight, InteractableButtons[BlockLogicType.RotateRight]);

        if (Input.GetKeyDown(KeyCode.I))
            ClickLogic(BlockLogicType.PopItem, InteractableButtons[BlockLogicType.PopItem]);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Alpha1");
            ClickLogic(BlockLogicType.Function, FunctionInteractableButtons.First().Value);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Alpha2");
            ClickLogic(BlockLogicType.Repeat, RepeatInteractableButtons.First().Value);
        }

        if (Input.GetKeyDown(KeyCode.O))
            OnClick_OpenExercise(); // VR 터치 대신 키입력으로 임시 코드
    }

    //초기화 함수
    //상호작용 버튼을 생성하고 BlockLogic이 추가되는 이벤트를 연결해준다
    public void Init()
    {
        InteractableManager.ExerciseButton.WhenSelect.AddListener(OnClick_OpenExercise);

        foreach (var interactableButton in InteractableManager.InteractableButtons)
        {
            UnityAction action = GetLogicEvent(interactableButton.Key, interactableButton.Value);
            interactableButton.Value.WhenSelect.AddListener(action);
        }
        foreach (var interactableButton in InteractableManager.FunctionInteractableButtons)
        {
            UnityAction action = GetLogicEvent(BlockLogicType.Function, interactableButton.Value);
            interactableButton.Value.WhenSelect.AddListener(action);

            GameObject sheetPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("FunctionSheet");
            RepeatSheet functionSheet = Instantiate(sheetPrefab, RepeatFunctionSheetContainer.transform).GetComponent<RepeatSheet>();
            
            //TODO 위치 조정 스크립트도 작성해야함
            
            RepeatFunctionSheets.Add(interactableButton.Key, functionSheet);
            _repeatBlockLogicBasesDictionary.Add(interactableButton.Key, new List<BlockLogicBase>());
            _sheetIndexDictionary.Add(interactableButton.Value, interactableButton.Key);
            functionSheet.GetComponent<RepeatSheet>().Init(this, interactableButton.Key);
            _repeatCountDictionary.Add(interactableButton.Key, 1);

            SetSheet(interactableButton.Key, _repeatBlockLogicBasesDictionary[interactableButton.Key], functionSheet.SheetParent);
        }
        foreach (var interactableButton in InteractableManager.RepeatInteractableButtons)
        {
            UnityAction action = GetLogicEvent(BlockLogicType.Repeat, interactableButton.Value);
            interactableButton.Value.WhenSelect.AddListener(action);
            
            GameObject sheetPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("RepeatSheet");
            RepeatSheet repeatSheet = Instantiate(sheetPrefab, RepeatFunctionSheetContainer.transform).GetComponent<RepeatSheet>();
            
            RepeatFunctionSheets.Add(interactableButton.Key, repeatSheet);
            _repeatBlockLogicBasesDictionary.Add(interactableButton.Key, new List<BlockLogicBase>());
            _sheetIndexDictionary.Add(interactableButton.Value, interactableButton.Key);
            repeatSheet.GetComponent<RepeatSheet>().Init(this, interactableButton.Key);
            _repeatCountDictionary.Add(interactableButton.Key, 1);

            SetSheet(interactableButton.Key, _repeatBlockLogicBasesDictionary[interactableButton.Key], repeatSheet.SheetParent);
        }
        List<Transform> sheetTransformList = new List<Transform>();
        foreach(var sheet in RepeatFunctionSheets.Values)
        {
            sheetTransformList.Add(sheet.transform);
        }
        RepeatFunctionSheetContainer.AddSheet(sheetTransformList);

        SetSheet(0, _mainBlockLogicBases, MainSheet.SheetParent);
        SetSheet(-1, _exBlockLogicBases, ExSheet.SheetParent);

        MainSheet.gameObject.SetActive(true);
        foreach (var repeatSheet in RepeatFunctionSheets.Values)
            repeatSheet.gameObject.SetActive(true);
        ExSheet.gameObject.SetActive(false);

        ChoiceSheet(0);
    }

    private void SetSheet(int index, List<BlockLogicBase> blockLogicList, Transform sheetParent)
    {
        _sheetDictionary.TryAdd(index, blockLogicList);
        _sheetParentDictionary.TryAdd(index, sheetParent);
    }

    public void ChoiceSheet(int index)
    {
        _isExBlockLogic = (index == -1) ? true : false;
        _currentSheet = _sheetDictionary[index];
        _currentSheetParent = _sheetParentDictionary[index];
    }

    #region 블록로직 오브젝트 생성
    private UnityAction GetLogicEvent(BlockLogicType type, InteractableUnityEventWrapper interactableUnityEventWrapper)
    {
        switch (type)
        {
            case BlockLogicType.Start:
                return RunSheetBlock;
            case BlockLogicType.Reset:
                return ResetSheetBlock;
            case BlockLogicType.Clear:
                return ClearAllBlockLogic;
            default:
                return () => { ClickLogic(type, interactableUnityEventWrapper); };
        }
        return null;
    }
    private void ClickLogic(BlockLogicType type, InteractableUnityEventWrapper interactableUnityEventWrapper)
    {
        //함수 시트에 함수가 들어가지 못하게 막기용 기능
        if (_currentSheet != _mainBlockLogicBases)
        {
            if (type == BlockLogicType.Repeat || type == BlockLogicType.Function)
                return;
        }
        
        GameObject tempPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>($"{type.ToString()}BlockLogic");
        AddOutlineComponent(tempPrefab);

        BlockLogicBase logicBase = Instantiate(tempPrefab).GetComponent<BlockLogicBase>();
        if (logicBase != null)
        {
            AddBlockLogic(logicBase);
            if (logicBase is RepeatBlockLogic rBlockLogic)
                if (_sheetIndexDictionary.TryGetValue(interactableUnityEventWrapper, out int index))
                    rBlockLogic.SheetIndex = index;
            if (logicBase is FunctionBlockLogic fBlockLogic)
                if (_sheetIndexDictionary.TryGetValue(interactableUnityEventWrapper, out int index))
                    fBlockLogic.SheetIndex = index;
        }
    }
    #endregion
    
    private void OnClick_OpenExercise()
    {
        ExSheet.gameObject.SetActive(true);
        ChoiceSheet(-1);
    }

    //연습장의 내용을 지우며 ExSheet비활성화
    public void OnClick_ExitExercise()
    {
        ExSheet.gameObject.SetActive(false);
        _currentSheet = _sheetDictionary[0];
        _currentSheetParent = _sheetParentDictionary[0];
    }

    public void SetRepeatCount(int index, int count)
    {
        _repeatCountDictionary[index] = count;
    }

    public void AddBlockLogic(BlockLogicBase blockLogic)
    {
        blockLogic.transform.SetParent(_currentSheetParent, false);
        _currentSheet.Add(blockLogic);
    }

    public void ClearAllBlockLogic()
    {
        //_sheetIndex = 1;
        ClearMainBlockLogic();
        ClearRepeatBlockLogic();
        ClearExBlockLogic();
    }

    public void ClearMainBlockLogic()
    {
        foreach (var blockLogicBase in _mainBlockLogicBases)
        {
            Destroy(blockLogicBase.gameObject);
        }
        _mainBlockLogicBases.Clear();
    }

    public void ClearRepeatBlockLogic()
    {
        foreach (var blockLogicBaseKeyValue in _repeatBlockLogicBasesDictionary)
        {
            foreach (var blockLogicBase in blockLogicBaseKeyValue.Value)
            {
                Destroy(blockLogicBase.gameObject);
            }
            blockLogicBaseKeyValue.Value.Clear();
        }
        _repeatBlockLogicBasesDictionary.Clear();
    }

    //ExBlock 비우기
    public void ClearExBlockLogic()
    {
        foreach (var blockLogicBase in _exBlockLogicBases)
        {
            Destroy(blockLogicBase.gameObject);
        }

        _exBlockLogicBases.Clear();
    }


    public void ResetSheetBlock()
    {
        //기능 답안지  유지, 스테이지 원래상태 복귀
        //RunBlockLogic를 중지해야함

        StageManager.InteractableManager.InteractableButtons[BlockLogicType.Start].gameObject.SetActive(true);
        StageManager.InteractableManager.InteractableButtons[BlockLogicType.Reset].gameObject.SetActive(false);
        StageManager.TableManager.ClearTable();
    }

    public void RunSheetBlock()
    {
        if (_isLogicRunning == true)
        {
            Debug.Log("IsLogicRunning");
            return;
        }

        if (_isExBlockLogic == true)
        {
            Debug.Log("IsExBlockLogic");
            return;
        }

        StageManager.InteractableManager.InteractableButtons[BlockLogicType.Start].gameObject.SetActive(false);
        StageManager.InteractableManager.InteractableButtons[BlockLogicType.Reset].gameObject.SetActive(true);
        _isLogicRunning = true;
        RunBlockLogics(0).Forget();
    }

    #region BlockLogic 구동부
    async UniTask<ErrorType> RunBlockLogics(int sheetIndex)
    {
        int tempLenght = 0;

        //foreach (var blockLogic in _blockLogicBases)
        foreach (var blockLogic in _sheetDictionary[sheetIndex])
        {
            ErrorType result = await RunBlockLogic(blockLogic);
            if (result != ErrorType.NoError)
            {
                if (sheetIndex == 0)
                {
                    ActiveError(MainSheet.SheetParent, tempLenght);
                    Debug.LogError(
                        $"ErrorType : {result} ErrorMessage : {DataManager.Instance.GetGameData<ErrorMessageData>(((int)result).ToString()).Context}");
                    NPCManager.GetMessage(result);
                    _isLogicRunning = false;
                }
                return result;
            }
            else
            {
                ActiveHint(MainSheet.SheetParent, tempLenght);
                NPCManager.GetMessage(result);
            }
            tempLenght++;
        }
        _isLogicRunning = false;
        return ErrorType.NoError;
    }

    async UniTask<ErrorType> RunBlockLogic(BlockLogicBase blockLogic)
    {
        Debug.Log("RunBlockLogic");
        if (blockLogic is FunctionBlockLogic functionBlockLogic)
        {
            return await RunBlockLogics(functionBlockLogic.SheetIndex);
        }
        if (blockLogic is RepeatBlockLogic repeatBlockLogic)
        {
            for (int i = 0; i < _repeatCountDictionary[repeatBlockLogic.SheetIndex]; i++)
            {
                ErrorType repeatResult = await RunBlockLogics(repeatBlockLogic.SheetIndex);
                if (repeatResult != ErrorType.NoError)
                    return repeatResult;
            }
        }

        ErrorType result = blockLogic.IsExecutable(StageManager);
        if (result != ErrorType.NoError)
            return result;

        while (true)
        {
            LogicState logicState = blockLogic.Execute(StageManager);
            if (logicState == LogicState.Success)
                return ErrorType.NoError;
            await UniTask.NextFrame();
        }
    }
    #endregion
    
    #region Highlights
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

                GameObject errorWarningPrefab =
                    ResourceManager.Instance.LoadResourceWithCaching<GameObject>("ErrorWarning");
                if (i == len && errorWarningPrefab && child.childCount < 1)
                {

                    Instantiate(errorWarningPrefab, child);
                }
            }
        }
    }

    #endregion

    public void Clear()
    {
        foreach (var blockLogic in _mainBlockLogicBases)
        {
            Destroy(blockLogic.gameObject);
        }

        _mainBlockLogicBases.Clear();

        foreach (var repeatBlockLogicBases in _repeatBlockLogicBasesDictionary)
        {
            foreach (var blockLogic in repeatBlockLogicBases.Value)
                Destroy(blockLogic.gameObject);
        }

        _repeatBlockLogicBasesDictionary.Clear();

        foreach (var repeatSheet in RepeatFunctionSheets)
        {
            Destroy(repeatSheet.Value.gameObject);
        }

        RepeatFunctionSheets.Clear();
    }
}

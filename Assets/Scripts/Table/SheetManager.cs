using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Oculus.Interaction;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SheetManager : MonoBehaviour
{
    [SerializeField, ReadOnly] private bool _isLogicRunning = false;
    [SerializeField, ReadOnly] private bool _isExBlockLogic = false;

    private bool _isStartResetWaiting = false;

    [Header("Manager")] [SerializeField]
    private StageManager StageManager;

    [SerializeField] private InteractableManager InteractableManager;
    [SerializeField] private NPCManager NPCManager;
    [SerializeField] private RepeatFunctionSheetContainer RepeatFunctionSheetContainer;


    //MainSheet
    [Header("MainSheet")] [SerializeField] private MainSheet MainSheet;
    //private List<BlockLogicBase> _mainBlockLogicBases = new List<BlockLogicBase>();

    //SubSheet
    private Dictionary<int, RepeatSheet> RepeatFunctionSheets = new Dictionary<int, RepeatSheet>();
    //private Dictionary<int, List<BlockLogicBase>> _repeatFunctionDictionary = new Dictionary<int, List<BlockLogicBase>>();

    //ExSheet
    [Header("연습장")] [SerializeField] private ExSheet ExSheet;
    //private List<BlockLogicBase> _exBlockLogicBases = new List<BlockLogicBase>();

    //함수블럭의 반복횟수를 저장
    private Dictionary<int, int> _repeatCountDictionary = new Dictionary<int, int>();
    
    
    private Dictionary<int, SheetBase> _sheetDictionary = new Dictionary<int, SheetBase>();
    private Dictionary<int, List<BlockLogicBase>> _blockLogicListDictionary = new Dictionary<int, List<BlockLogicBase>>();

    //현재 활성화된 시트
    private SheetBase _currentSheet;
    private List<BlockLogicBase> _currentBlockLogicList;

    //private int _sheetIndex;
    private Dictionary<GrapAndPoke, int> _sheetIndexDictionary = new Dictionary<GrapAndPoke, int>();

    //임시 코드
    private void Update()
    {
        Dictionary<BlockLogicType, GrapAndPoke> InteractableButtons = InteractableManager.GrapAndPokeObjects;
        Dictionary<int, GrapAndPoke> FunctionInteractableButtons = InteractableManager.FunctionGrapAndPokeObjects;
        Dictionary<int, GrapAndPoke> RepeatInteractableButtons = InteractableManager.RepeatGrapAndPokeObjects;

        if (Input.GetKeyDown(KeyCode.S))
            RunSheetBlock();
        
        if (Input.GetKeyDown(KeyCode.X))
            ClearAllBlockLogic();

        if (Input.GetKeyDown(KeyCode.Z))
            ResetStage();
        
        
        if (Input.GetKeyDown(KeyCode.C))
            ClickLogicButton(BlockLogicType.Cook, InteractableButtons[BlockLogicType.Cook]);

        if (Input.GetKeyDown(KeyCode.M))
            ClickLogicButton(BlockLogicType.Move, InteractableButtons[BlockLogicType.Move]);

        if (Input.GetKeyDown(KeyCode.P))
            ClickLogicButton(BlockLogicType.PushItem, InteractableButtons[BlockLogicType.PushItem]);

        if (Input.GetKeyDown(KeyCode.L))
            ClickLogicButton(BlockLogicType.RotateLeft, InteractableButtons[BlockLogicType.RotateLeft]);

        if (Input.GetKeyDown(KeyCode.R))
            ClickLogicButton(BlockLogicType.RotateRight, InteractableButtons[BlockLogicType.RotateRight]);

        if (Input.GetKeyDown(KeyCode.I))
            ClickLogicButton(BlockLogicType.PopItem, InteractableButtons[BlockLogicType.PopItem]);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Alpha1");
            ClickLogicButton(BlockLogicType.Function, FunctionInteractableButtons.First().Value);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Alpha2");
            ClickLogicButton(BlockLogicType.Repeat, RepeatInteractableButtons.First().Value);
        }

        if (Input.GetKeyDown(KeyCode.O))
            OnClick_OpenExercise(); // VR 터치 대신 키입력으로 임시 코드
    }

    //초기화 함수
    //상호작용 버튼을 생성하고 BlockLogic이 추가되는 이벤트를 연결해준다
    public void Init()
    {
        InteractableManager.ExerciseButton.WhenSelect.AddListener(OnClick_OpenExercise);
        InteractableManager.StartButton.WhenSelect.AddListener(RunSheetBlock);
        InteractableManager.ResetButton.WhenSelect.AddListener(ResetStage);
        InteractableManager.ClearButton.WhenSelect.AddListener(ClearAllBlockLogic);
        
        StageData stageData = GameManager.Instance.GetCurrentStageData();
        
        MainSheet.Init(stageData.AnswerBlockAmount);
        ExSheet.Init(-1);
        
        foreach (var grapAndPoke in InteractableManager.GrapAndPokeObjects)
        {
            UnityAction action = GetLogicEvent(grapAndPoke.Key, grapAndPoke.Value);
            grapAndPoke.Value.pokeInteractable.WhenSelect.AddListener(action);
        }
        foreach (var grapAndPoke in InteractableManager.FunctionGrapAndPokeObjects)
        {
            UnityAction action = GetLogicEvent(BlockLogicType.Function, grapAndPoke.Value);
            grapAndPoke.Value.pokeInteractable.WhenSelect.AddListener(action);

            RepeatSheet functionSheet = InstantiateRepeatSheet("FunctionSheet", grapAndPoke.Value);

            AddRepeatFunctionSheet(functionSheet, grapAndPoke.Key, grapAndPoke.Value);
            SetSheet(grapAndPoke.Key, functionSheet, _repeatFunctionDictionary[grapAndPoke.Key]);
        }
        foreach (var grapAndPoke in InteractableManager.RepeatGrapAndPokeObjects)
        {
            UnityAction action = GetLogicEvent(BlockLogicType.Repeat, grapAndPoke.Value);
            grapAndPoke.Value.pokeInteractable.WhenSelect.AddListener(action);
            
            RepeatSheet repeatSheet = InstantiateRepeatSheet("RepeatSheet", grapAndPoke.Value);

            AddRepeatFunctionSheet(repeatSheet, grapAndPoke.Key, grapAndPoke.Value);
            SetSheet(grapAndPoke.Key, repeatSheet, _repeatFunctionDictionary[grapAndPoke.Key]);
        }
        List<Transform> sheetTransformList = new List<Transform>();
        foreach(var sheet in RepeatFunctionSheets.Values)
        {
            sheetTransformList.Add(sheet.transform);
        }
        RepeatFunctionSheetContainer.AddSheet(sheetTransformList);

        SetSheet(0, MainSheet, _mainBlockLogicBases);
        SetSheet(-1, ExSheet, _exBlockLogicBases);

        MainSheet.gameObject.SetActive(true);
        foreach (var repeatSheet in RepeatFunctionSheets.Values)
            repeatSheet.gameObject.SetActive(true);
        ExSheet.gameObject.SetActive(false);

        ChoiceSheet(0);
    }

    private RepeatSheet InstantiateRepeatSheet(string sheetName, GrapAndPoke grapAndPoke)
    {
        GameObject sheetPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(sheetName);
        RepeatSheet repeatSheet = Instantiate(sheetPrefab, RepeatFunctionSheetContainer.transform).GetComponent<RepeatSheet>();
        
        FunctionBlockData functionBlockData = grapAndPoke.GetComponent<FunctionBlockData>();
        repeatSheet.Init(functionBlockData.functionLimit);
        repeatSheet.SheetName = functionBlockData.sheetName;
        
        return repeatSheet;
    }
    private void AddRepeatFunctionSheet(RepeatSheet sheet, int index, GrapAndPoke grapAndPoke)
    {
        RepeatFunctionSheets.Add(index, sheet);
        _repeatFunctionDictionary.Add(index, new List<BlockLogicBase>());
        _sheetIndexDictionary.Add(grapAndPoke, index);
        sheet.InitRepeat(this, index);
        _repeatCountDictionary.Add(index, 1);
    }
    private void SetSheet(int index, SheetBase sheetBase, List<BlockLogicBase> blockLogicList)
    {
        _sheetDictionary.Add(index, sheetBase);
        _blockLogicListDictionary.Add(index, blockLogicList);
    }
    public void ChoiceSheet(int index)
    {
        _isExBlockLogic = (index == -1) ? true : false;
        _currentSheet = _sheetDictionary[index];
        _currentBlockLogicList = _blockLogicListDictionary[index];
    }

    #region 블록로직 오브젝트 생성
    private UnityAction GetLogicEvent(BlockLogicType type, GrapAndPoke grapAndPoke)
    {
        switch (type)
        {
            case BlockLogicType.Start:
                Debug.LogError("GetLogicEvent Start");
                break;
            case BlockLogicType.Reset:
                Debug.LogError("GetLogicEvent Reset");
                break;
            case BlockLogicType.Clear:
                Debug.LogError("GetLogicEvent Clear");
                break;
        }
        return () => { ClickLogicButton(type, grapAndPoke); };
    }
    private void ClickLogicButton(BlockLogicType type, GrapAndPoke grapAndPoke)
    {
        if (_currentSheet.SheetLimit != -1 && _currentSheet.SheetLimit <= _currentSheet.BlockCount)
            return;
        
        //함수 시트에 함수가 들어가지 못하게 막기용 기능
        if (_currentBlockLogicList != _mainBlockLogicBases)
        {
            if (type == BlockLogicType.Repeat || type == BlockLogicType.Function)
                return;
        }
        
        GameObject tempPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>($"{type.ToString()}BlockLogic");
        AddOutlineComponent(tempPrefab);

        BlockLogicBase logicBase = Instantiate(tempPrefab).GetComponent<BlockLogicBase>();
        var dataDictionary = DataManager.Instance.GetGameDataDictionary<CodingBlockData>();
        foreach (var codingBlockData in dataDictionary.Values)
        {
            if (codingBlockData.Type == type)
            {
                if (logicBase is RepeatBlockLogic repeat)
                {
                    repeat.RepeatText.text = grapAndPoke.GetComponent<FunctionBlockData>().sheetName;
                    break;
                }
                else if(logicBase is FunctionBlockLogic function)
                {
                    function.FunctionText.text = grapAndPoke.GetComponent<FunctionBlockData>().sheetName;
                    break;
                }
                else
                {
                    logicBase.BlockIcon = ResourceManager.Instance.LoadResourceWithCaching<Sprite>(codingBlockData.IconBlock);
                    break;
                }
            }
        }
        if (logicBase != null)
        {
            AddBlockLogic(logicBase);
            if (logicBase is RepeatBlockLogic rBlockLogic)
                if (_sheetIndexDictionary.TryGetValue(grapAndPoke, out int index))
                    rBlockLogic.SheetIndex = index;
            if (logicBase is FunctionBlockLogic fBlockLogic)
                if (_sheetIndexDictionary.TryGetValue(grapAndPoke, out int index))
                    fBlockLogic.SheetIndex = index;
        }
        if(_currentSheet.SheetLimit != -1)
            _currentSheet.BlockCount++;
    }
    #endregion
    
    #region 연습장
    private void OnClick_OpenExercise()
    {
        MainSheet.gameObject.SetActive(false);
        foreach (var repeatSheet in RepeatFunctionSheets.Values)
            repeatSheet.gameObject.SetActive(false);
        ExSheet.gameObject.SetActive(true);
        ChoiceSheet(-1);
    }

    //연습장의 내용을 지우며 ExSheet비활성화
    public void OnClick_ExitExercise()
    {
        MainSheet.gameObject.SetActive(true);
        foreach (var repeatSheet in RepeatFunctionSheets.Values)
            repeatSheet.gameObject.SetActive(true);
        ExSheet.gameObject.SetActive(false);
        
        ChoiceSheet(0);
    }
    #endregion

    public void SetRepeatCount(int index, int count)
    {
        _repeatCountDictionary[index] = count;
    }

    public void AddBlockLogic(BlockLogicBase blockLogic)
    {
        _currentSheet.Push(blockLogic);
        _currentBlockLogicList.Add(blockLogic);
    }

    public void ClearAllBlockLogic()
    {
        ClearMainBlockLogic();
        ClearRepeatBlockLogic();
        ClearExBlockLogic();
    }
    public void ClearMainBlockLogic()
    {
        MainSheet.BlockCount = 0;
        foreach (var blockLogicBase in _mainBlockLogicBases)
        {
            Destroy(blockLogicBase.gameObject);
        }
        _mainBlockLogicBases.Clear();
    }
    public void ClearRepeatBlockLogic()
    {
        foreach (var RepeatFunctionSheet in RepeatFunctionSheets.Values)
        {
            RepeatFunctionSheet.BlockCount = 0;
        }
        foreach (var blockLogicBaseKeyValue in _repeatFunctionDictionary)
        {
            foreach (var blockLogicBase in blockLogicBaseKeyValue.Value)
            {
                Destroy(blockLogicBase.gameObject);
            }
            blockLogicBaseKeyValue.Value.Clear();
        }
        //_repeatFunctionDictionary.Clear();
    }
    public void ClearExBlockLogic()
    {
        foreach (var blockLogicBase in _exBlockLogicBases)
        {
            Destroy(blockLogicBase.gameObject);
        }

        _exBlockLogicBases.Clear();
    }


    private async UniTaskVoid StartResetWaiting()
    {
        _isStartResetWaiting = true;
        await UniTask.Delay(2000);
        _isStartResetWaiting = false;
    }
    public void ResetStage()
    {
        if (_isStartResetWaiting == true)
        {
            Debug.Log("IsStartResetWaiting");
            return;
        }
        
        StartResetWaiting().Forget();
        StageManager.ResetStage();
    }

    public async void RunSheetBlock()
    {
        if (_isStartResetWaiting == true)
        {
            Debug.Log("IsStartResetWaiting");
            return;
        }
        
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
        
        StartResetWaiting().Forget();

        StageManager.InteractableManager.GrapAndPokeObjects[BlockLogicType.Start].gameObject.SetActive(false);
        StageManager.InteractableManager.GrapAndPokeObjects[BlockLogicType.Reset].gameObject.SetActive(true);
        _isLogicRunning = true;
        
        StageManager.Controller.SetAnimationState(AnimationState.IsIdle, true);
        StageManager.Controller.SetAnimationState(AnimationState.IsCarry, false);
        
        ErrorType result = await RunBlockLogics(0);
        Debug.Log(result);
        OnTaskCompleted(result);
    }

    private void OnTaskCompleted(ErrorType type)
    {
        Debug.Log($"ErrorType : {type}");
        if (type == ErrorType.StageClear)
        {
            if (GameManager.Instance.HasNextStage())
            {
                StageManager.ClearStage();
                
                //다음 스테이지가 있을 시 자동으로 넘어감
                GameManager.Instance.SelectedStageIndex++;
                StageManager.InitStage();
                Debug.Log("다음 스테이지가 있을 시 자동으로 넘어감");
            }
            else
            {
                if (GameManager.Instance.HasNextChapter())
                {
                    //다음 스테이지가 없을 시 Clear Popup을 띄움
                    StageManager.UIContainer.ClearPopup.gameObject.SetActive(true);
                    Debug.Log("다음 스테이지가 없을 시 Clear Popup을 띄움");
                }
                else
                {
                    //다음 스테이지가 없고 다음 챕터도 없을 때는?
                    Debug.Log("다음 스테이지가 없고 다음 챕터도 없을 때는?");
                }
            }
        }
    }

    #region BlockLogic 구동부
    async UniTask<ErrorType> RunBlockLogics(int sheetIndex)
    {
        StageManager.Controller.LogicHint.HideLogicHint();
        int tempLenght = 0;

        foreach (var blockLogic in _blockLogicListDictionary[sheetIndex])
        {
            ActiveHint(blockLogic);
            ErrorType result = await RunBlockLogic(blockLogic);
            
            if (result == ErrorType.StageClear)
            {
                StageManager.Controller.LogicHint.HideLogicHint();
                _isLogicRunning = false;
                Debug.Log("StageClear");
                GameManager.Instance.HasNextStage();
                return ErrorType.StageClear;
            }
            if (result != ErrorType.NoError && result != ErrorType.OmissionError)
            {
                if (sheetIndex == 0)
                {
                    ActiveError(blockLogic);
                    Debug.LogError(
                        $"ErrorType : {result} ErrorMessage : {DataManager.Instance.GetGameData<ErrorMessageData>(((int)result).ToString()).Context}");
                    NPCManager.GetMessage(result);
                    _isLogicRunning = false;
                }
                return result;
            }
            else
            {
                //ActiveHint(blockLogic);
                NPCManager.GetMessage(result);
            }
            tempLenght++;
        }
        _isLogicRunning = false;
        return ErrorType.NoError;
    }

    async UniTask<ErrorType> RunBlockLogic(BlockLogicBase blockLogic)
    {
        Debug.Log($"CurrentLogicType : {blockLogic.GetType()}");
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
        
        if (blockLogic is CookLogic cook)
            Debug.Log("CookLogic");
        
        StageManager.Controller.LogicHint.ShowLogicHint();
        StageManager.Controller.LogicHint.SetLogicImage(blockLogic.BlockIcon);
        ErrorType result = blockLogic.IsExecutable(StageManager);
        if (result != ErrorType.NoError)
        {
            StageManager.Controller.SetAnimationState(AnimationState.IsIdle, true);
            return result;
        }
        
        StageManager.Controller.SetAnimationState(AnimationState.IsIdle, false);
        while (true)
        {
            LogicState logicState = blockLogic.Execute(StageManager);
            if (logicState == LogicState.Success)
            {
                StageManager.Controller.SetAnimationState(AnimationState.IsIdle, true);
                blockLogic.CheakClear(StageManager);
                return (StageManager.levelManager.IsAllComplete == true) ? ErrorType.StageClear : ErrorType.NoError;
            }
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
            outline.OutlineWidth = 50f;
            outline.OutlineColor = new Color(0f, 0.87f, 1f); // Adjusted color value

            Debug.Log($"Outline added. Width: {outline.OutlineWidth}, Color: {outline.OutlineColor}");
        }
        else
        {
            outline.OutlineWidth = 50f;
            outline.OutlineColor = new Color(0f, 0.87f, 1f); // Adjusted color value
        }

        outline.enabled = false;
    }

    private Outline prevOutline;
    public void ActiveHint(BlockLogicBase blockLogicBase)
    {
        if (prevOutline != null)
            prevOutline.enabled = false;
        Outline outline = blockLogicBase.GetComponent<Outline>();
        outline.enabled = true;
        prevOutline = outline;
    }

    public void ActiveError(BlockLogicBase blockLogicBase)
    {
        Outline outline = blockLogicBase.GetComponent<Outline>();
        outline.enabled = false;
        GameObject errorWarningPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("ErrorWarning");
        Instantiate(errorWarningPrefab, blockLogicBase.transform);
    }

    #endregion

    public void Clear()
    {
        MainSheet.Clear();
        //반복 함수 시트 제거
        foreach (var repeatSheet in RepeatFunctionSheets)
            Destroy(repeatSheet.Value.gameObject);
        
        RepeatFunctionSheets.Clear();
        ClearAllBlockLogic();
        _repeatFunctionDictionary.Clear();
        
        _sheetDictionary.Clear();
        _blockLogicListDictionary.Clear();
        
        _repeatCountDictionary.Clear();
        MainSheet.gameObject.SetActive(false);
        ExSheet.gameObject.SetActive(false);
    }
}

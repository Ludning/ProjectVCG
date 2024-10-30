using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Oculus.Interaction;
using System.Linq;
using System.Threading;
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


    //MainSheet, SubSheet, ExSheet
    [Header("MainSheet")] [SerializeField] private MainSheet MainSheet;
    private Dictionary<int, RepeatSheet> SubSheets = new Dictionary<int, RepeatSheet>();
    [Header("연습장")] [SerializeField] private ExSheet ExSheet;

    
    //함수블럭의 반복횟수를 저장
    private Dictionary<int, int> _repeatCountDictionary = new Dictionary<int, int>();
    
    private Dictionary<int, SheetBase> _sheetDictionary = new Dictionary<int, SheetBase>();

    //현재 활성화된 시트
    private SheetBase _currentSheet;
    //private List<BlockLogicBase> _currentBlockLogicList;

    //private int _sheetIndex;
    private Dictionary<GrapAndPokeObject, int> _sheetIndexDictionary = new Dictionary<GrapAndPokeObject, int>();

    [SerializeField] private Color selectSheetColor;
    [SerializeField] private Color unSelectSheetColor;

    //임시 코드
    private void Update()
    {
        Dictionary<BlockLogicType, GrapAndPokeObject> InteractableButtons = InteractableManager.GrapAndPokeObjects;
        Dictionary<int, GrapAndPokeObject> FunctionInteractableButtons = InteractableManager.FunctionGrapAndPokeObjects;
        Dictionary<int, GrapAndPokeObject> RepeatInteractableButtons = InteractableManager.RepeatGrapAndPokeObjects;

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
            
            grapAndPoke.Value.OnGrapReleasedAddBlock -= OnGrapLogicReleased;
            grapAndPoke.Value.OnGrapReleasedAddBlock += OnGrapLogicReleased;
        }
        foreach (var grapAndPoke in InteractableManager.FunctionGrapAndPokeObjects)
        {
            UnityAction action = GetLogicEvent(BlockLogicType.Function, grapAndPoke.Value);
            grapAndPoke.Value.pokeInteractable.WhenSelect.AddListener(action);
            
            grapAndPoke.Value.OnGrapReleasedAddBlock -= OnGrapLogicReleased;
            grapAndPoke.Value.OnGrapReleasedAddBlock += OnGrapLogicReleased;

            RepeatSheet functionSheet = InstantiateRepeatSheet("FunctionSheet", grapAndPoke.Value);

            AddRepeatFunctionSheet(functionSheet, grapAndPoke.Key, grapAndPoke.Value);
            SetSheet(grapAndPoke.Key, functionSheet);
        }
        foreach (var grapAndPoke in InteractableManager.RepeatGrapAndPokeObjects)
        {
            UnityAction action = GetLogicEvent(BlockLogicType.Repeat, grapAndPoke.Value);
            grapAndPoke.Value.pokeInteractable.WhenSelect.AddListener(action);
            
            grapAndPoke.Value.OnGrapReleasedAddBlock -= OnGrapLogicReleased;
            grapAndPoke.Value.OnGrapReleasedAddBlock += OnGrapLogicReleased;
            
            RepeatSheet repeatSheet = InstantiateRepeatSheet("RepeatSheet", grapAndPoke.Value);

            AddRepeatFunctionSheet(repeatSheet, grapAndPoke.Key, grapAndPoke.Value);
            SetSheet(grapAndPoke.Key, repeatSheet);
        }
        List<Transform> sheetTransformList = new List<Transform>();
        foreach(var sheet in SubSheets.Values)
        {
            sheetTransformList.Add(sheet.transform);
        }
        RepeatFunctionSheetContainer.AddSheet(sheetTransformList);

        SetSheet(0, MainSheet);
        SetSheet(-1, ExSheet);

        MainSheet.gameObject.SetActive(true);
        foreach (var repeatSheet in SubSheets.Values)
            repeatSheet.gameObject.SetActive(true);
        ExSheet.gameObject.SetActive(false);

        ChoiceSheet(0);
    }

    private RepeatSheet InstantiateRepeatSheet(string sheetName, GrapAndPokeObject grapAndPoke)
    {
        GameObject sheetPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(sheetName);
        RepeatSheet repeatSheet = Instantiate(sheetPrefab, RepeatFunctionSheetContainer.transform).GetComponent<RepeatSheet>();
        
        FunctionBlockData functionBlockData = grapAndPoke.GetComponent<FunctionBlockData>();
        repeatSheet.Init(functionBlockData.functionLimit);
        repeatSheet.SheetName = functionBlockData.sheetName;
        
        return repeatSheet;
    }
    private void AddRepeatFunctionSheet(RepeatSheet sheet, int index, GrapAndPokeObject grapAndPoke)
    {
        SubSheets.Add(index, sheet);
        _sheetIndexDictionary.Add(grapAndPoke, index);
        sheet.InitRepeat(this, index);
        _repeatCountDictionary.Add(index, 1);
    }
    private void SetSheet(int index, SheetBase sheetBase)
    {
        _sheetDictionary.Add(index, sheetBase);
    }
    public void ChoiceSheet(int index)
    {
        _isExBlockLogic = (index == -1) ? true : false;
        _currentSheet = _sheetDictionary[index];
        foreach (var sheet in _sheetDictionary.Values)
        {
            sheet.SetBackgroundColor(sheet == _currentSheet ? selectSheetColor : unSelectSheetColor);
        }
    }

    #region 블록로직 오브젝트 생성
    private UnityAction GetLogicEvent(BlockLogicType type, GrapAndPokeObject grapAndPoke)
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
    private void ClickLogicButton(BlockLogicType type, GrapAndPokeObject grapAndPoke)
    {
        if (_currentSheet.SheetLimit != -1 && _currentSheet.SheetLimit <= _currentSheet.BlockCount)
            return;
        
        //함수 시트에 함수가 들어가지 못하게 막기용 기능
        if (_currentSheet is not global::MainSheet)
        {
            if (type == BlockLogicType.Repeat || type == BlockLogicType.Function)
                return;
        }
        
        GameObject tempPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>($"{type.ToString()}BlockLogic");
        AddOutlineComponent(tempPrefab);

        BlockLogicBase logicBase = Instantiate(tempPrefab).GetComponent<BlockLogicBase>();
        SetBlockLogicData(logicBase, grapAndPoke, type);
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
    
    private CancellationTokenSource cancelToken_RevertPosition;
    private void OnGrapLogicReleased(GrapAndPokeObject grapAndPoke, SheetBase sheet, int blockIndex)
    {
        if(sheet == null)
            return;
        if (blockIndex == -1)
            return;
        
        BlockLogicType type = grapAndPoke.Type;
        /*foreach (var typeGrapAndPokeObject in InteractableManager.GrapAndPokeObjects)
        {
            if (typeGrapAndPokeObject.Value == grapAndPoke)
            {
                type = typeGrapAndPokeObject.Key;
                break;
            }
        }*/

        if (type == BlockLogicType.Empty)
            return;
        
        if (sheet.SheetLimit != -1 && sheet.SheetLimit <= sheet.BlockCount)
            return;
        
        //함수 시트에 함수가 들어가지 못하게 막기용 기능
        if (sheet != MainSheet)
        {
            if (type == BlockLogicType.Repeat || type == BlockLogicType.Function)
            {
                Debug.LogWarning("메인시트가 아닙니다");
                return;
            }
        }
        
        GameObject tempPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>($"{type.ToString()}BlockLogic");
        AddOutlineComponent(tempPrefab);

        BlockLogicBase logicBase = Instantiate(tempPrefab).GetComponent<BlockLogicBase>();
        SetBlockLogicData(logicBase, grapAndPoke, type);
        
        if (logicBase != null)
        {
            AddBlockLogicByOrderIndex(sheet, logicBase, blockIndex);
            if (logicBase is RepeatBlockLogic rBlockLogic)
                if (_sheetIndexDictionary.TryGetValue(grapAndPoke, out int index))
                    rBlockLogic.SheetIndex = index;
            if (logicBase is FunctionBlockLogic fBlockLogic)
                if (_sheetIndexDictionary.TryGetValue(grapAndPoke, out int index))
                    fBlockLogic.SheetIndex = index;
        }
        if(sheet.SheetLimit != -1)
            sheet.BlockCount++;
        cancelToken_RevertPosition?.Cancel();
        cancelToken_RevertPosition = new CancellationTokenSource();
        sheet.UniTask_RevertPosition(cancelToken_RevertPosition.Token).Forget();
    }

    private void SetBlockLogicData(BlockLogicBase logicBase, GrapAndPokeObject grapAndPoke, BlockLogicType type)
    {
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
    }
    #endregion
    
    #region 연습장
    private void OnClick_OpenExercise()
    {
        MainSheet.gameObject.SetActive(false);
        foreach (var repeatSheet in SubSheets.Values)
            repeatSheet.gameObject.SetActive(false);
        ExSheet.gameObject.SetActive(true);
        ChoiceSheet(-1);
    }

    //연습장의 내용을 지우며 ExSheet비활성화
    public void OnClick_ExitExercise()
    {
        MainSheet.gameObject.SetActive(true);
        foreach (var repeatSheet in SubSheets.Values)
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
    }
    public void AddBlockLogicByOrderIndex(SheetBase sheet,BlockLogicBase blockLogic, int orderIndex)
    {
        sheet.InsertBlockLogicAtIndex(blockLogic, orderIndex);
    }

    public void ClearAllBlockLogic()
    {
        if(cancelToken_RunSheetBlock != null)
            cancelToken_RunSheetBlock.Cancel();
        
        ClearMainBlockLogic();
        ClearRepeatBlockLogic();
        ClearExBlockLogic();
        
        _isLogicRunning = false;
    }
    public void ClearMainBlockLogic()
    {
        MainSheet.ClearBlockLogic();
    }
    public void ClearRepeatBlockLogic()
    {
        foreach (var repeatFunctionSheet in SubSheets.Values)
            repeatFunctionSheet.ClearBlockLogic();
    }
    public void ClearExBlockLogic()
    {
        ExSheet.ClearBlockLogic();
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
        if(cancelToken_RunSheetBlock != null)
            cancelToken_RunSheetBlock.Cancel();
        StartResetWaiting().Forget();
        StageManager.ResetStage();

        _isLogicRunning = false;
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

        InteractableManager.StartButton.gameObject.SetActive(false);
        InteractableManager.ResetButton.gameObject.SetActive(true);
        _isLogicRunning = true;
        
        StageManager.Controller.SetAnimationState(AnimationState.IsIdle, true);
        StageManager.Controller.SetAnimationState(AnimationState.IsCarry, false);
        
        if(cancelToken_RunSheetBlock != null)
            cancelToken_RunSheetBlock.Cancel();
        cancelToken_RunSheetBlock = new CancellationTokenSource();
        ErrorType result = await RunBlockLogics(0, cancelToken_RunSheetBlock.Token);
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
    private CancellationTokenSource cancelToken_RunSheetBlock;
    async UniTask<ErrorType> RunBlockLogics(int sheetIndex, CancellationToken token)
    {
        StageManager.Controller.LogicHint.HideLogicHint();
        int tempLenght = 0;

        foreach (var blockLogic in _sheetDictionary[sheetIndex]._blockLogicBases)
        {
            ActiveHint(blockLogic);
            ErrorType result = await RunBlockLogic(blockLogic, token);
            
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

    async UniTask<ErrorType> RunBlockLogic(BlockLogicBase blockLogic, CancellationToken token)
    {
        Debug.Log($"CurrentLogicType : {blockLogic.GetType()}");
        if (blockLogic is FunctionBlockLogic functionBlockLogic)
        {
            return await RunBlockLogics(functionBlockLogic.SheetIndex, token);
        }
        if (blockLogic is RepeatBlockLogic repeatBlockLogic)
        {
            for (int i = 0; i < _repeatCountDictionary[repeatBlockLogic.SheetIndex]; i++)
            {
                ErrorType repeatResult = await RunBlockLogics(repeatBlockLogic.SheetIndex, token);
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
            await UniTask.NextFrame(token);
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
        foreach (var repeatSheet in SubSheets)
            Destroy(repeatSheet.Value.gameObject);
        
        SubSheets.Clear();
        ClearAllBlockLogic();
        
        _sheetDictionary.Clear();
        
        _repeatCountDictionary.Clear();
        MainSheet.gameObject.SetActive(false);
        ExSheet.gameObject.SetActive(false);
    }
}

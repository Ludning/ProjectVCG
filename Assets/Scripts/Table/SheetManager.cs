
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEditor.SceneManagement;
using Oculus.Interaction;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class SheetManager : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private bool _isLogicRunning = false;
    [SerializeField, ReadOnly]
    private bool _isExBlockLogic = false;
    
    [FormerlySerializedAs("Stage")]
    [Header("Manager")]
    [SerializeField] private StageManager StageManager;
    [SerializeField] private InteractableManager InteractableManager;
    [SerializeField] private NPCManager NPCManager;
    
    
    //MainSheet
    [Header("MainSheet")]
    [SerializeField] private MainSheet MainSheet;
    private List<BlockLogicBase> _blockLogicBases = new List<BlockLogicBase>();
    
    //SubSheet
    private Dictionary<int, RepeatSheet> RepeatSheets = new Dictionary<int, RepeatSheet>();
    private Dictionary<int, List<BlockLogicBase>> _repeatBlockLogicBasesDictionary = new Dictionary<int, List<BlockLogicBase>>();
    
    //ExSheet
    [Header("연습장")]
    [SerializeField] private ExSheet ExSheet;
    private List<BlockLogicBase> _exBlockLogicBases = new List<BlockLogicBase>();
    
    //함수블럭의 반복횟수를 저장
    private Dictionary<int, int> _repeatCountDictionary = new Dictionary<int, int>();


    private Dictionary<int, List<BlockLogicBase>> _sheetDictionary = new Dictionary<int, List<BlockLogicBase>>();
    private Dictionary<int, Transform> _sheetParentDictionary = new Dictionary<int, Transform>();
    
    //현재 활성화된 시트
    private List<BlockLogicBase> _currentSheet;
    private Transform _currentSheetParent;
    
    private int _sheetIndex;
    private Dictionary<InteractableUnityEventWrapper, int> _sheetIndexDictionary = new Dictionary<InteractableUnityEventWrapper, int>();

    //임시 코드
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            OnClick_SetLogic(GetButtonKeyValue(BlockLogicType.Start));
        
        if (Input.GetKeyDown(KeyCode.C))
            OnClick_SetLogic(GetButtonKeyValue(BlockLogicType.Cook));
        
        if (Input.GetKeyDown(KeyCode.M))
            OnClick_SetLogic(GetButtonKeyValue(BlockLogicType.Move));
        
        if (Input.GetKeyDown(KeyCode.P))
            OnClick_SetLogic(GetButtonKeyValue(BlockLogicType.PushItem));
        
        if (Input.GetKeyDown(KeyCode.L))
            OnClick_SetLogic(GetButtonKeyValue(BlockLogicType.RotateLeft));
        
        if (Input.GetKeyDown(KeyCode.R))
            OnClick_SetLogic(GetButtonKeyValue(BlockLogicType.RotateRight));
        
        if (Input.GetKeyDown(KeyCode.I))
            OnClick_SetLogic(GetButtonKeyValue(BlockLogicType.PopItem));
        
        if (Input.GetKeyDown(KeyCode.X))
            OnClick_SetLogic(GetButtonKeyValue(BlockLogicType.Clear));
        
        if (Input.GetKeyDown(KeyCode.Z))
            OnClick_SetLogic(GetButtonKeyValue(BlockLogicType.Reset));
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
            OnClick_SetLogic(GetButtonKeyValue(BlockLogicType.Repeat));
            
        if (Input.GetKeyDown(KeyCode.O)) 
        	OnClick_OpenExercise(); // VR 터치 대신 키입력으로 임시 코드
    }

    private KeyValuePair<BlockLogicType, InteractableUnityEventWrapper> GetButtonKeyValue(BlockLogicType type)
    {
        Debug.Log($"Input Type : {type}");
        return new KeyValuePair<BlockLogicType, InteractableUnityEventWrapper>(type, InteractableManager.InteractableButtons[type]);
    }


    //초기화 함수
    //상호작용 버튼을 생성하고 BlockLogic이 추가되는 이벤트를 연결해준다
    public void Init()
    {
        _sheetIndex = 1;

        InteractableManager.ExerciseButton.WhenSelect.AddListener(OnClick_OpenExercise);


        foreach (var interactableButton in InteractableManager.InteractableButtons)
        {
            //Debug.Log($"BlockLogicType : {interactableButton.Key}");
            interactableButton.Value.WhenSelect.AddListener(()=>OnClick_SetLogic(interactableButton));
            if(interactableButton.Key == BlockLogicType.Repeat || interactableButton.Key == BlockLogicType.Function)
            {
                GameObject sheetPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("RepeatSheet");
                RepeatSheet repeatSheet = Instantiate(sheetPrefab).GetComponent<RepeatSheet>();
                //TODO 위치 조정 스크립트도 작성해야함
                int index = GetNextSheetIndex();
                RepeatSheets.Add(index, repeatSheet);
                _repeatBlockLogicBasesDictionary.Add(index, new List<BlockLogicBase>());
                _sheetIndexDictionary.Add(interactableButton.Value, index);
                repeatSheet.GetComponent<RepeatSheet>().Init(this, ChoiceSheet, index);
                _repeatCountDictionary.Add(index, 1);
            }
        }

        SetSheet(0, _blockLogicBases, MainSheet.SheetParent);
        SetSheet(-1, _exBlockLogicBases, ExSheet.SheetParent);

        ChoiceSheet(0);
    }

    private void SetSheet(int index, List<BlockLogicBase> blockLogicList, Transform sheetParent)
    {
        _sheetDictionary.TryAdd(index, blockLogicList);
        _sheetParentDictionary.TryAdd(index, sheetParent);
    }
    private int GetNextSheetIndex()
    {
        return _sheetIndex++;
    }
    private void ChoiceSheet(int index)
    {
        _isExBlockLogic = (index == -1) ? true : false;
        _currentSheet = _sheetDictionary[index];
        _currentSheetParent = _sheetParentDictionary[index];
    }
    private void OnClick_SetLogic(KeyValuePair<BlockLogicType, InteractableUnityEventWrapper> keyValuePair)
    {
        //함수 시트에 함수가 들어가지 못하게 막기용 기능
        if(_currentSheet != _blockLogicBases || _currentSheet != _exBlockLogicBases)
        {
            if(keyValuePair.Key == BlockLogicType.Repeat || keyValuePair.Key == BlockLogicType.Function)
                return;
        } 
        switch (keyValuePair.Key)
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
        Debug.Log(keyValuePair.Key.ToString());
        GameObject tempPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>($"{keyValuePair.Key.ToString()}BlockLogic");

        AddOutlineComponent(tempPrefab);

        BlockLogicBase logicBase = Instantiate(tempPrefab).GetComponent<BlockLogicBase>();
        
        if (logicBase != null)
        {
            AddBlockLogic(logicBase);
            
            if(logicBase is RepeatBlockLogic rBlockLogic)
            {
                if(_sheetIndexDictionary.TryGetValue(keyValuePair.Value, out int index))
                {
                    rBlockLogic.SheetIndex = index;
                }
            }
            if (logicBase is FunctionBlockLogic fBlockLogic)
            {
                if (_sheetIndexDictionary.TryGetValue(keyValuePair.Value, out int index))
                {
                    fBlockLogic.SheetIndex = index;
                }
            }
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
        Debug.Log("AddBlockLogic");
        Debug.Log("AddBlockLogic");
    }
    public void ClearAllBlockLogic()
    {
        _sheetIndex = 1;
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
    //ExBlock 비우기
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
                    Debug.LogError($"ErrorType : {result} ErrorMessage : {DataManager.Instance.GetGameData<ErrorMessageData>(((int)result).ToString()).Context}");
                    NPCManager.GetMessage(result);
                    _isLogicRunning = false;
                }
                return result;
            }
            else
            {
                ActiveHint(MainSheet.SheetParent,tempLenght);
                NPCManager.GetMessage(result);
            }

            tempLenght++;

        }
        return ErrorType.NoError;
    }
    async UniTask<ErrorType> RunBlockLogic(BlockLogicBase blockLogic)
    {
        ErrorType result = blockLogic.IsExecutable(StageManager);
        //SimulatorManager.Instance.CheckAnswer(result);// To Do
        if (result != ErrorType.NoError)
            return result;

        if (blockLogic is RepeatBlockLogic repeatBlockLogic)
        {
            for (int i = 0; i < _repeatCountDictionary[repeatBlockLogic.SheetIndex]; i++)
            {
                return await RunBlockLogics(repeatBlockLogic.SheetIndex);
            }
        }
        while (true)
        {
            LogicState logicState = blockLogic.Execute(StageManager);
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
        
        foreach (var repeatBlockLogicBases in _repeatBlockLogicBasesDictionary)
        {
            foreach (var blockLogic in repeatBlockLogicBases.Value)
                Destroy(blockLogic.gameObject);
        }
        _repeatBlockLogicBasesDictionary.Clear();
        
        foreach (var repeatSheet in RepeatSheets)
        {
            Destroy(repeatSheet.Value.gameObject);
        }
        RepeatSheets.Clear();
    }
}

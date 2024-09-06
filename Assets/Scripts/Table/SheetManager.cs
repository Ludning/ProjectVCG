
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEditor.SceneManagement;
using Oculus.Interaction;
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

    //[Header("SubSheet")]
    //private List<Transform> repeatSheetParents = new List<Transform>();
    private Dictionary<int, Transform> repeatSheetParents = new Dictionary<int, Transform>();
    private Dictionary<int, List<BlockLogicBase>> _repeatBlockLogicBasesDictionary = new Dictionary<int, List<BlockLogicBase>>();

    [Header("연습장")]
    [SerializeField] private Transform ExSheetParent;
    private List<BlockLogicBase> _exBlockLogicBases = new List<BlockLogicBase>();


    private Dictionary<int, List<BlockLogicBase>> _sheetDictionary = new Dictionary<int, List<BlockLogicBase>>();
    private Dictionary<int, Transform> _sheetParentDictionary = new Dictionary<int, Transform>();
    
    private List<BlockLogicBase> _currentSheet;
    private Transform _currentSheetParent;



    private int sheetIndex;
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
        return new KeyValuePair<BlockLogicType, InteractableUnityEventWrapper>(type, InteractableManager.InteractableButtons[type]);
    }


    //초기화 함수
    //상호작용 버튼을 생성하고 BlockLogic이 추가되는 이벤트를 연결해준다
    public void Init()
    {
        sheetIndex = 1;

        InteractableManager.ExerciseButton.WhenSelect.AddListener(OnClick_OpenExercise);


        foreach (var interactableButton in InteractableManager.InteractableButtons)
        {
            //Debug.Log($"BlockLogicType : {interactableButton.Key}");
            interactableButton.Value.WhenSelect.AddListener(()=>OnClick_SetLogic(interactableButton));
            if(interactableButton.Key == BlockLogicType.Repeat || interactableButton.Key == BlockLogicType.Function)
            {
                GameObject sheetPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("RepeatSheets");
                GameObject repeatSheet = Instantiate(sheetPrefab);
                //TODO 위치 조정 스크립트도 작성해야함
                int index = GetNextSheetIndex();
                repeatSheetParents.Add(index, repeatSheet.transform);
                _repeatBlockLogicBasesDictionary.Add(index, new List<BlockLogicBase>());
                _sheetIndexDictionary.Add(interactableButton.Value, index);
            }
        }

        SetSheet(0, _blockLogicBases, SheetParent);
        SetSheet(-1, _exBlockLogicBases, ExSheetParent);

        _currentSheet = _sheetDictionary[0];
        _currentSheetParent = _sheetParentDictionary[0];
    }
    private void SetSheet(int index, List<BlockLogicBase> blockLogicList, Transform sheetParent)
    {
        _sheetDictionary.TryAdd(index, blockLogicList);
        _sheetParentDictionary.TryAdd(index, sheetParent);
    }

    private int GetNextSheetIndex()
    {
        return sheetIndex++;
    }
    /*private void OnClick_SetLogic(BlockLogicType type)
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
        Debug.Log(type.ToString());
        GameObject tempPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>($"{type.ToString()}BlockLogic");

        AddOutlineComponent(tempPrefab);

        BlockLogicBase logicBase = Instantiate(tempPrefab).GetComponent<BlockLogicBase>();

        if (logicBase != null)
        {
            AddBlockLogic(logicBase);
        }
    }*/
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
            //TODO
            //if (logicBase is FunctionBlockLogic rBlockLogic)
            //{
            //    if (_sheetIndexDictionary.TryGetValue(keyValuePair.Value, out int index))
            //    {
            //        rBlockLogic.SheetIndex = index;
            //    }
            //}
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
        _currentSheet = _sheetDictionary[-1];
        _currentSheetParent = _sheetParentDictionary[-1];
    }
    //연습장의 내용이 지워지는지에 따라 내용구현 달라짐
    //TODO
    private void OnClick_ExitExercise()
    {
        Stage.UIContainer.ExSheetPopup.gameObject.SetActive(false);
        ExSheetParent.gameObject.SetActive(false);
        _currentSheet = _sheetDictionary[0];
        _currentSheetParent = _sheetParentDictionary[0];
    }


    public void AddBlockLogic(BlockLogicBase blockLogic)
    {
        blockLogic.transform.SetParent(_currentSheetParent, false);
        _currentSheet.Add(blockLogic);
        Debug.Log("AddBlockLogic");
    }
    public void ClearAllBlockLogic()
    {
        sheetIndex = 1;
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
            //Destroy(repeatSheetParent.gameObject);
        }
        repeatSheetParents.Clear();
    }

    public void TestLogics()
    {
        OnClick_SetLogic(GetButtonKeyValue(BlockLogicType.Repeat));
        
        OnClick_SetLogic(GetButtonKeyValue(BlockLogicType.Repeat));
        OnClick_SetLogic(GetButtonKeyValue(BlockLogicType.Repeat));
    }
}

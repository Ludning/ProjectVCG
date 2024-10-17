using System.Collections.Generic;
using Oculus.Interaction;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableManager : MonoBehaviour
{
    private const string InteractableButtonName = "ButtonInteractable";
    private const string GrapAndPokeObjectName = "GrapAndPokeObject";
    private const string GrapAndPokeFunctionObjectName = "GrapAndPokeObject_Function";

    [SerializeField] private Transform ExerciseParent;
    [SerializeField] private Transform LogicButtonParent;
    [SerializeField] private Transform StartAndResetParent;
    [SerializeField] private Transform ClearParent;

    [HideInInspector]
    public InteractableUnityEventWrapper ExerciseButton;
    [HideInInspector]
    public InteractableUnityEventWrapper StartButton;
    [HideInInspector]
    public InteractableUnityEventWrapper ResetButton;
    [HideInInspector]
    public InteractableUnityEventWrapper ClearButton;
    
    public Dictionary<BlockLogicType, GrapAndPokeObject> GrapAndPokeObjects = new Dictionary<BlockLogicType, GrapAndPokeObject>();
    public Dictionary<int, GrapAndPokeObject> FunctionGrapAndPokeObjects = new Dictionary<int, GrapAndPokeObject>();
    public Dictionary<int, GrapAndPokeObject> RepeatGrapAndPokeObjects = new Dictionary<int, GrapAndPokeObject>();
    private int _sheetIndex;

    //public void Init(string showBlockString)
    public void Init(StageData stageData)
    {
        _sheetIndex = 1;
        
        GameObject buttonPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(GrapAndPokeObjectName);
        GameObject functionButtonPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(GrapAndPokeFunctionObjectName);

        InstantiateControlButton(BlockLogicType.Exercise);
        InstantiateControlButton(BlockLogicType.Start);
        InstantiateControlButton(BlockLogicType.Reset);
        InstantiateControlButton(BlockLogicType.Clear);

        List<string> blockList = new List<string>(stageData.ShowBlock.Split(", "));
        foreach (var blockIndex in blockList)
        {
            CodingBlockData data = DataManager.Instance.GetGameData<CodingBlockData>(blockIndex);
            InstantiateButton(buttonPrefab, LogicButtonParent, data.Type);
        }

        if (!string.IsNullOrWhiteSpace(stageData.Show_F_Block))
        {
            List<string> functionBlockList = new List<string>(stageData.Show_F_Block.Split(", "));
            foreach (var functionBlockIndex in functionBlockList)
            {
                FunctionData functionData = DataManager.Instance.GetGameData<FunctionData>(functionBlockIndex);
                InstantiateFunctionButton(functionButtonPrefab, LogicButtonParent, functionData.Type, functionData.Name, functionData.FunctionAmount);
            }
        }
        
        StartButton.gameObject.SetActive(true);
        ResetButton.gameObject.SetActive(false);
    }

    private void InstantiateControlButton(BlockLogicType type)
    {
        GameObject prefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(InteractableButtonName);;
        Transform parent = null;
        Material mat = null;
        
        switch (type)
        {
            case BlockLogicType.Start:
                mat = ResourceManager.Instance.LoadResourceWithCaching<Material>("StartMat");
                parent = StartAndResetParent;
                break;
            case BlockLogicType.Clear:
                mat = ResourceManager.Instance.LoadResourceWithCaching<Material>("ClearMat");
                parent = ClearParent;
                break;
            case BlockLogicType.Reset:
                mat = ResourceManager.Instance.LoadResourceWithCaching<Material>("ResetMat");
                parent = StartAndResetParent;
                break;
            case BlockLogicType.Exercise:
                mat = ResourceManager.Instance.LoadResourceWithCaching<Material>("MemoMat");
                parent = ExerciseParent;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        GameObject button = Instantiate(prefab, parent);
        button.GetComponent<CodeBlockMaterial>().SetMaterial(mat);
        InteractableUnityEventWrapper interactableUnityEventWrapper = button.GetComponent<InteractableUnityEventWrapper>();
        
        switch (type)
        {
            case BlockLogicType.Start:
                StartButton = interactableUnityEventWrapper;
                break;
            case BlockLogicType.Clear:
                ClearButton = interactableUnityEventWrapper;
                break;
            case BlockLogicType.Reset:
                ResetButton = interactableUnityEventWrapper;
                break;
            case BlockLogicType.Exercise:
                ExerciseButton = interactableUnityEventWrapper;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    private void InstantiateButton(GameObject prefab, Transform parent, BlockLogicType type)
    {
        //Debug.Log(type);
        GameObject button = Instantiate(prefab, parent);
        button.GetComponent<CodeBlockMaterial>().Init(type);
        GrapAndPokeObject grapAndPoke = button.GetComponent<GrapAndPokeObject>();
        GrapAndPokeObjects[type] = grapAndPoke;
    }
    private void InstantiateFunctionButton(GameObject prefab, Transform parent, BlockLogicType type, string functionName, int functionLimit)
    {
        GameObject button = Instantiate(prefab, parent);
        button.GetComponent<FunctionBlockData>().Init(functionName, functionLimit);
        GrapAndPokeObject grapAndPoke = button.GetComponent<GrapAndPokeObject>();

        switch (type)
        {
            case BlockLogicType.Function:
                FunctionGrapAndPokeObjects.Add(GetNextSheetIndex(), grapAndPoke);
                break;
            case BlockLogicType.Repeat:
                RepeatGrapAndPokeObjects.Add(GetNextSheetIndex(), grapAndPoke);
                break;
        }
    }
    public void Clear()
    {
        Destroy(ExerciseButton.gameObject);
        
        foreach (var interactableButton in GrapAndPokeObjects.Values)
            Destroy(interactableButton.gameObject);
        GrapAndPokeObjects.Clear();
        
        foreach (var interactableButton in FunctionGrapAndPokeObjects.Values)
            Destroy(interactableButton.gameObject);
        FunctionGrapAndPokeObjects.Clear();
        
        foreach (var interactableButton in RepeatGrapAndPokeObjects.Values)
            Destroy(interactableButton.gameObject);
        RepeatGrapAndPokeObjects.Clear();
    }
    
    private int GetNextSheetIndex()
    {
        return _sheetIndex++;
    }
}

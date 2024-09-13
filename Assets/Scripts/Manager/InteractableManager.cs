using System.Collections.Generic;
using Oculus.Interaction;
using System.Linq;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    private const string InteractableButtonName = "ButtonInteractable";

    [SerializeField] private Transform ExerciseParent;
    [SerializeField] private Transform LogicButtonParent;
    [SerializeField] private Transform StartParent;
    [SerializeField] private Transform ClearParent;

    [HideInInspector]
    public InteractableUnityEventWrapper ExerciseButton;
    public Dictionary<BlockLogicType, InteractableUnityEventWrapper> InteractableButtons = new Dictionary<BlockLogicType, InteractableUnityEventWrapper>();
    public Dictionary<int, InteractableUnityEventWrapper> FunctionInteractableButtons = new Dictionary<int, InteractableUnityEventWrapper>();
    public Dictionary<int, InteractableUnityEventWrapper> RepeatInteractableButtons = new Dictionary<int, InteractableUnityEventWrapper>();
    private int _sheetIndex;

    public void Init(string showBlockString)
    {
        _sheetIndex = 1;
        
        GameObject buttonPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(InteractableButtonName);

        InstantiateExerciseButton(buttonPrefab, ExerciseParent);

        List<string> blockList = new List<string>(showBlockString.Split(", "));
        foreach (var blockIndex in blockList)
        {
            CodingBlockData data = DataManager.Instance.GetGameData<CodingBlockData>(blockIndex);
            InstantiateButton(buttonPrefab, LogicButtonParent, data.Type);
        }
        
        InstantiateButton(buttonPrefab, StartParent, BlockLogicType.Start);
        InstantiateButton(buttonPrefab, StartParent, BlockLogicType.Reset);
        InstantiateButton(buttonPrefab, ClearParent, BlockLogicType.Clear);

        InteractableButtons[BlockLogicType.Reset].gameObject.SetActive(false);
    }

    private void InstantiateExerciseButton(GameObject prefab, Transform parent)
    {
        GameObject button = Instantiate(prefab, parent);
        Material mat = ResourceManager.Instance.LoadResourceWithCaching<Material>("MemoMat");
        button.GetComponent<LoadCodeBlockMaterial>().SetMaterial(mat);
        InteractableUnityEventWrapper interactableUnityEventWrapper = button.GetComponent<InteractableUnityEventWrapper>();

        ExerciseButton = interactableUnityEventWrapper;
    }
    private void InstantiateButton(GameObject prefab, Transform parent, BlockLogicType type)
    {
        //Debug.Log(type);
        GameObject button = Instantiate(prefab, parent);
        button.GetComponent<LoadCodeBlockMaterial>().Init(type);
        InteractableUnityEventWrapper interactableUnityEventWrapper = button.GetComponent<InteractableUnityEventWrapper>();

        switch (type)
        {
            case BlockLogicType.Function:
                FunctionInteractableButtons.Add(GetNextSheetIndex(), interactableUnityEventWrapper);
                break;
            case BlockLogicType.Repeat:
                RepeatInteractableButtons.Add(GetNextSheetIndex(), interactableUnityEventWrapper);
                break;
            default:
                InteractableButtons[type] = interactableUnityEventWrapper;
                break;
        }
    }
    public void Clear()
    {
        foreach (var interactableButton in InteractableButtons.Values)
            Destroy(interactableButton.gameObject);
        InteractableButtons.Clear();
        
        foreach (var interactableButton in FunctionInteractableButtons.Values)
            Destroy(interactableButton.gameObject);
        FunctionInteractableButtons.Clear();
        
        foreach (var interactableButton in RepeatInteractableButtons.Values)
            Destroy(interactableButton.gameObject);
        RepeatInteractableButtons.Clear();
    }
    
    private int GetNextSheetIndex()
    {
        return _sheetIndex++;
    }
}

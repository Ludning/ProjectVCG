using System.Collections.Generic;
using Oculus.Interaction;
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

    public void Init()
    {
        GameObject buttonPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(InteractableButtonName);

        InstantiateExerciseButton(buttonPrefab, ExerciseParent);

        InstantiateButton(buttonPrefab, LogicButtonParent, BlockLogicType.Cook);
        InstantiateButton(buttonPrefab, LogicButtonParent, BlockLogicType.Move);
        InstantiateButton(buttonPrefab, LogicButtonParent, BlockLogicType.RotateLeft);
        InstantiateButton(buttonPrefab, LogicButtonParent, BlockLogicType.RotateRight);
        InstantiateButton(buttonPrefab, LogicButtonParent, BlockLogicType.PushItem);
        InstantiateButton(buttonPrefab, LogicButtonParent, BlockLogicType.PopItem);
        
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
        GameObject button = Instantiate(prefab, parent);
        button.GetComponent<LoadCodeBlockMaterial>().Init(type);
        InteractableUnityEventWrapper interactableUnityEventWrapper = button.GetComponent<InteractableUnityEventWrapper>();
        
        InteractableButtons.Add(type, interactableUnityEventWrapper);
    }
    public void Clear()
    {
        foreach (var interactableButton in InteractableButtons.Values)
        {
            Destroy(interactableButton.gameObject);
        }
        InteractableButtons.Clear();
    }
}

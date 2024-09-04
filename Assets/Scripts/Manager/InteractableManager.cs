using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    private const string InteractableButtonName = "ButtonInteractable";

    [SerializeField] private Transform LogicButtonParent;
    [SerializeField] private Transform StartParent;
    [SerializeField] private Transform ClearParent;
    
    public Dictionary<BlockLogicType, InteractableUnityEventWrapper> InteractableButtons = new Dictionary<BlockLogicType, InteractableUnityEventWrapper>();

    public void Init()
    {
        GameObject buttonPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(InteractableButtonName);

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

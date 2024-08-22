using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Oculus.Interaction;
using UnityEngine;

public class SheetManager : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private StageManager Stage;
    [SerializeField] private InteractableManager InteractableManager;
    
    [Header("Parent")]
    [SerializeField] private Transform SheetParent;
    private List<BlockLogicBase> _blockLogicBases = new List<BlockLogicBase>();

    public void Init()
    {
        //InteractableButtons = new Dictionary<BlockLogicType, InteractableUnityEventWrapper>();
        
        /*foreach (var interactableButton in InteractableButtons)
        {
            interactableButton.Value.WhenSelect.AddListener(()=>OnClick_SetLogic(interactableButton.Key));
        }*/
        
        /*StartButton.WhenSelect.AddListener(OnClick_StartLogic);
        ClearButton.WhenSelect.AddListener(OnClick_ClearLogic);
        CookButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.Cook));
        MoveButton.WhenSelect.AddListener(()=> OnClick_SetLogic(BlockLogicType.Move));
        PushItemButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.PushItem));
        RotateLeftButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.RotateLeft));
        RotateRightButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.RotateRight));
        SetItemButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.SetItem));*/
    }
    private void OnClick_StartLogic()
    {
        Debug.Log("Start Logic");
        RunSheetBlock();
    }
    private void OnClick_ClearLogic()
    {
        ClearBlockLogic();
    }
    private void OnClick_SetLogic(BlockLogicType type)
    {
        switch (type)
        {
            case BlockLogicType.Start:
                OnClick_StartLogic();
                return;
            case BlockLogicType.Clear:
                OnClick_ClearLogic();
                return;
        }
        
        GameObject tempPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(type.ToString());
        BlockLogicBase logicBase = Instantiate(tempPrefab).GetComponent<BlockLogicBase>();
        
        if (logicBase != null)
        {
            logicBase.transform.SetParent(SheetParent);
            AddBlockLogic(logicBase);
        }
    }
    public void AddBlockLogic(BlockLogicBase blockLogic)
    {
        _blockLogicBases.Add(blockLogic);
    }
    public void ClearBlockLogic()
    {
        foreach (var blockLogicBase in _blockLogicBases)
        {
            Destroy(blockLogicBase.gameObject);
        }
        _blockLogicBases.Clear();
    }
    public void RunSheetBlock()
    {
        StartBlockLogics().Forget();
    }
    async UniTask<bool> StartBlockLogics()
    {
        foreach (var blockLogic in _blockLogicBases)
        {
            bool result = await BlockLogic(blockLogic);
            if (result == false)
            {
                Debug.Log("로직 실패!!");
                return false;
            }
        }
        return true;
    }
    async UniTask<bool> BlockLogic(BlockLogicBase blockLogic)
    {
        bool result = blockLogic.IsExecutable(Stage);
        if (result == false)
            return false;

        while (true)
        {
            bool isComplete = blockLogic.Execute(Stage);
            if (isComplete)
                return true;
            await UniTask.NextFrame();
        }
    }
}

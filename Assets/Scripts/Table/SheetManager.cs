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
        foreach (var interactableButton in InteractableManager.InteractableButtons)
        {
            interactableButton.Value.WhenSelect.AddListener(()=>OnClick_SetLogic(interactableButton.Key));
        }
    }
    private void OnClick_SetLogic(BlockLogicType type)
    {
        switch (type)
        {
            case BlockLogicType.Start:
                RunSheetBlock();
                return;
            case BlockLogicType.Clear:
                ClearBlockLogic();
                return;
        }
        GameObject tempPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>($"{type.ToString()}BlockLogic");
        BlockLogicBase logicBase = Instantiate(tempPrefab).GetComponent<BlockLogicBase>();
        
        if (logicBase != null)
        {
            AddBlockLogic(logicBase);
        }
    }
    public void AddBlockLogic(BlockLogicBase blockLogic)
    {
        blockLogic.transform.SetParent(SheetParent, false);
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

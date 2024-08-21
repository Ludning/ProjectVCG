using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Oculus.Interaction;
using UnityEngine;

public class SheetManager : MonoBehaviour
{
    [SerializeField] private List<InteractableUnityEventWrapper> InteractableButtons;
    
    [SerializeField] private StageManager Stage;
    private List<BlockLogicBase> _blockLogicBases = new List<BlockLogicBase>();

    public void OnClick_StartLogic()
    {
        Debug.Log("Start Logic");
        RunSheetBlock();
    }
    public void OnClick_ClearLogic()
    {
        //TODO
        //시각적 오브젝트 추가
        ClearBlockLogic();
    }
    public void OnClick_SetLogic(BlockLogicType type)
    {
        GameObject tempPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(type.ToString());
        BlockLogicBase logicBase = Instantiate(tempPrefab).GetComponent<BlockLogicBase>();
        

        if (logicBase != null)
        {
            logicBase.transform.position = new Vector3(100, 100, 100);
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

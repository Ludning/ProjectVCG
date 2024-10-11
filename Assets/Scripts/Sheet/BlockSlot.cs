using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSlot : MonoBehaviour
{
    private GameObject BlockLogic;
    
    public void SetBlockLogic(string blockLogicName)
    {
        GameObject logicDisplayPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(blockLogicName);
        BlockLogic = Instantiate(logicDisplayPrefab, transform, false);
    }

    public void ClearBlockLogic()
    {
        if (BlockLogic != null)
            Destroy(BlockLogic);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSlot : MonoBehaviour
{
    private GameObject BlockLogic;
    
    public void SetBlockLogic(BlockLogicBase blockLogic)
    {
        blockLogic.transform.SetParent(transform, false);
        blockLogic.transform.localPosition = Vector3.zero;
    }

    public void ClearBlockLogic()
    {
        if (BlockLogic != null)
            Destroy(BlockLogic);
    }
}

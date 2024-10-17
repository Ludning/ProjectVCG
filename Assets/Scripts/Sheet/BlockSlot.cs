using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSlot : MonoBehaviour
{
    [SerializeField] private Transform BlockParent;
    public void SetBlockLogic(BlockLogicBase blockLogic)
    {
        blockLogic.transform.SetParent(BlockParent, false);
        blockLogic.transform.localPosition = Vector3.zero;
        
    }
}

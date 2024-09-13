using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatFunctionSheetContainer : MonoBehaviour
{
    [SerializeField] Transform leftTransform;
    [SerializeField] Transform rightTransform;
    [SerializeField] private float additiveHeight;
    [SerializeField, ReadOnly] private float currentHeight;
    bool isLeft = true;
    public void AddSheet(List<Transform> sheetTransform)
    {
        if (sheetTransform == null || sheetTransform.Count == 0)
            return;
        currentHeight = 0;
        if (sheetTransform.Count == 1)
        {
            sheetTransform[0].position = Vector3.up * additiveHeight;
        }
        else
        {
            foreach(var sheet in sheetTransform)
            {
                sheet.position = (isLeft) ? leftTransform.position + Vector3.up * currentHeight : rightTransform.position + Vector3.up * currentHeight;
                if(isLeft == false)
                    currentHeight += additiveHeight;

                isLeft = !isLeft;
            }
        }
    }
}

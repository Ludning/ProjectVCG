using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatFunctionSheetContainer : MonoBehaviour
{
    [SerializeField] Transform sheetPositionRig;
    [SerializeField] private float additiveHeight = 0.11f;
    [SerializeField, ReadOnly] private float currentHeight;
    public void AddSheet(List<Transform> sheetTransformList)
    {
        if (sheetTransformList == null || sheetTransformList.Count == 0)
            return;
        currentHeight = 0;

        foreach (var sheetTransform in sheetTransformList)
        {
            sheetTransform.position = sheetPositionRig.position + Vector3.up * currentHeight;
            sheetTransform.rotation = sheetPositionRig.rotation;
            currentHeight += additiveHeight;
        }
        
        /*if (sheetTransform.Count == 1)
        {
            sheetTransform[0].localPosition = Vector3.up * additiveHeight;
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
        }*/
    }
}

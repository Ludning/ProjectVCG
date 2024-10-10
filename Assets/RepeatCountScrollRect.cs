using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RepeatCountScrollRect : ScrollRect
{
    public override void OnDrag(PointerEventData eventData)
    {
        if (eventData.selectedObject == verticalScrollbar.gameObject)
        {
            base.OnDrag(eventData);
        }
        return;
    }
}


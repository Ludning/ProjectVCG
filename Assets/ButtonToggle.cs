using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private ButtonToggleGroup ToggleGroup;
    private event Action<ButtonToggle> OnSelectEvent;

    private Image ButtonImage;

    public bool IsSelect = false;

    private void Awake()
    {
        ButtonImage = GetComponent<Image>();
        ToggleGroup = transform.parent.GetComponent<ButtonToggleGroup>();
        ToggleGroup.AddToggleToList(this);
    }

    public void RegistSelectEvent(Action<ButtonToggle> action)
    {
        OnSelectEvent = action;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        OnSelectEvent?.Invoke(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        OnHighlightChangeColor();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
        OnUnhighlightChangeColor();
    }
    
    public void OnSelectChangeColor()
    {
        ButtonImage.color = ToggleGroup.SelectedColor;
    }
    public void OnUnselectChangeColor()
    {
        ButtonImage.color = ToggleGroup.UnselectedColor;
    }
    public void OnHighlightChangeColor()
    {
        ButtonImage.color = ToggleGroup.HighlightColor;
    }
    public void OnUnhighlightChangeColor()
    {
        ButtonImage.color = IsSelect ? ToggleGroup.SelectedColor : ToggleGroup.UnselectedColor;
    }
}

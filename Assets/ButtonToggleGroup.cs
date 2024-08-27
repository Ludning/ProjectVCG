using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonToggleGroup : MonoBehaviour
{
    [SerializeField] private UnityEvent<int> OnChangeToggleEvent;
    
    private List<ButtonToggle> ButtonToggleList = new List<ButtonToggle>();
    
    public Color SelectedColor;
    public Color UnselectedColor;
    public Color HighlightColor;

    public void AddToggleToList(ButtonToggle buttonToggle)
    {
        if (buttonToggle == null)
            return;
        ButtonToggleList.Add(buttonToggle);
        buttonToggle.RegistSelectEvent(OnChangeSelectButtonToggle);
    }

    public void ClearToggle()
    {
        foreach (var buttonToggle in ButtonToggleList)
        {
            Destroy(buttonToggle.gameObject);
        }
    }

    public void OnChangeSelectButtonToggle(ButtonToggle sender)
    {
        foreach (var toggle in ButtonToggleList)
        {
            if (toggle == sender)
            {
                toggle.OnSelectChangeColor();
                toggle.IsSelect = true;
                OnChangeToggleEvent.Invoke(toggle.value);
            }
            else
            {
                toggle.OnUnselectChangeColor();
                toggle.IsSelect = false;
            }
        }
    }
}

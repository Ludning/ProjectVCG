using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private ButtonToggleGroup ToggleGroup;
    private event Action<ButtonToggle> OnSelectEvent;

    [SerializeField] private Image ButtonImage;
    [SerializeField] private Text ButtonText;

    public bool IsSelect = false;

    [Header("선택되었을 시 넘겨줄 값")] public int value;

    public void SetUIText(string context)
    {
        ButtonText.text = context;
    }
    private void Awake()
    {
        ToggleGroup = transform.parent.GetComponent<ButtonToggleGroup>();
        ToggleGroup.AddToggleToList(this);
    }

    public void RegistSelectEvent(Action<ButtonToggle> action)
    {
        OnSelectEvent += action;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelectEvent?.Invoke(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHighlightChangeColor();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
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

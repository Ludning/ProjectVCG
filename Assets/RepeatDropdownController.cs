using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepeatDropdownController : MonoBehaviour
{
    public static List<RepeatDropdownController> RepeatDropdownControllers = new List<RepeatDropdownController>();
    
    private bool _isActive = false;
    [SerializeField] private GameObject Dropdown;
    [SerializeField] private Button DropdownButton;
    private const int _delay = 500;

    private void Awake()
    {
        RepeatDropdownControllers.Add(this);
    }
    private void OnDestroy()
    {
        RepeatDropdownControllers.Remove(this);
    }

    public void OnClickOnOffDropdown()
    {
        Debug.Log("OnClickOnOffDropdown");
        foreach (var RepeatDropdownController in RepeatDropdownControllers)
        {
            if (RepeatDropdownController != this)
            {
                RepeatDropdownController.Dropdown.SetActive(false);
                RepeatDropdownController._isActive = false;
            }
        }

        if (_isActive)
        {
            Dropdown.SetActive(false);
            _isActive = false;
        }
        else
        {
            Dropdown.SetActive(true);
            _isActive = true;
        }
        EnableButtonAfterDelay().Forget();
    }
    
    private async UniTaskVoid EnableButtonAfterDelay()
    {
        DropdownButton.interactable = false;
        await UniTask.Delay(_delay);
        DropdownButton.interactable = true;
    }
}

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepeatDropdownController : MonoBehaviour
{
    private bool _isActive = false;
    [SerializeField] private GameObject Dropdown;
    [SerializeField] private Button DropdownButton;
    private const int _delay = 500;
    
    public void OnClickOnOffDropdown()
    {
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

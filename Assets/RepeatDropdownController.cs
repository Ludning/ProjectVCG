using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatDropdownController : MonoBehaviour
{
    private bool _isActive = false;
    [SerializeField] private GameObject Dropdown;
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
    }
}

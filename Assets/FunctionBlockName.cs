using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class FunctionBlockName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    public string sheetName;
    
    public void Init(string sheetName)
    {
        this.sheetName = sheetName;
        textComponent.text = sheetName;
    }
}

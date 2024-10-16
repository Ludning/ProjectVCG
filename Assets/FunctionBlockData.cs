using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class FunctionBlockData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PokeTextComponent;
    [SerializeField] private TextMeshProUGUI GrapTextComponent;
    public int functionLimit;
    public string sheetName;
    
    public void Init(string sheetName, int functionLimit)
    {
        this.functionLimit = functionLimit;
        this.sheetName = sheetName;
        PokeTextComponent.text = sheetName;
        GrapTextComponent.text = sheetName;
    }
}

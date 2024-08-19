using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetInstaller : MonoBehaviour
{
    [SerializeField] private int _sheetCount;

    [SerializeField] private GameObject _sheetSlotPrefab;

    private void Awake()
    {
        Init(_sheetCount);
    }

    public void Init(int sheetCount)
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetBase : MonoBehaviour
{
    public Transform SheetParent;
    [ReadOnly] public SheetManager SheetManager;
    public int SheetLimit { get; set; }
    public int BlockCount { get; set; }
}

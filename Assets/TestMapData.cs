using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMapData : MonoBehaviour
{
    void Start()
    {
        MapData mapData = DataManager.Instance.GetMapData();
        Debug.Log(mapData);
    }
}

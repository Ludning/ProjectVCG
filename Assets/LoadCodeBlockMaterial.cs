using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCodeBlockMaterial : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    public void Init(BlockLogicType type)
    {
        Material tempMat = ResourceManager.Instance.LoadResourceWithCaching<Material>($"{type.ToString()}Mat");
        if(tempMat == null)
            tempMat = ResourceManager.Instance.LoadResourceWithCaching<Material>("TempLogicMat");
        _meshRenderer.materials = new[] { tempMat };
    }
}

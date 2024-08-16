using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCodeBlockMaterial : MonoBehaviour
{
    [SerializeField] private BlockLogicType _type;
    [SerializeField] private MeshRenderer _meshRenderer;
    private void Awake()
    {
        Material tempMat = null;
        
        switch (_type)
        {
            case BlockLogicType.Cook:
                tempMat = DataManager.Instance.Cook;
                break;
            case BlockLogicType.Move:
                tempMat = DataManager.Instance.Move;
                break;
            case BlockLogicType.PushItem:
                tempMat = DataManager.Instance.Push;
                break;
            case BlockLogicType.RotateLeft:
                tempMat = DataManager.Instance.Turn_Left;
                break;
            case BlockLogicType.RotateRight:
                tempMat = DataManager.Instance.Turn_Right;
                break;
            case BlockLogicType.SetItem:
                tempMat = DataManager.Instance.Pop;
                break;
            case BlockLogicType.Start:
                tempMat = DataManager.Instance.Start;
                break;
            case BlockLogicType.Clear:
                tempMat = DataManager.Instance.Clear;
                break;
            default:
                tempMat = DataManager.Instance.NullMat;
                break;
        }

        _meshRenderer.materials = new[] { tempMat };
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMakerManager : MonoBehaviour
{
    public GameObject[] Objects;
    public float _gridSize;
    public float _rotateAmount;

    private bool _gridOn;
    private GameObject _pendingObject;           // 오브젝트 보류
    private Vector3 _pos;                       // 위치 저장하기 위해
    private RaycastHit _hit;
    
    [SerializeField] private Toggle GridToggle;
    [SerializeField] private LayerMask LayerMask;

    void Update()
    {
        if(_pendingObject != null)
        {
            if (_gridOn)
            {
                _pendingObject.transform.position = new Vector3(
                    RoundToNearestGrid(_pos.x),
                    RoundToNearestGrid(_pos.y),
                    RoundToNearestGrid(_pos.z)
                    );
            }
            else
            {
                _pendingObject.transform.position = _pos; // null 이면 마우스 위치 값 적용

            }
            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateObject();
            }
        }
    }
    public void PlaceObject()
    {
        _pendingObject = null;
    }

    public void RotateObject()
    {
        _pendingObject.transform.Rotate(Vector3.up, _rotateAmount);
    }

    // 물리 관련
    private void FixedUpdate() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스 위치 반환

        if (Physics.Raycast(ray, out _hit, 1000, LayerMask))
        {
            _pos = _hit.point;
        }
    }

    // 선택
    public void SelectObject (int idx)
    {
        _pendingObject = Instantiate(Objects[idx], _pos, transform.rotation);
    }

    public void ToggleGrid()
    {
        if (GridToggle.isOn)
        {
            _gridOn = true;
        } else
        {
            _gridOn = false;
        }
    }

    private float RoundToNearestGrid(float pos)
    {
        float xDiff = pos % _gridSize;
        pos -= xDiff;
        if(xDiff > (_gridSize / 2))
        {
            pos += _gridSize;
        }
        return pos;
    }
}



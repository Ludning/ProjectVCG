using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMakerManager : MonoBehaviour
{
    public GameObject[] Objects;                   // 맵에 배치 할 프리팹들
    public GameObject PendingObject;             // 마우스를 따라 다니며 배치 대기 중인 오브젝트

    private bool _gridOn = true;                   // 스냅 기능 사용 여부
    private Vector3 _pos;                          // 마우스 위치 값 저장
    private RaycastHit _hit;                       // 히트 정보 저장

    [SerializeField] private float RotateAmount;   // R 키입력 시 오브젝트가 회전하는 각도
    [SerializeField] private float GridSize = 1f;       // 그리드 크기 (1 권장)
    [SerializeField] private Toggle GridToggle;    // 토글 UI 
    [SerializeField] private LayerMask LayerMask;  // 레이캐스트에 사용할 레이어 마스크

    void Update()
    {
        // 배치 대기 중인 오브젝트가 있을 때 (캔버스에서 1,2,3 중 하나 버튼 누르면 됨)
        if (PendingObject != null)
        {
            if (_gridOn)
            {
                PendingObject.transform.position = new Vector3(
                    RoundToNearestGrid(_pos.x),
                    RoundToNearestGrid(_pos.y),
                    RoundToNearestGrid(_pos.z)
                );
            }
            else
            {
                PendingObject.transform.position = _pos; // null 이면 마우스 위치 값 적용
            }
            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateObject();
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectObject(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectObject(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SelectObject(2);
            }
        }
    }

    public void PlaceObject()
    {
        PendingObject = null;
    }

    public void RotateObject()
    {
        PendingObject.transform.Rotate(Vector3.up, RotateAmount);
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
    public void SelectObject(int idx)
    {
        PendingObject = Instantiate(Objects[idx], _pos, transform.rotation);
    }

    public void ToggleGrid()
    {
        _gridOn = GridToggle.isOn;
    }

    private float RoundToNearestGrid(float pos)
    {
        return Mathf.Round(pos / GridSize) * GridSize;
    }
}
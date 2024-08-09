using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapMakerManager : MonoBehaviour
{
    public GameObject[] Objects;                   // 맵에 배치 할 프리팹들
    public GameObject PendingObject;             // 마우스를 따라 다니며 배치 대기 중인 오브젝트

    private bool _gridOn = true;                   // 스냅 기능 사용 여부
    private Vector3 _pos;                          // 마우스 위치 값 저장
    private RaycastHit _hit;                       // 히트 정보 저장

    [SerializeField] private GameObject GroundObj;
    [SerializeField] private Button CloneBtn;
    [SerializeField] private GameObject RootObj;
    [SerializeField] private float RotateAmount;   // R 키입력 시 오브젝트가 회전하는 각도
    [SerializeField] private float GridSize = 1f;       // 그리드 크기 (1 권장)
    [SerializeField] private Toggle GridToggle;    // 토글 UI 
    [SerializeField] private LayerMask LayerMask;  // 레이캐스트에 사용할 레이어 마스크

    private void Start()
    {
        if (Objects != null && CloneBtn != null)
        {
            // CloneBtn이 씬에 배치된 오브젝트라고 가정합니다.
            int idx = 0;
            foreach (GameObject objPrefab in Objects)
            {
                // 버튼 프리팹 생성
                GameObject newButton = Instantiate(CloneBtn.gameObject, RootObj.transform);
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = (idx + 1).ToString();
                // 
                Button newButtonGameObject = newButton.GetComponent<Button>();

                int capturedIdx = idx; // 클로저 문제를 피하기 위해 지역 변수로 캡처

                newButtonGameObject.onClick.AddListener(() => SelectObject(capturedIdx));
                idx++;

                // 프리팹을 인스턴스화
                GameObject objInstance = Instantiate(objPrefab, newButton.transform);
                objInstance.transform.localScale = new Vector3(100, 100, 100);

                // 인스턴스화된 오브젝트를 새로 생성된 버튼의 자식으로 설정

                // 필요에 따라 추가적인 작업 수행
                objInstance.name = "Child_" + objPrefab.name; // 이름 변경 등
            }
        }
    }
    void Update()
    {
        // 배치 대기 중인 오브젝트가 있을 때 (캔버스에서 1,2,3 중 하나 버튼 누르면 됨)
        if (PendingObject != null)
        {
            if (_gridOn)
            {
                PendingObject.transform.position = new Vector3(
                    RoundToNearestGrid(_pos.x),
                    .5f,
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
        }
        else
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
        Vector3 currentPosition = PendingObject.transform.localPosition;
        currentPosition.y = 0.5f;
        PendingObject.transform.localPosition = currentPosition;
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
        PendingObject = Instantiate(Objects[idx], _pos, transform.rotation,GroundObj.transform);
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
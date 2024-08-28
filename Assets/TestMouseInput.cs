using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMouseInput : MonoBehaviour
{
    // Ray의 길이를 설정
    public float rayLength = 100f;
    // Ray의 색상을 설정
    public Color rayColor = Color.red;
    
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
    }


    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Raycast를 사용하여 충돌 검사
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            // 충돌한 오브젝트가 있을 때
            Debug.Log("마우스가 가리키고 있는 오브젝트: " + hit.collider.gameObject.name);

            // 클릭한 위치에 레이를 그려 시각화
            Debug.DrawRay(ray.origin, ray.direction * rayLength, rayColor, 2f);
        }
        else
        {
            // 충돌한 오브젝트가 없을 때
            Debug.Log("마우스가 가리키고 있는 위치에 오브젝트가 없습니다.");
        }
    }
}

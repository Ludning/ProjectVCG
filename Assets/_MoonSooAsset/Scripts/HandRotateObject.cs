using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRotateObject : MonoBehaviour
{
    [SerializeField] private OVRHand leftHand;
    [SerializeField] private OVRHand rightHand;
    
    [SerializeField] private Transform objectToRotate; // 회전시킬 오브젝트
    [SerializeField] private Material cubeMat;
    [SerializeField] private float rotationSpeed = 100f; // 회전 속도
    
    private Vector3 lastHandPosition;
    private bool isRotating = false;

    private bool IsRotating
    {
        get => isRotating;
        set
        {
            cubeMat.color = (value) ? Color.red : Color.white;
            isRotating = value;
        }
    }
    
    void Update()
    {
        // 오른손의 상태 확인 (왼손을 사용할 경우 rightHand를 leftHand로 바꾸세요)
        bool isIndexFingerExtended = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index) && 
                                     !rightHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) &&
                                     !rightHand.GetFingerIsPinching(OVRHand.HandFinger.Ring) &&
                                     !rightHand.GetFingerIsPinching(OVRHand.HandFinger.Pinky) &&
                                     !rightHand.GetFingerIsPinching(OVRHand.HandFinger.Thumb);

        if (isIndexFingerExtended)
        {
            // 회전 시작
            if (!IsRotating)
            {
                IsRotating = true;
                lastHandPosition = rightHand.transform.position;
            }
            else
            {
                // 손의 현재 위치
                Vector3 currentHandPosition = rightHand.transform.position;

                // 손의 이동 거리 계산
                float deltaX = currentHandPosition.x - lastHandPosition.x;

                // 오브젝트 회전
                objectToRotate.Rotate(Vector3.up, deltaX * rotationSpeed * Time.deltaTime);

                // 이전 위치 업데이트
                lastHandPosition = currentHandPosition;
            }
        }
        else
        {
            IsRotating = false;
        }
    }

    private bool CheakHandGesture()
    {
        bool isPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        return isPinching;
    }

    private void RotateObject()
    {
        
    }
}

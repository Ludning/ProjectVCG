using UnityEngine;

public class HandRotateObject : MonoBehaviour
{
    [SerializeField] private OVRHand leftHand;
    [SerializeField] private OVRHand rightHand;
    
    [SerializeField] private Transform objectToRotate; // 회전시킬 오브젝트
    [SerializeField] private Material cubeMat;
    [SerializeField] private float rotationSpeed = 100.0f; // 회전 속도
    
    private Vector3 _lastHandPosition;
    
    private bool _isRotating = false;
    private bool IsRotating
    {
        get => _isRotating;
        set
        {
            cubeMat.color = (value) ? Color.red : Color.white;
            _isRotating = value;
        }
    }
    
    private void Update()
    {
        // 오른손의 상태 확인 (왼손을 사용할 경우 rightHand를 leftHand로 바꾸세요)
        OVRHand ovrHand = rightHand;

        if (GetIndexFingerIsPinching(ovrHand))
        {
            // 회전 시작
            if (!IsRotating)
            {
                IsRotating = true;
                _lastHandPosition = ovrHand.transform.position;
            }
            else
            {
                // 손의 현재 위치
                Vector3 currentHandPosition = ovrHand.transform.position;

                // 손의 이동 거리 계산
                float deltaX = currentHandPosition.x - _lastHandPosition.x;

                // 오브젝트 회전
                objectToRotate.Rotate(Vector3.up, deltaX * rotationSpeed * Time.deltaTime);

                // 이전 위치 업데이트
                _lastHandPosition = currentHandPosition;
            }
        }
        else
        {
            IsRotating = false;
        }
    }

    private bool GetIndexFingerIsPinching(OVRHand ovrHand) =>
    (
        ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Index) && 
        !ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) &&
        !ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Ring) &&
        !ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Pinky) &&
        !ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Thumb)
    );

    private bool CheckHandGesture(OVRHand ovrHand) => ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

    private void RotateObject() { }
}

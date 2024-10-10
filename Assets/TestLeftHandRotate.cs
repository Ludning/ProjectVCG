using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLeftHandRotate : MonoBehaviour
{
    [SerializeField] private TableManager TableManager;
    [SerializeField] private OVRHand leftHand;

    private bool isRotateMode = false;

    void Update()
    {
        Debug.Log($"leftHand.IsTracked : {leftHand.IsTracked}");
        
        if (!leftHand.IsTracked && isRotateMode == true)
        {
            OnRotateEnd();
        }
        
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            if (isRotateMode == true)
            {
                OnRotateEnd();
            }
            else
            {
                OnRotateStart();
            }
        }

        if (isRotateMode == false)
            return;
        
        // 왼손의 회전값 가져오기 (퀘이터니언 값)
        Quaternion leftHandRotation = leftHand.transform.rotation;
        TableManager.RotateTable(leftHandRotation);
    }

    private void OnRotateStart()
    {
        isRotateMode = true;
        Debug.Log("OnRotateStart");
    }
    private void OnRotateEnd()
    {
        isRotateMode = false;
        Debug.Log("OnRotateEnd");
    }
    
}

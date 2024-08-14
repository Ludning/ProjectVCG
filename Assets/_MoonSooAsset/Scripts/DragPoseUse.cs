using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using TMPro;
using UnityEngine;

public class DragPoseUse : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IHmd))]
        private UnityEngine.Object _hmd;
        private IHmd Hmd { get; set; }

        [SerializeField] private ActiveStateSelector _pose;

        [SerializeField] private Transform objectTransform;
        [SerializeField] private Material _cubeMat;
        
        private Pose hmdPose;
        private Vector3 lastHandPosition;
        private float rotationSpeed = 100f;
        private bool isRotating = false;
        private bool IsRotating
        {
            get => isRotating;
            set
            {
                _cubeMat.color = (value) ? Color.red : Color.white;
                isRotating = value;
            }
        }
        protected virtual void Awake()
        {
            Hmd = _hmd as IHmd;
        }
        protected virtual void Start()
        {
            this.AssertField(Hmd, nameof(Hmd));
            //this.AssertField(_poseActiveVisualPrefab, nameof(_poseActiveVisualPrefab));

            _pose.WhenSelected += StartDragObjectRotation;
            _pose.WhenUnselected += EndDragObjectRotation;
            
        }
        void Update()
        {
            if (IsRotating)
            {
                // 손의 현재 위치
                Vector3 currentHandPosition = hmdPose.position;
                
                // 손의 이동 거리 계산
                float deltaX = currentHandPosition.x - lastHandPosition.x;
                
                // 오브젝트 회전
                objectTransform.Rotate(Vector3.up, deltaX * rotationSpeed * Time.deltaTime);
                
                // 이전 위치 업데이트
                lastHandPosition = currentHandPosition;
            }
        }
        private void StartDragObjectRotation()
        {
            if (!Hmd.TryGetRootPose(out Pose hmdPose))
                return;
            this.hmdPose = hmdPose;
            lastHandPosition = hmdPose.position;
            IsRotating = true;
        }
        private void EndDragObjectRotation()
        {
            IsRotating = false;
        }
        /*private void ShowVisuals()
        {
            if (!Hmd.TryGetRootPose(out Pose hmdPose))
                return;

            Vector3 spawnSpot = hmdPose.position + hmdPose.forward;
            _poseActiveVisuals[poseNumber].transform.position = spawnSpot;
            _poseActiveVisuals[poseNumber].transform.LookAt(2 * _poseActiveVisuals[poseNumber].transform.position - hmdPose.position);

            var hands = _poses[poseNumber].GetComponents<HandRef>();
            Vector3 visualsPos = Vector3.zero;
            foreach (var hand in hands)
            {
                hand.GetRootPose(out Pose wristPose);
                Vector3 forward = hand.Handedness == Handedness.Left ? wristPose.right : -wristPose.right;
                visualsPos += wristPose.position + forward * .15f + Vector3.up * .02f;
            }
            _poseActiveVisuals[poseNumber].transform.position = visualsPos / hands.Length;
            _poseActiveVisuals[poseNumber].gameObject.SetActive(true);
        }

        private void HideVisuals()
        {
            _poseActiveVisuals[poseNumber].gameObject.SetActive(false);
        }*/
    }
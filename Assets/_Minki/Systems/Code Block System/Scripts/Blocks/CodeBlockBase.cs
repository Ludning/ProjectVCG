using System;
using UnityEngine;

// 코드 블록 시스템
namespace CodeBlockSystem
{
    // 코드 블록의 종류를 정의하는 열거형
    public enum CodeBlockType
    {
        WalkForward, // 자신의 앞 타일로 1칸 이동합니다.
        TurnLeft, // 자신의 타일에서 왼쪽으로 90도 회전합니다.
        TurnRight, // 자신의 타일에서 오른쪽으로 90도 회전합니다.
        Lift, // 자신의 앞 타일에 놓여 있는 아이템을 들어올립니다.
        Drop, // 들고 있는 아이템을 자신의 앞 타일에 내려놓습니다.
    }
    
    // 코드 블록 객체의 최상위 클래스
    public abstract class CodeBlockBase : MonoBehaviour
    {
        // 변수
        [SerializeField] private CodeBlockType type; // 종류
        private event Action<CodeBlockBase> OnTouch; // 터치 이벤트

        // 함수
        public abstract void Execute(); // 각 코드 블록의 고유 행동 코드를 실행합니다.

        // 터치 이벤트를 등록, 해제하는 함수
        public void AddTouchListener(Action<CodeBlockBase> action) { OnTouch += action; }
        public void RemoveTouchListener(Action<CodeBlockBase> action) { OnTouch -= action; }
        
        // 유니티 충돌 이벤트
        // private void OnCollisionEnter(Collision other) // = 터치 이벤트
        // {
        //     if (other.gameObject.CompareTag("Hand")) // TODO: VR 핸드 트래킹과의 접촉을 지정해야 한다.
        //     {
        //         OnTouch?.Invoke(this); // 터치 이벤트를 호출합니다.
        //     }
        // }

        public void InvokeTouchEvent()
        {
            OnTouch?.Invoke(this);
        }
    }
}

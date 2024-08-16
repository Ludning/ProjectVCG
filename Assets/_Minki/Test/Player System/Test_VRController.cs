using UnityEngine;

namespace PlayerSystem
{
    public class VRController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // 마우스의 왼쪽 클릭 시,
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // 메인 카메라를 기준으로, 마우스의 위치에 Ray을 쏜다.
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    // 만약 충돌한 오브젝트가 코드 블록이라면,
                    if (hitInfo.collider.TryGetComponent(out CodeBlockSystem.BaseCodeBlock codeBlock))
                    {
                        // 코드 블록의 터치 이벤트를 호출합니다.
                        codeBlock.InvokeTouchEvent();
                    }
                }
            }
        }
    }
}

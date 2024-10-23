using UnityEngine;

namespace JH
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5.0f;

        private void Update()
        {
            float h = Input.GetAxis("Horizontal"); // A, D 키 -> X 축 이동
            float v = Input.GetAxis("Vertical");   // W, S 키 -> Z 축 이동

            // X 축과 Z 축으로 이동하도록 설정하고, Y 축은 0으로 고정
            Vector3 move = new Vector3(h, 0, v) * (moveSpeed * Time.deltaTime);

            // Y 축은 고정된 상태로 X와 Z만 이동
            transform.Translate(move, Space.World);
        }
    }
}


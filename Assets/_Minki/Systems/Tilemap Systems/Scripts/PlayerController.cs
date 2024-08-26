using System.Collections;
using TilemapSystem;
using UnityEngine;

namespace PlayerSystem
{
    // 플레이어가 바라보는 방향을 정의하는 열거형(enum)
    public enum PlayerDirection
    {
        Forward, Back, Left, Right // 상, 하, 좌, 우
    }
    
    public class PlayerController : MonoBehaviour
    {
        #region 변수(Field)

        [SerializeField] private float moveSpeed = 1.0f;

        private TilemapManager _tilemapManager; // 타일맵(Tilemap)
        private PlayerDirection _currentDirection; // 플레이어의 방향
        private Coroutine _moveCoroutine; // 플레이어의 이동과 관련한 코루틴; 중복 호출의 제어 등을 위한 변수
        
        #endregion 변수(Field)

        #region 함수(Method)
        
        // Awake()
        private void Awake()
        {
            // 타일맵은 타일맵 매니저가 참조하고 있는 타일맵을 사용한다.
            _tilemapManager = FindAnyObjectByType<TilemapManager>();

            // 타일맵의 시작 타일의 중심 위치에서 시작한다.
            Vector3 startTilePosition = _tilemapManager.GetStartTileCenterPosition();
            transform.position = new Vector3(startTilePosition.x, transform.position.y, transform.position.z);
        }
        
        // Update()
        private void Update()
        {
            Vector3 currentPosition = transform.position; // 플레이어의 현재 위치 (월드 좌표)
            Vector3Int forwardDirection = default; // 바라보는 방향
            
            // ↑ 키를 누를 경우,
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // 위쪽 방향을 목표 방향으로 한다.
                forwardDirection = ConvertDirectionToVector(PlayerDirection.Forward);
            }

            // ↓ 키를 누를 경우,
            if (Input.GetKeyDown(KeyCode.DownArrow))
            { 
                // 아래쪽 방향을 목표 방향으로 한다.
                forwardDirection = ConvertDirectionToVector(PlayerDirection.Back);
            }

            // ← 키를 누를 경우,
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            { 
                // 왼쪽 방향을 목표 방향으로 한다.
                forwardDirection = ConvertDirectionToVector(PlayerDirection.Left);
            }

            // → 키를 누를 경우,
            if (Input.GetKeyDown(KeyCode.RightArrow))
            { 
                // 오른쪽 방향을 목표 방향으로 한다.
                forwardDirection = ConvertDirectionToVector(PlayerDirection.Right);
            }
            
            // 목표 방향에 타일이 있을 경우,
            if (forwardDirection != default && _tilemapManager.GetNextTile(currentPosition, forwardDirection))
            {
                // 그 타일의 중심 위치를 받아온다.
                Vector3 nextTilePosition = _tilemapManager.GetNextTileCenterPosition(currentPosition, forwardDirection);
                
                // 받아온 위치로 이동한다. 단, 이동 중일 때는 이동할 수 없다.
                _moveCoroutine ??= StartCoroutine(MoveToNextTile(nextTilePosition));
            }
        }

        // PlayerDirection 열거형을 Vector3Int로 변환하는 함수
        private Vector3Int ConvertDirectionToVector(PlayerDirection direction)
        {
            Vector3Int convertedVector = default;
            
            switch (direction)
            {
                case PlayerDirection.Forward: // Forward = up
                    convertedVector = Vector3Int.up;
                    break;
                case PlayerDirection.Back: // Back = down
                    convertedVector = Vector3Int.down;
                    break;
                case PlayerDirection.Left: // Left = left
                    convertedVector = Vector3Int.left;
                    break;
                case PlayerDirection.Right: // Right = right
                    convertedVector = Vector3Int.right;
                    break;
            }

            return convertedVector;
        }

        // 이동을 담당하는 코루틴 함수
        private IEnumerator MoveToNextTile(Vector3 nextTile)
        {
            // 다음 타일에 도달할 때까지 이동을 반복한다.
            while (transform.position != nextTile)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextTile, 0.1f);
                Debug.Log("Coroutine is Running.");
                yield return null;
            }

            _moveCoroutine = null;
        }
        
        #endregion 함수(Method)
    }
}

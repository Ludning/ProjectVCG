using UnityEngine;

public class StageManager : MonoBehaviour // ≒ GameManager; 각 스테이지의 최상위 매니저 같은 느낌
{
    #region 변수
    
    private TilemapManager _tilemapManager; // 타일맵(Tilemap) 매니저
    private PlayerController _playerController; // 플레이어(Player)
    
    #endregion 변수
    
    
    #region 함수
    
    // Awake()
    private void Awake()
    {
        // 전역 변수를 초기화한다.
        InitializeComponents();
        
        // 플레이어를 시작 타일을 기준으로 하여 초기화한다.
        _playerController.Initialize(_tilemapManager.StartTile);
    }

    // 전역 변수를 초기화한다.
    private void InitializeComponents()
    {
        _tilemapManager = FindAnyObjectByType<TilemapManager>();
        _playerController = FindAnyObjectByType<PlayerController>();
    }
    
    #endregion 함수
}

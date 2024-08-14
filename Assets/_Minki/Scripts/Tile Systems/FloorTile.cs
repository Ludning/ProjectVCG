using UnityEngine;

namespace TileSystem
{
    public enum SpawnRotation
    {
        _0,
        _90,
        _180,
        _270,
    }
    
    // 바닥 타일; 플레이어는 바닥 타일 위로 이동할 수 있습니다.
    public class FloorTile : BaseTile //, IWalkable
    {
        [SerializeField] private Transform centrePosition; // 타일의 정중앙 위치
        [SerializeField] private bool isSpawnTile; // 시작 타일인가?
        [SerializeField] private SpawnRotation spawnRotation; // 시작 방향
    }
}

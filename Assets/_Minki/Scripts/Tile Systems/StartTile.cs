using UnityEngine;

namespace TileSystem
{
    public enum Rotation
    {
        _0 = 0,
        _90 = 90,
        _180 = 180,
        _270 = 270,
    }
    
    // 시작 타일; 바닥 타일과 같은, 플레이어가 시작 시 생성되는 위치의 타일입니다.
    public class StartTile : FloorTile
    {
        // [SerializeField] private bool isSpawnTile; // 시작 타일인가?
        [SerializeField] private Direction spawnRotation; // 시작 방향
        public Direction SpawnRotation => spawnRotation;
    }
}

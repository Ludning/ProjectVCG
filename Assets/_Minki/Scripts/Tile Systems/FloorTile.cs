using UnityEngine;

namespace TileSystem
{
    // 바닥 타일; 플레이어는 바닥 타일 위로 이동할 수 있습니다.
    public class FloorTile : BaseTile
    {
        [SerializeField] private Transform centreTransform; // 타일의 정중앙 위치
        public Transform CentreTransform => centreTransform;
    }
}

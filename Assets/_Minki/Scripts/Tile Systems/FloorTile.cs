namespace TileSystem
{
    // 바닥 타일; 플레이어는 바닥 타일 위로 이동할 수 있습니다.
    public class FloorTile : BaseTile, IWalkable
    {
        public bool Walk()
        {
            return true;
        }
    }
}

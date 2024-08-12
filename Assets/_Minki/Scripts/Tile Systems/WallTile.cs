namespace TileSystem
{
    // 벽 타일; 플레이어는 벽 타일 위로 이동할 수 없습니다.
    public class WallTile : BaseTile, INotWalkable
    {
        private void Awake()
        {
            // 시작 전, 타일의 종류를 '벽'으로 지정합니다.
            TypeName = TileType.Wall;
        }
        
        // Interface Method
        public bool Walk()
        {
            return false;
        }
    }
}

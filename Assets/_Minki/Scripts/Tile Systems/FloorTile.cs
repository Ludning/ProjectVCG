namespace TileSystem
{
    // 바닥 타일; 플레이어는 바닥 타일 위로 이동할 수 있습니다.
    public class FloorTile : BaseTile, IWalkable
    {
        private void Awake()
        {
            // 시작 전, 타일의 종류를 '바닥'으로 지정합니다.
            TypeName = TileType.Walk;
        }

        // Interface Method
        public bool Walk()
        {
            // TODO: 플레이어의 움직임과 관련한 내용을 이곳에서 작업할 것인가?
            return true;
        }
    }
}

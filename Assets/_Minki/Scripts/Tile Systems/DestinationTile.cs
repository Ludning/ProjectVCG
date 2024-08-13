namespace TileSystem
{
    // 목적지 타일; 플레이어는 목적지 타일 위로 이동할 수 있으며, 이 타일에 도달할 경우 스테이지를 달성합니다.
    public class DestinationTile : BaseTile, IWalkable
    {
        // Interface Method
        // TODO: 목적지의 도착 판정을 타입의 비교로 할 것인가, 아니면 충돌체의 충돌 이벤트로 할 것인가?
        public bool Walk()
        {
            return true;
        }
    }
}

public enum UIType
{
    MainUI,
    TitleUI,
}

public enum TileType
{
    Floor, // 바닥: PC가 이동할 수 있는 타일; 1층, 정육면체
    Wall, // 벽: PC가 이동할 수 없는 타일; 무엇과도 상호작용할 수 없는 높은 층, 정육면체
    Kitchen, // 조리 테이블: 바닥보다 높은, 이동할 수 없는 타일; 2층, 정육면체
    Customer, // 손님 테이블: 손님에게 서빙하는 컨텐츠용 테이블 타일
    Plate, // 음식을 완성하는 타일 (플레이팅)
}

public enum BlockLogicType
{
    
}
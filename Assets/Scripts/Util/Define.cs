public enum UIType
{
    MainUI,
    TitleUI,
}

public enum LogicState
{
    Running,
    Success,
    Failure,
}

public enum TileType
{
    Empty,
    Walk,
    Serving,
    Kitchen,
    Customer,
    Blocking,
    Cook,
    Pass,
}

public enum BlockLogicType
{
    Cook,
    Move,
    RotateLeft,
    RotateRight,
    RotateBack,
    PushItem,
    PopItem,
    Start,
    Clear,
    Dash,
}
public enum Rotate
{
    Left,
    Right,
}
public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right,
}

public enum ChapterIndex
{
    Chapter1,
    Chapter2,
    Chapter3,
}

public enum StageIndex
{
    Serving1,
    Serving2,
    Serving3,
    Serving4,
    Serving5,
    Serving6,
    Serving7,
    Serving8,
    None
}

public enum CookType
{
    Chop,
    Glid,
    Boil,
}

public enum AssetType
{
    GameObject,
    Material,
}

public enum ErrorType
{
    NoError,             // 에러 없음
    UnKnownError,        // 알수 없는 에러
    NotMove,             // 이동 불가 오류
    NoPickableItem,      // 들 수 있는 물건 없음 오류
    InventoryOverflow,   // 인벤토리 초과 오류
    NoDropItem,          // 내려놓을 오브젝트 없음 오류
    InvalidDropTile,     // 내려놓을 수 없는 타일 오류
    WrongItemOnTool,     // 조리도구에 알맞지 않은 오브젝트 내려놓음 오류
    ItemAlreadyOnTile,   // 조리도구 아닌 타일에 이미 오브젝트 올려져 있음 오류
    InvalidCookAction,   // 조리 타일 아닌 곳에서 조리 행동 오류
    NoIngredientOnTile,  // 조리 타일에 올려진 재료가 없음 오류
    InvalidCookCombo,    // 조리 불가능한 조합 조리 오류
    StackLimitExceeded,  // 적치 가능 수치 초과 오류
    ToolInventoryFull,   // 조리 도구 인벤토리 초과 오류
    MissionNotStarted    // 미션 미시행 오류
}

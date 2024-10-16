using System;

public enum NpcType
{
    Null,
    Dummy = 10,
    Asian_Male = 11,
    Asian_Female = 12,
    European_Male = 13,
    European_Female = 14,
    African_Female = 15,
    African_Male = 16
}

public enum CookingPropertyType
{
    NULL,
    CUT = 30000,
    POT = 30001,
    FAN = 30002,
    FRYER = 30003,
    TRAY = 30004,
}

public enum SortClearType
{
    ARRIVE,         //목적지 도달
    SERVING,        //서빙
    RECIPE,         //레시피
    RECIPE_CLEAR,   //레시피 클리어
}

public enum ItemType
{
    NULL,
    SHRIMP = 15001,
    RICE = 15002,
    FISH = 15003,
    FRYER_SHRIMP = 15512,
    POT_RICE = 15502,
    CUT_FISH = 15503,
    CUT_SHRIMP = 15504,
    EBIDON = 19001,
    SUSHI_SHRIMP = 19002,
}

public enum FoodType
{
    ORIGINAL,
    PROCESSED

}

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
    EMPTY = 0,
    WALK = 10000,
    WALL = 10100,
    PASS = 10001,
    SERVING = 10110,
    CUSTOMER = 10111,
    INGREDIENT = 10201,
    COOKING = 10301,
    TABLE = 10401,
}

public enum TileDetailType
{
    EMPTY,
}

[Flags]
public enum TileAttributeType
{
    None = 0,
    Moveable = 1 << 0,
    Pushable  = 1 << 1,
    Popable  = 1 << 2,
    Stackable  = 1 << 3,
    Cookable  = 1 << 4,
}

public enum TileAttributeCheckType
{
    WalkAble,
    PushAble,
    PopAble,
    StackAble,
    CookAble,
    InventoryEmpty,
    InventoryEmptyOrFull
}

public enum BlockLogicType
{
    Cook,
    Move,
    Dash,
    RotateLeft,
    RotateRight,
    RotateBack,
    PopItem,
    PushItem,
    Function,
    Repeat,
    Start,
    Clear,
    Reset,
    Exercise,
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

public enum AssetType
{
    GameObject,
    Material,
}

public enum ErrorType
{
    NoError = 2015, // 에러 없음
    StageClear = 2016, // 클리어
    OmissionError = 2014, // 무시할 수 있는 에러(함수블록을 생략한다)
    UnKnownError = 2014, // 알수 없는 에러
    NoTile = 2013, // 타일 없음
    NotMove = 2000, // 이동 불가 오류
    NoPickableItem = 2001, // 들 수 있는 물건 없음 오류
    InventoryOverflow = 2002, // 인벤토리 초과 오류
    NoDropableItem = 2003, // 내려놓을 오브젝트 없음 오류
    InvalidDropTile = 2004, // 내려놓을 수 없는 타일 오류
    WrongItemOnTool = 2005, // 조리도구에 알맞지 않은 오브젝트 내려놓음 오류
    ItemAlreadyOnTile = 2006, // 조리도구 아닌 타일에 이미 오브젝트 올려져 있음 오류
    InvalidCookAction = 2007, // 조리 타일 아닌 곳에서 조리 행동 오류
    NoIngredientOnTile = 2008, // 조리 타일에 올려진 재료가 없음 오류
    InvalidCookCombo = 2009, // 조리 불가능한 조합 조리 오류
    StackLimitExceeded = 2010, // 적치 가능 수치 초과 오류
    ToolInventoryFull = 2011, // 조리 도구 인벤토리 초과 오류
    MissionNotStarted = 2012, // 미션 미시행 오류
}

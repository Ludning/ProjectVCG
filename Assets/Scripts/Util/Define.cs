using System;

public enum NpcType
{
    
}

public enum ItemType
{
    Null,
    Poke = 11100,
    HamburgerSet = 11101,
    Cut_Potato = 11200,
    Pot_Rice = 11201,
    Cut_Shirimp = 11202,
    Pot_Rice_2 = 11203,
    Frier_Shirimp = 11204,
    Cut_Fish = 11205,
    Potato = 11500,
    Tuna = 11501,
    Rice = 11502,
    Fish = 11503,
    Shirimp = 11504,
    SHIRIMP_SUSHI = 21000,
    SHIRIMP_RICE = 21001,
    SASHIMI = 21002,
    POT_RICE = 20000,
    CUT_SHIRIMP = 20001,
    POT_RICE_2 = 20002,
    FRIER_SHIRIMP = 20003,
    CUT_FISH = 20004,
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
    None,
    Moveable,
    Pushable,
    Popable,
    Stackable,
    Cookable,
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
    RotateLeft,
    RotateRight,
    RotateBack,
    PushItem,
    PopItem,
    Start,
    Clear,
    Reset,
    Dash,
    Function,
    Repeat,
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
    Empty,
    Chop,
    Glid,
    Boil,
    Fry,
    Tray,
}

public enum AssetType
{
    GameObject,
    Material,
}

public enum ErrorType
{
    NoError = 2015, // 에러 없음
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

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
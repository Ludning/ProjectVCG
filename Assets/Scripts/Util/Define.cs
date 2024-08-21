public enum UIType
{
    MainUI,
    TitleUI,
}

public enum TileType
{
    Empty,
    Walk,
    Serving,
    Kitchen,
    Customer,
    Blocking,
    Pass,
}

public enum BlockLogicType
{
    Cook,
    Move,
    PushItem,
    RotateLeft,
    RotateRight,
    RotateBack,
    SetItem,
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

public enum CookType
{
    Chop,
    Glid,
    Boil,
}
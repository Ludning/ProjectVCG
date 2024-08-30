using System.Collections.Generic;

public class GameData
{
    public Dictionary<string, PcData> Pc;
    public Dictionary<string, NpcData> Npc;
    public Dictionary<string, CodingBlockData> CodingBlock;
    public Dictionary<string, FeedbackData> Feedback;
    public Dictionary<string, ErrorData> ErrorMessage;
    public Dictionary<string, StageData> Stage;
    public Dictionary<string, StageGoalData> StageGoal;
    public Dictionary<string, TileData> Tile;
    public Dictionary<string, Tile_FoodData> Food;

    //public Dictionary<string, 개별_레벨Data> 개별_레벨;\
}

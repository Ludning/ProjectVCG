using System.Collections.Generic;

public class GameData
{
    public Dictionary<string, PcData> Pc;
    public Dictionary<string, NpcData> Npc;
    public Dictionary<string, CodingBlockData> CodingBlock;
    public Dictionary<string, FeedbackData> Feedback;
    public Dictionary<string, StageData> Stage;
    public Dictionary<string, StageGoalData> StageGoal;
    public Dictionary<string, Tile_SpaceData> Tile_Space;
    public Dictionary<string, Tile_FoodData> Tile_Food;

    //public Dictionary<string, 개별_레벨Data> 개별_레벨;\

}

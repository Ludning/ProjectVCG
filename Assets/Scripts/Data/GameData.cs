using System.Collections.Generic;

public class GameData
{
    public Dictionary<string, PcData> Pc { get; set; }
    public Dictionary<string, NpcData> Npc { get; set; }
    public Dictionary<string, CodingBlockData> CodingBlock { get; set; }
    public Dictionary<string, FeedbackData> Feedback { get; set; }
    public Dictionary<string, StageData> Stage { get; set; }
    public Dictionary<string, StageGoalData> StageGoal { get; set; }
    public Dictionary<string, Tile_SpaceData> Tile_Space { get; set; }
    public Dictionary<string, Tile_FoodData> Tile_Food { get; set; }
    
    //public Dictionary<string, 개별_레벨Data> 개별_레벨;\

}

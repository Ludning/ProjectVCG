using System.Collections.Generic;

public class GameData
{
    public Dictionary<string, PcData> Pc;
    public Dictionary<string, NpcData> Npc;
    public Dictionary<string, CodingBlockData> CodingBlock;
    public Dictionary<string, FeedbackData> Feedback;
    public Dictionary<string, ErrorMessageData> ErrorMessage;
    public Dictionary<string, StageData> Stage;
    public Dictionary<string, StageGoalData> StageGoal;
    public Dictionary<string, LevelData> Level;
    public Dictionary<string, TileData> Tile;
    public Dictionary<string, FoodData> Food;
    public Dictionary<string, CookingPropertyData> CookingProperty;
    public Dictionary<string, RecipeData> Recipe;
}

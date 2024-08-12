using UnityEngine;

public class BlockLogicBase : MonoBehaviour
{
    public virtual bool IsExecutable(StageManager owner)
    {
        //이동의 예시
        
        //앞으로 이동하면 캐릭터 기준 앞 위치를 매개변수로 넘겨줌
        TileType tileType = table.GetTileType(position.x, position.y);
        ItemBase item = table.GetTileItem(position.x, position.y);

        if (tileType != TileType.Floor || item != null)
        {
            return false;
        }
        return true;
    }
    
    public virtual bool Execute(StageManager owner)
    {
        return true;
    }
}

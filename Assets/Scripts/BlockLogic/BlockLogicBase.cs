using UnityEngine;

public class BlockLogicBase : MonoBehaviour
{
    public bool IsExecutable(TableManager table, Vector2Int position)
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
    
    public bool Execute(StageManager owner)
    {
        //TODO, 이동 로직의 예시
        //owner.Player.transform.position = Vector2.Lerp(owner.Player.transform.position, owner.TargetPosition, 0.1f);
        
        //TODO
        //완료하면 True, 아니면 False
        return true;
    }
}

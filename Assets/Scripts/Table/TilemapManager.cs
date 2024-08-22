using System.Collections.Generic;
using UnityEngine;

// 타일맵을 관리하는 매니저 클래스
public class TilemapManager : MonoBehaviour
{
    #region 변수
    
    private Dictionary<Vector2Int, TileSystem.BaseTile> _tilemap; // 타일맵; 전체 타일의 정보를 Dictionary<Vector2Int, TileBase>로 저장한다.
    public TileSystem.StartTile StartTile { get; private set; } // 시작 타일

    #endregion 변수
    
    #region 함수
    
    // Awake()
    private void Awake()
    {
        // 타일맵을 초기화하여, 시작 타일을 찾는다.
        StartTile = InitializeTilemap(_tilemap);
    }

    // 타일맵을 초기화한다.
    private TileSystem.StartTile InitializeTilemap(Dictionary<Vector2Int, TileSystem.BaseTile> tilemap)
    {
        // 시작 타일의 참조를 생성한다.
        TileSystem.StartTile start = null;
        
        // Dictionary를 초기화한다.
        tilemap ??= new(); // = if (tilemap == null) new();
        tilemap.Clear();
        
        // 타일인 모든 자식을 가져와서, Dictionary에 넣는다.
        TileSystem.BaseTile[] tiles = GetComponentsInChildren<TileSystem.BaseTile>();
        foreach (TileSystem.BaseTile tile in tiles)
        {
            // 타일의 위치를 Vector2Int로 저장한다. (float → int)
            // TODO: Vector2Int? Vector2?
            Vector2Int tilePosition = new((int)tile.transform.position.x, (int)tile.transform.position.z);
            tilemap.TryAdd(tilePosition, tile);

            // 만약 시작 타일일 경우, 참조를 연결한다.
            if (tile is TileSystem.StartTile sTile) start = sTile;
        }

        // 시작 타일 참조를 반환한다.
        return start;
    }

    // 타일의 종류를 반환한다.
    public TileSystem.TileType GetTileType(Vector2Int position)
    {
        TileSystem.TileType returnType = _tilemap.TryGetValue(position, out TileSystem.BaseTile baseTile) ? baseTile.TypeName : TileSystem.TileType.None;
        return returnType;
    }
    
    // 타일의 아이템을 반환한다.
    public ItemSystem.BaseItem GetTileItem(Vector2Int position)
    {
        ItemSystem.BaseItem returnItem = _tilemap.TryGetValue(position, out TileSystem.BaseTile baseTile) ? baseTile.ItemName : null;
        return returnItem;
    }
    
    #endregion 함수
}

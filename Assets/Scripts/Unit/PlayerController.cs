using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TableManager TableManager;

    public LogicHint LogicHint;
    
    [ReadOnly]
    public Direction PlayerForwardType;
    private Vector2Int _playerPosition;
    public Vector2Int PlayerPosition
    {
        get => _playerPosition;
        set
        {
            TileBase prevTile = TableManager.PeekTile(_playerPosition);
            prevTile?.OutlineMap?.ChangeCurrentTileColor(false);
            Debug.Log($"prevTile : {_playerPosition}");
            _playerPosition = value;
            TileBase nextTile = TableManager.PeekTile(_playerPosition);
            nextTile.OutlineMap.ChangeCurrentTileColor(true);
            Debug.Log($"nextTile : {_playerPosition}");
        }
    }
    public Vector2Int PlayerForwardPosition => PlayerPosition + PlayerForward;
    public Vector2Int PlayerForward
    {
        get
        {
            switch (PlayerForwardType)
            {
                case Direction.Up:
                    return Vector2Int.left;
                case Direction.Down:
                    return Vector2Int.right;
                case Direction.Left:
                    return Vector2Int.down;
                case Direction.Right:
                    return Vector2Int.up;
            }
            return Vector2Int.zero;
        }
    }
    
    public void Init(Vector2Int position, Vector3 worldPosition, Direction forwardType)
    {
        transform.position = worldPosition;
        transform.position += transform.up * GameManager.Instance.PlayerPositionAdditive;
        
        PlayerPosition = position;
        PlayerForwardType = forwardType;
        switch (forwardType)
        {
            case Direction.Up:
                transform.localRotation = Quaternion.Euler(0, 90, 0);
                break;
            case Direction.Down:
                transform.localRotation = Quaternion.Euler(0, 270, 0);
                break;
            case Direction.Left:
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.Right:
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                break;
        }
    }

    public void Clear()
    {
        LogicHint.HideLogicHint();;
    }
}

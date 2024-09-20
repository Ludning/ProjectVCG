using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LogicHint LogicHint;
    
    [ReadOnly]
    public Direction PlayerForwardType;
    [ReadOnly]
    public Vector2Int PlayerPosition;
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

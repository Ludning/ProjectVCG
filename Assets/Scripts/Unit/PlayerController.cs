using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform LogicHint;
    
    public Direction PlayerForwardType;
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
    
    public void Init(Vector2Int position, Vector3 wolrdPosition, Direction forwardType)
    {
        transform.position = wolrdPosition;
        transform.position = new Vector3(transform.position.x, 0.8f, transform.position.z);
        
        PlayerPosition = position;
        PlayerForwardType = forwardType;
        switch (forwardType)
        {
            case Direction.Up:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case Direction.Down:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.Left:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Direction.Right:
                transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
        }
    }
}

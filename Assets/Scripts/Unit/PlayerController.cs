using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2Int PlayerForward;
    public Vector2Int PlayerPosition;
    public Vector2Int PlayerForwardPosition => PlayerPosition + PlayerForward;

    public void SetPLayerForward(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                PlayerForward = Vector2Int.up;
                break;
            case Direction.Down:
                PlayerForward = Vector2Int.down;
                break;
            case Direction.Left:
                PlayerForward = Vector2Int.left;
                break;
            case Direction.Right:
                PlayerForward = Vector2Int.right;
                break;
        }
    }
    public Direction GetPLayerForwardDirection()
    {
        if (PlayerForward == Vector2Int.up)
            return Direction.Up;
        else if (PlayerForward == Vector2Int.down)
            return Direction.Down;
        else if (PlayerForward == Vector2Int.left)
            return Direction.Left;
        else if (PlayerForward == Vector2Int.right)
            return Direction.Right;
        else
            return Direction.None;

    }
}

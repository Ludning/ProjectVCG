using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Direction PlayerForwardType;
    public Vector2Int PlayerPosition;
    public Vector2Int PlayerForwardPosition => PlayerPosition + PlayerForward;
/*    private PlayerTopMsg playerTopMsg;
*/    public Vector2Int PlayerForward
    {
        get
        {
            switch (PlayerForwardType)
            {
                case Direction.Up:
                    return Vector2Int.up;
                case Direction.Down:
                    return Vector2Int.down;
                case Direction.Left:
                    return Vector2Int.left;
                case Direction.Right:
                    return Vector2Int.right;
            }
            return Vector2Int.zero;
        }
    }
    
    public void Init(Vector2Int position, Direction forwardType)
    {
        transform.position = new Vector3(position.x, 1.5f, position.y);
        PlayerPosition = position;
        PlayerForwardType = forwardType;
    }
}

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Direction _forwardDirection;
    private Vector2Int _position;

    public void Initialize(TileSystem.StartTile startTile)
    {
        transform.position = startTile.CentreTransform.position;
        _forwardDirection = startTile.SpawnRotation;
    }
}

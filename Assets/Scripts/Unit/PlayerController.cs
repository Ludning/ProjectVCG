using System;
using UnityEngine;

public enum AnimationState
{
    IsIdle,
    IsMove,
    IsDash,
    IsLeftTurn,
    IsRightTurn,
    IsTurnBack,
    IsCook,
    IsPuttingDown,
    IsLifting,
    IsCarry,
}
public class PlayerController : MonoBehaviour
{
    public TableManager TableManager;

    public LogicHint LogicHint;

    [SerializeField] private Animator PlayerAnimator;
    
    private static readonly int IsIdle = Animator.StringToHash("IsIdle");
    private static readonly int IsMove = Animator.StringToHash("IsMove");
    private static readonly int IsDash = Animator.StringToHash("IsDash");
    private static readonly int IsLeftTurn = Animator.StringToHash("IsLeftTurn");
    private static readonly int IsRightTurn = Animator.StringToHash("IsRightTurn");
    private static readonly int IsTurnBack = Animator.StringToHash("IsTurnBack");
    private static readonly int IsCook = Animator.StringToHash("IsCook");
    private static readonly int IsPuttingDown = Animator.StringToHash("IsPuttingDown");
    private static readonly int IsLifting = Animator.StringToHash("IsLifting");
    private static readonly int IsCarry = Animator.StringToHash("IsCarry");
    
    [ReadOnly]
    public Direction PlayerForwardType;
    private Vector2Int _playerPosition;
    public Vector2Int PlayerPosition
    {
        get => _playerPosition;
        set
        {
            TileBase prevTile = TableManager.PeekTile(_playerPosition);
            prevTile?.ChangeCurrentTileSpriteColor(false);
            Debug.Log($"prevTile : {_playerPosition}");
            _playerPosition = value;
            TileBase nextTile = TableManager.PeekTile(_playerPosition);
            nextTile.ChangeCurrentTileSpriteColor(true);
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
        PlayerAnimator.SetBool(IsIdle, true);
        PlayerAnimator.SetBool(IsMove, false);
        PlayerAnimator.SetBool(IsDash, false);
        PlayerAnimator.SetBool(IsLeftTurn, false);
        PlayerAnimator.SetBool(IsRightTurn, false);
        PlayerAnimator.SetBool(IsTurnBack, false);
        PlayerAnimator.SetBool(IsCook, false);
        PlayerAnimator.SetBool(IsPuttingDown, false);
        PlayerAnimator.SetBool(IsLifting, false);
        PlayerAnimator.SetBool(IsCarry, false);
        
        transform.position = worldPosition;
        transform.position += transform.up * GameManager.Instance.PlayerPositionAdditive;
        
        PlayerPosition = position;
        PlayerForwardType = forwardType;
        switch (forwardType)
        {
            case Direction.Up:
                transform.localRotation = Quaternion.Euler(0, 270, 0);
                break;
            case Direction.Down:
                transform.localRotation = Quaternion.Euler(0, 90, 0);
                break;
            case Direction.Left:
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                break;
            case Direction.Right:
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
        }
    }

    public void SetAnimationState(AnimationState state, bool value)
    {
        switch (state)
        {
            case AnimationState.IsIdle:
                PlayerAnimator.SetBool(IsIdle, value);
                break;
            case AnimationState.IsMove:
                PlayerAnimator.SetBool(IsMove, value);
                break;
            case AnimationState.IsDash:
                PlayerAnimator.SetBool(IsDash, value);
                break;
            case AnimationState.IsLeftTurn:
                PlayerAnimator.SetBool(IsLeftTurn, value);
                break;
            case AnimationState.IsRightTurn:
                PlayerAnimator.SetBool(IsRightTurn, value);
                break;
            case AnimationState.IsTurnBack:
                PlayerAnimator.SetBool(IsTurnBack, value);
                break;
            case AnimationState.IsCook:
                PlayerAnimator.SetBool(IsCook, value);
                break;
            case AnimationState.IsPuttingDown:
                PlayerAnimator.SetBool(IsPuttingDown, value);
                break;
            case AnimationState.IsLifting:
                PlayerAnimator.SetBool(IsLifting, value);
                break;
            case AnimationState.IsCarry:
                PlayerAnimator.SetBool(IsCarry, value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    public void Clear()
    {
        LogicHint.HideLogicHint();;
    }
}

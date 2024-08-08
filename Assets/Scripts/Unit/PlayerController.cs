using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2Int PlayerForward;
    public Vector2Int PlayerPosition;
    public Vector2Int PlayerForwardPosition => PlayerPosition + PlayerForward;
}

using UnityEngine;

[CreateAssetMenu()]
public class TestTile : UnityEngine.Tilemaps.TileBase
{
    [SerializeField] private int myInt;
    [SerializeField] private float myFloat;
    [SerializeField] private bool myBool;
    [SerializeField] private string myString;
}
using System.Collections.Generic;
using UnityEngine;

public class TilemapReader : MonoBehaviour
{
    // [SerializeField] private TableManager tableManager;

    public void GetTilemap()
    {
        Transform[] tileTransforms = GetComponentsInChildren<Transform>();
        
        
        Dictionary<Vector2Int, TileBase> tileBases = new Dictionary<Vector2Int, TileBase>();
        
        foreach (var tileTransform in tileTransforms)
        {
            //좌표는 
            int x = (int)tileTransform.position.x;
            int y = (int)tileTransform.position.z;

            tileBases[new Vector2Int(x, y)] = tileTransform.GetComponent<TileBase>();
        }
        
        // tableManager.Init(tileBases);
    }
}

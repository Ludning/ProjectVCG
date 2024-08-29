using System.Collections.Generic;
using UnityEngine;

public class MapReader : MonoBehaviour
{
    [SerializeField] private TableManager tableManager;

    /*public void ReadMap()
    {
        List<Transform> tileTransforms = new List<Transform>();
        foreach (Transform child in transform)
        {
            tileTransforms.Add(child);
        }
        Dictionary<Vector2Int, TileBase> tileBases = new Dictionary<Vector2Int, TileBase>();
        foreach (var tileTransform in tileTransforms)
        {
            //좌표는 
            int x = (int)tileTransform.position.x;
            int y = (int)tileTransform.position.z;

            tileBases[new Vector2Int(x, y)] = tileTransform.GetComponent<TileBase>();
        }
        tableManager.Init(tileBases);
    }*/
}

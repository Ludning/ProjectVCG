using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileSystem
{
    public class TilemapManager : MonoBehaviour
    {
        // 타일맵(Tilemap) 컴포넌트
        [SerializeField] private Tilemap tilemap;
        
        private void Awake()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                tilemap.GetTile
            }
        }
    }
}

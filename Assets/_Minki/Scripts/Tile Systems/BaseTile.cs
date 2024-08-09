using UnityEngine;

namespace Minki.TileSystem
{
    // A Top-Level Class for Tile Item Classes.
    public class BaseTile : MonoBehaviour
    {
        // Tile Type; Defines What Type of the Tile is.
        public TileType TypeName { get; protected set; }
    }
}

using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapSystem
{
    [CreateAssetMenu(fileName = "Custom 3D Tile", menuName = "Tiles/Custom 3D Tile", order = 0)]
    public class Custom3DTile : UnityEngine.Tilemaps.TileBase
    {
        #region 변수(Field)
        
        // 3D 게임 오브젝트 프리팹
        [SerializeField] private GameObject tilePrefab;
        // 2D 스프라이트 프리팹; 타일 팔레트(Tile Palette)에서 타일을 구별하기 위한 요소
        [SerializeField] private Sprite tileSprite;
        // 2D 스프라이트 색상(Color); 스프라이트 프리팹 자체로 구별해도 된다.
        [SerializeField] private Color tileSpriteColor;
        
        #endregion 변수(Field)
        
        #region TileBase 클래스의 재정의(Override) 함수

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref UnityEngine.Tilemaps.TileData tileData)
        {
            tileData.gameObject = tilePrefab;
            tileData.sprite = tileSprite;
            tileData.color = tileSpriteColor;
        }
        
        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            base.RefreshTile(position, tilemap);
        }
        
        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        {
            return base.GetTileAnimationData(position, tilemap, ref tileAnimationData);
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            // Debug.Log($"StartUp() 함수가 호출되었습니다! position = {position}, tilemap = {tilemap}, go = {go}");
            return base.StartUp(position, tilemap, go);
        }

        #endregion TileBase 클래스의 재정의(Override) 함수
    }
}

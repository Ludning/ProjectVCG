using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapSystem
{
    // 타일맵(Tilemap)을 관리하는 매니저 클래스
    public class TilemapManager : MonoBehaviour
    {
        #region 컴포넌트(Component)
    
        // 타일맵(Tilemap) 컴포넌트
        [SerializeField] private Tilemap tilemap;
        
        #endregion 컴포넌트(Component)
        
        #region 함수(Method)
        
        /// <summary>
        /// 타일맵 안의 시작 타일을 찾아, 그 중심 위치 값을 반환합니다.
        /// </summary>
        /// <returns>시작 타일의 중심 위치 값, 월드 좌표 기준</returns>
        public Vector3 GetStartTileCenterPosition()
        {
            // 시작 타일의 위치 값 변수 (default = Vector3.zero)
            Vector3 startTile = default;
            
            // 타일맵 안의 모든 타일을 순회하면서,
            foreach (Vector3Int tilePosition in tilemap.cellBounds.allPositionsWithin)
            {
                // 시작 타일을 찾을 경우,
                if (tilemap.GetTile<UnityEngine.Tilemaps.TileBase>(tilePosition) is Custom3DTile) // TODO: Custom3DTile → StartTile
                {
                    // 그 시작 타일의 중심 위치를 월드 좌표로 변환하여 변수에 저장합니다.
                    startTile = tilemap.GetCellCenterWorld(tilePosition);
                    break;
                }
            }
            
            // 변수를 반환합니다.
            return startTile;
        }

        /// <summary>
        /// 플레이어의 다음 행동에 대응하는 타일의 중심 위치 값을 반환합니다.
        /// </summary>
        /// <param name="currentPlayerPosition">현재 플레이어의 위치 값, 월드 좌표 기준</param>
        /// <param name="nextTileDirection">다음 타일로의 목표 방향, 타일맵 좌표 기준</param>
        /// <returns>다음 타일의 중심 위치 값, 월드 좌표 기준</returns>
        public Vector3 GetNextTileCenterPosition(Vector3 currentPlayerPosition, Vector3Int nextTileDirection)
        {
            // 플레이어의 현재 위치를 월드 좌표로 가져와서, 타일맵의 셀 좌표로 변환합니다.
            Vector3Int currentOnTilePosition = tilemap.WorldToCell(currentPlayerPosition);
            
            // 현재 위치의 타일로부터, 다음 방향에 있는 타일의 중심 위치를 찾습니다.
            Vector3 nextTilePosition = tilemap.GetCellCenterWorld(currentOnTilePosition + nextTileDirection);
            
            // y축의 값은 무시합니다.
            nextTilePosition.y = currentPlayerPosition.y;

            // 값을 반환합니다.
            return nextTilePosition;
        }

        /// <summary>
        /// 플레이어의 다음 행동에 대응하는 타일의 정보를 반환합니다.
        /// </summary>
        /// <param name="currentPlayerPosition">현재 플레이어의 위치 값, 월드 좌표 기준</param>
        /// <param name="nextTileDirection">다음 타일로의 목표 방향, 타일맵 좌표 기준</param>
        /// <returns>다음 타일의 객체(정보)</returns>
        public UnityEngine.Tilemaps.TileBase GetNextTile(Vector3 currentPlayerPosition, Vector3Int nextTileDirection)
        {
            // 플레이어의 현재 위치를 월드 좌표로 가져와서, 타일맵의 셀 좌표로 변환한다.
            Vector3Int currentOnTilePosition = tilemap.WorldToCell(currentPlayerPosition);

            // 현재 위치 타일로부터, 다음 방향에 있는 타일을 찾는다.
            UnityEngine.Tilemaps.TileBase nextTile = tilemap.GetTile(currentOnTilePosition + nextTileDirection);

            // 찾지 못할 경우, 기본 값(= Vector3Int.zero)을 반환한다.
            return nextTile;
        }
        
        #endregion 함수(Method)
    }
}

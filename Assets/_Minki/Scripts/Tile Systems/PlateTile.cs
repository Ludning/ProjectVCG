using System.Collections.Generic;
using FoodSystem;
using UnityEngine;

namespace TileSystem
{
    // 서빙 테이블 타일; 플레이어가 손님 테이블 타일에 전달하기 위한 완성된 음식을 가지고 있는 타일입니다.
    // 플레이어는 서빙 테이블 타일 위로 이동할 수 없으며, 이 타일에 있는 완성된 음식을 들어올릴 수 있습니다.
    public class ServingTile : BaseTile, ILiftable
    {
        // 완성된 음식 객체
        // 직렬화: 스테이지에 따라 처음부터 완성된 음식이 존재할 수 있습니다.
        [SerializeField] private BaseFood servingFood;
        public BaseFood ServingFood
        {
            set => servingFood = value;
        }
        
        private void Awake()
        {
            // 시작 전, 타일의 종류를 지정합니다.
            TypeName = TileType.Serving;
        }
        
        #region Interface Methods

        // 이 함수는 플레이어가 호출합니다.
        public BaseFood Lift()
        {
            return servingFood; // 플레이어는 완성된 음식의 참조 값을 받습니다.
        }

        // Queue<T>를 사용할 경우
        public void Lift(Queue<BaseFood> inHand)
        {
            inHand.Enqueue(servingFood); // 플레이어는 자신의 Queue<BaseFood> 데이터에 완성된 음식의 참조 값을 추가합니다.
        }
        
        #endregion Interface Methods
    }
}

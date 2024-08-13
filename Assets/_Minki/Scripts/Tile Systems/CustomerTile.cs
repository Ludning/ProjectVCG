using System.Collections.Generic;
using FoodSystem;
using UnityEngine;
using UnityEngine.UI;

namespace TileSystem
{
    // 손님 테이블 타일; 플레이어가 어떠한 음식을 들고 와야 하는지, 스테이지 클리어 목표를 제시하는 타일입니다.
    // 플레이어는 손님 테이블 타일 위로 이동할 수 없으며, 이 타일에 들고 있는 음식을 내려놓을 수 있습니다.
    public class CustomerTile : BaseTile, IDropable
    {
        [SerializeField] private BaseFood desiredFood; // 목표 음식
        [SerializeField] private Image desiredFoodImage; // UI: Image; 목표 음식을 이미지로 보여주고자 할 경우

        #region Interface Methods
        
        // 이 함수는 플레이어가 호출합니다.
        public void Drop(BaseFood foodInHand)
        {
            // 플레이어가 내려놓는 음식이 목표 음식과 같을 경우,
            if (foodInHand.TypeName == desiredFood.TypeName)
            {
                // 게임의 결과를 관리하는 게임 매니저 등에게, 플레이어가 스테이지를 달성했음을 알립니다.
            }
        }

        // Queue<T>를 사용할 경우
        public void Drop(Queue<BaseFood> foodInHand)
        {
            if (foodInHand.Dequeue().TypeName == desiredFood.TypeName)
            {
                
            }
        }
        
        #endregion Interface Methods
    }
}

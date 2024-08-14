using System.Collections.Generic;
using CodeBlockSystem;
using ItemSystem;
using UnityEngine;

// 플레이어 시스템
namespace PlayerSystem
{
    // 플레이어 객체 클래스
    public class PlayerCharacter : MonoBehaviour
    {
        // 변수
        private List<BaseItem> _itemInHand; // 플레이어가 들고 있는 아이템(재료, 음식 등)
        
        // 함수
        private void ActByCode(BaseCodeBlock codeBlock)
        {
            
            
            switch (codeBlock)
            {
                case MoveForwardBlock:
                    transform.Translate(transform.position + Vector3.forward);
                    break;
                case TurnLeftBlock:
                    transform.Rotate(Vector3.left);
                    break;
            }
        }

        private void GetFrontTile()
        {
            
        }
    }
}

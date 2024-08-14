using System.Collections.Generic;
using FoodSystem;
using UnityEngine;

namespace TileSystem
{
    // 타일의 종류를 정의하는 열거형
    public enum TileType
    {
        Floor, // 바닥 타일: 플레이어가 이동할 수 있는 기본 타일
        Wall, // 벽 타일: 플레이어가 이동할 수 없는 기본 타일
        Destination, // 목적지 타일: 플레이어가 최종적으로 이동해야 하는 타일; 도착 시 스테이지를 클리어합니다.
        Serving, // 서빙 타일: 완성된 음식이 위치한 타일; 플레이어는 상호작용을 통해 음식을 들어올릴 수 있습니다.
        Customer, // 손님 타일: 손님이 위치한 타일; 플레이어는 상호작용을 통해 들어올린 음식을 내려놓을 수 있습니다.
        // Ingredient,
        // Kitchen,
    }
    
    // 타일 아이템 클래스의 최상위 클래스
    public class BaseTile : MonoBehaviour
    {
        // 타일의 종류(타입)
        [SerializeField] private TileType typeName;
        public TileType TypeName => typeName;
    }

    #region Interfaces
    
    // 이동할 수 있는 타일
    public interface IWalkable
    {
        bool Walk();
    }
    
    // (재료, 음식을) 들어1`2올릴 수 있는 타일
    public interface ILiftable
    {
        BaseFood Lift(); // (재료, 음식을) 들어올립니다.
        void Lift(Queue<BaseFood> inHand); // (재료, 음식을) 들어올립니다. (Queue<T>를 사용할 경우)
    }

    // (재료, 음식을) 내려놓을 수 있는 타일
    public interface IDropable
    {
        // [주의] 플레이어는 후입선출(LIFO; Last In, First Out) 방법으로 내려놓습니다.
        void Drop(BaseFood foodInHand); // (재료, 음식을) 내려놓습니다.
        void Drop(Queue<BaseFood> foodInHand); // (재료, 음식을) 내려놓습니다. (Queue<T>를 사용할 경우)
    }

    #endregion Interfaces
}

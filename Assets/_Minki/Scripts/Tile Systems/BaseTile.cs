using System.Collections.Generic;
using FoodSystem;
using UnityEngine;

namespace TileSystem
{
    // 타일 아이템 클래스의 최상위 클래스
    public class BaseTile : MonoBehaviour
    {
        // 타일의 종류
        public TileType TypeName { get; protected set; }
    }

    #region Interfaces
    
    // 이동할 수 있는 타일
    public interface IWalkable
    {
        bool Walk();
    }

    // 이동할 수 없는 타일
    public interface INotWalkable
    {
        bool Walk();
    }
    
    // (재료, 음식을) 들어올릴 수 있는 타일
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

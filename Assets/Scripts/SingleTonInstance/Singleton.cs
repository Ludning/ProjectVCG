using System;
using UnityEngine;

namespace Frameworks
{
    /// <summary>
    /// 싱글톤(Singleton) 일반화 클래스
    /// </summary>
    /// <typeparam name="T">상속받는 클래스</typeparam>
    public class Singleton<T> where T : new()
    {
        // Lazy<T>를 사용하여 스레드로부터 안전한, 게으른 생성을 사용한다.
        private static Lazy<T> _instance = new(new T());
        public static T Instance => _instance.Value;

        // 생성자를 private/protected으로 제한하여 생성자를 통한 객체의 생성을 막는다.
        protected Singleton() { }
        
        [RuntimeInitializeOnLoadMethod]
        private static void OnApplicationQuit()
        {
            _instance = null;
        }
    }
}

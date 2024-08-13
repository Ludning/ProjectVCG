using System;
using UnityEngine;
using System.Collections.Generic;

namespace Minki.Frameworks
{
    public class Singleton<T> where T : class
    {
        // Use Lazy<T> for Thread-Safe, Lazy Creation.
        private static readonly Lazy<Singleton<T>> _instance = new(() => new Singleton<T>());
        public static Singleton<T> Instance => _instance.Value;

        // Set Constructor to Private to Prevent from Creating Singleton by Constructor.
        private Singleton() { }
    }
}

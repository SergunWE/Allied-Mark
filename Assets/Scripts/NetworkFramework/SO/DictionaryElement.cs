using System;
using UnityEngine;

namespace NetworkFramework.SO
{
    /// <summary>
    /// A class for storing lobby data in ScriptableObject
    /// </summary>
    /// <typeparam name="T">Any</typeparam>
    [Serializable]
    public class DictionaryElement<T>
    {
        [SerializeField]
        public string key;
        [SerializeField]
        public T value;
    }
}
using System;
using UnityEngine;

namespace NetworkFramework.Data
{
    [Serializable]
    public class DictionaryElement<T>
    {
        [SerializeField]
        public string key;
        [SerializeField]
        public T value;
    }
}
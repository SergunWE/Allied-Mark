using System;
using UnityEngine;

namespace NetworkFramework.SO
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
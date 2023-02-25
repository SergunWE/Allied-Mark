using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace NetworkFramework.Data
{
    [Serializable]
    public class LobbyData<T>
    {
        [SerializeField]
        public string key;
        [SerializeField]
        public T value;
    }
}
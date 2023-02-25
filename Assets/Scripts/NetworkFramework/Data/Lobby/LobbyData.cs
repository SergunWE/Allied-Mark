using System;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace NetworkFramework.Data
{
    [CreateAssetMenu(menuName = "Lobby/Lobby Data")]
    public class LobbyData : ScriptableObject
    {
        [SerializeField]
        private List<DictionaryElement<LobbyDictionaryElement>> playerData;

        private Dictionary<string, DataObject> _dictionary;

        private void OnValidate()
        {
            InitDictionary();
        }

        private void OnEnable()
        {
            InitDictionary();
        }

        private void InitDictionary()
        {
            if(playerData == null || playerData.Count == 0) return;
            _dictionary ??= new Dictionary<string, DataObject>();
            foreach (var data in playerData)
            {
                if (_dictionary.ContainsKey(data.key)) continue;
                var value = data.value;
                _dictionary.Add(data.key, new DataObject(value.visibility, value.value));
            }
        }

        public Dictionary<string, DataObject> GetDictionary => _dictionary;
    }

    [Serializable]
    public class LobbyDictionaryElement
    {
        [SerializeField]
        public string value;
        [SerializeField]
        public DataObject.VisibilityOptions visibility;
    }
}
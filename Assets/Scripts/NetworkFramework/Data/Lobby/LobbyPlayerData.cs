using System;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace NetworkFramework.Data
{
    [CreateAssetMenu(menuName = "Lobby/Lobby Player Data")]
    public class LobbyPlayerData : ScriptableObject
    {
        [SerializeField]
        private List<LobbyData<PlayerData>> playerData;

        private Dictionary<string, PlayerDataObject> _dictionary;

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
            _dictionary ??= new Dictionary<string, PlayerDataObject>();
            foreach (var data in playerData)
            {
                if (_dictionary.ContainsKey(data.key)) continue;
                var value = data.value;
                _dictionary.Add(data.key, new PlayerDataObject(value.visibility, value.value));
            }
        }

        public Dictionary<string, PlayerDataObject> GetDictionary => _dictionary;
    }

    [Serializable]
    public class PlayerData
    {
        [SerializeField]
        public string value;
        [SerializeField]
        public PlayerDataObject.VisibilityOptions visibility;
    }
}
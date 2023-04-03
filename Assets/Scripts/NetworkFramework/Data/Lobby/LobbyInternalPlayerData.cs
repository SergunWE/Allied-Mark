using System;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NetworkFramework.Data
{
    [CreateAssetMenu(menuName = "Lobby/Lobby Player Data")]
    public class LobbyInternalPlayerData : InternalData<PlayerDataObject, PlayerDictionaryElement>
    {
        // ReSharper disable once InconsistentNaming
        [SerializeField] private string _playerName;
        public string PlayerName
        {
            get
            {
                if (string.IsNullOrEmpty(_playerName))
                {
                    _playerName = $"Player{Random.Range(0, 10000)}";
                }

                return _playerName;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value == _playerName) return;
                _playerName = value;
                AddElement(DataKeysConstants.PlayerName.Key,
                    new PlayerDataObject(DataKeysConstants.PlayerName.Visibility, PlayerName));
                onDataUpdated.Raise();
            }
        }

        // ReSharper disable once InconsistentNaming
        [SerializeField] private bool _playerReady;
        public bool PlayerReady
        {
            get => _playerReady;
            set
            {
                if (value == _playerReady) return;
                _playerReady = value;
                AddElement(DataKeysConstants.PlayerReady.Key,
                    new PlayerDataObject(DataKeysConstants.PlayerReady.Visibility, _playerReady.ToString()));
                onDataUpdated.Raise();
            }
        }

        private void Awake()
        {
            _playerName = "";
            _playerReady = false;
        }

        private void OnEnable()
        {
            #if UNITY_EDITOR
            Awake();
            #endif
        }

        protected override void UpdateBasicData()
        {
            AddElement(DataKeysConstants.PlayerName.Key,
                new PlayerDataObject(DataKeysConstants.PlayerName.Visibility, PlayerName));
            AddElement(DataKeysConstants.PlayerReady.Key,
                new PlayerDataObject(DataKeysConstants.PlayerReady.Visibility, _playerReady.ToString()));
        }

        public override void AddCustomElement(string key, PlayerDictionaryElement element)
        {
            Dictionary[key] = new PlayerDataObject(element.visibility, element.value);
            onDataUpdated.Raise();
        }
    }

    [Serializable]
    public class PlayerDictionaryElement
    {
        [SerializeField] public string value;
        [SerializeField] public PlayerDataObject.VisibilityOptions visibility;
    }
}
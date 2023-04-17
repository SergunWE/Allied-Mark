using System;
using NetworkFramework.EventSystem.EventParameter;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace NetworkFramework.SO
{
    /// <summary>
    /// ScriptableObject lobby player data
    /// </summary>
    [CreateAssetMenu(menuName = "Lobby/Lobby Player Data")]
    public class LobbyInternalPlayerData : InternalData<PlayerDataObject, PlayerDictionaryElement>
    {
        [SerializeField] private GameEventBool playerHostChanged;
        
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
                AddElement(DataKeys.PlayerName.Key,
                    new PlayerDataObject(DataKeys.PlayerName.Visibility, PlayerName));
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
                AddElement(DataKeys.PlayerReady.Key,
                    new PlayerDataObject(DataKeys.PlayerReady.Visibility, _playerReady.ToString()));
                onDataUpdated.Raise();
            }
        }

        // ReSharper disable once InconsistentNaming
        [SerializeField] private bool _playerHost;
        public bool PlayerHost
        {
            get => _playerHost;
            set
            {
                if (_playerHost == value) return;
                _playerHost = value;
                playerHostChanged.Raise(value);
            }
        }

        private void Awake()
        {
            _playerName = "";
            _playerReady = false;
        }

        protected override void UpdateBasicData()
        {
            #if UNITY_EDITOR
            Awake();
            #endif
            AddElement(DataKeys.PlayerName.Key,
                new PlayerDataObject(DataKeys.PlayerName.Visibility, PlayerName));
            AddElement(DataKeys.PlayerReady.Key,
                new PlayerDataObject(DataKeys.PlayerReady.Visibility, _playerReady.ToString()));
        }

        public override void AddCustomElement(string key, PlayerDictionaryElement element)
        {
            Dictionary[key] = new PlayerDataObject(element.visibility, element.startValue);
            onDataUpdated.Raise();
        }
    }

    [Serializable]
    public class PlayerDictionaryElement
    {
        [FormerlySerializedAs("value")] [SerializeField] public string startValue;
        [SerializeField] public PlayerDataObject.VisibilityOptions visibility;
    }
}
using NetworkFramework.Data;
using NetworkFramework.EventSystem.EventParameter;
using NetworkFramework.LobbyCore;
using NetworkFramework.SO;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class LobbyRefresherComponent : MonoBehaviour
    {
        [SerializeField] private GameEvent lobbyRefreshed;
        [SerializeField] private GameEvent lobbyLeaved;
        [SerializeField] private GameEventBool lobbyHostChanged;

        [SerializeField] private LobbyInternalPlayerData lobbyInternalPlayerData;
        
        private LobbyRefresherCore _core;
        private bool _isUpdate = true;

        private void Awake()
        {
            _core ??= new LobbyRefresherCore();
            lobbyHostChanged.Raise(_core.PlayerIsHost);
        }

        private void FixedUpdate()
        {
            if (!_isUpdate) return;
            if (LobbyData.Exist)
            {
                lobbyRefreshed.Raise();
                lobbyInternalPlayerData.PlayerHost = _core.PlayerIsHost;
            }
            _isUpdate = false;
        }

        private void OnEnable()
        {
            _core.OnLobbyUpdated += OnLobbyUpdated;
            _core.StartHeartbeat();
            _core.StartRefresh();
        }

        private void OnDisable()
        {
            _core.OnLobbyUpdated -= OnLobbyUpdated;
            _core.StopUpdatingLobby();
        }
        
        public async void LeaveLobby()
        {
            if ((await _core.LeaveLobbyAsync()).Success)
            {
                lobbyLeaved.Raise();
            }
        }

        private void OnLobbyUpdated()
        {
            _isUpdate = true;
        }
    }
}
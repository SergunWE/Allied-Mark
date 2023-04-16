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

		//wait for another thread to change the variable
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

		//add a listener
        private void OnEnable()
        {
            _core.OnLobbyUpdated += OnLobbyUpdated;
            _core.StartHeartbeat();
            _core.StartRefresh();
        }

		//remove the listener
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

		//The core level thread changes the variable when the event is triggered
        private void OnLobbyUpdated()
        {
            _isUpdate = true;
        }
    }
}
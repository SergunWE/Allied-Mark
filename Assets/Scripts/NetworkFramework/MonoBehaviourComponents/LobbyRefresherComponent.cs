using NetworkFramework.Data;
using NetworkFramework.EventSystem.EventParameter;
using NetworkFramework.LobbyCore;
using NetworkFramework.SO;
using UnityEngine;

namespace NetworkFramework.MonoBehaviourComponents
{
    /// <summary>
    /// Lobby data update component
    /// </summary>
    public class LobbyRefresherComponent : MonoBehaviour
    {
        /// <summary>
        /// Lobby update event
        /// </summary>
        [SerializeField] private GameEvent lobbyRefreshed;
        /// <summary>
        /// Exit lobby event
        /// </summary>
        [SerializeField] private GameEvent lobbyLeaved;
        /// <summary>
        /// Host change event
        /// </summary>
        [SerializeField] private GameEventBool lobbyHostChanged;

        [SerializeField] private LobbyInternalPlayerData lobbyInternalPlayerData;
        
        private LobbyRefresherCore _core;
        private bool _isUpdate = true;

        private void Awake()
        {
            _core ??= new LobbyRefresherCore();
            lobbyHostChanged.Raise(_core.PlayerIsHost);
        }
        
        /// <summary>
        /// <remarks>Wait for another thread to change the variable</remarks>
        /// </summary>
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

        /// <summary>
        /// <remarks>Add a listener</remarks>
        /// </summary>
        private void OnEnable()
        {
            _core.OnLobbyUpdated += OnLobbyUpdated;
            _core.StartHeartbeat();
            _core.StartRefresh();
        }
        
        /// <summary>
        /// <remarks>Remove the listener</remarks>
        /// </summary>
        private void OnDisable()
        {
            _core.OnLobbyUpdated -= OnLobbyUpdated;
            _core.StopUpdatingLobby();
        }
        
        /// <summary>
        /// Exiting the lobby
        /// </summary>
        public async void LeaveLobby()
        {
            if ((await _core.LeaveLobbyAsync()).Success)
            {
                lobbyLeaved.Raise();
            }
        }
        
        /// <summary>
        /// <remarks>The core level thread changes the variable when the event is triggered</remarks>
        /// </summary>
        private void OnLobbyUpdated()
        {
            _isUpdate = true;
        }
    }
}
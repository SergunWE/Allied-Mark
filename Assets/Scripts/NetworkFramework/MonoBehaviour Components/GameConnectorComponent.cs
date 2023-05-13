using NetworkFramework.Data;
using NetworkFramework.Data.Scene;
using NetworkFramework.EventSystem.EventParameter;
using NetworkFramework.LobbyCore;
using NetworkFramework.NetcodeCore;
using NetworkFramework.RelayCore;
using NetworkFramework.SO;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    /// <summary>
    /// Component of connecting to the game
    /// </summary>
    public class GameConnectorComponent : MonoBehaviour
    {
        /// <summary>
        /// Player readiness check component
        /// </summary>
        [SerializeField] private PlayersReadyChecker readyChecker;

        /// <summary>
        /// Relay server start event
        /// </summary>
        [SerializeField] private GameEventBool relayStarted;

        /// <summary>
        /// The event of the start of the game
        /// </summary>
        [SerializeField] private GameEvent gameStarted;

        /// <summary>
        /// Lobby settings
        /// </summary>
        [SerializeField] private LobbyOptions lobbyOptions;

        [SerializeField] private LobbyInternalData lobbyInternalData;

        private LobbyRefresherCore _refreshCore;
        private RelayConnectorCore _relayCore;
        private LobbyDataUpdaterCore _dataUpdaterCore;
        private NetcodeConnectorCore _netcodeConnectorCore;

        private bool _playerStartedGame;

        private void Awake()
        {
            _refreshCore ??= new LobbyRefresherCore();
            _relayCore ??= new RelayConnectorCore();
            _dataUpdaterCore ??= new LobbyDataUpdaterCore(false);
        }

        private void Start()
        {
            _netcodeConnectorCore ??= new NetcodeConnectorCore();
        }

        /// <summary>
        /// Creating a Relay Server
        /// <remarks>
        /// Only the lobby owner can create a server
        /// </remarks>
        /// </summary>
        public async void CreateRelay()
        {
            if (!readyChecker.PlayerReady() || LobbyData.GetLobbyData(DataKeys.RelayCode.Key) != null) return;
            if ((await _relayCore.CreateRelay(lobbyOptions.MaxPlayer)).Success)
            {
                relayStarted.Raise((await _dataUpdaterCore.UpdateLobbyData(DataKeys.RelayCode,
                    _relayCore.JoinCode)).Success);
            }
        }

        /// <summary>
        /// Actions when a lobby update event is triggered
        /// </summary>
        public void OnLobbyRefreshed()
        {
            if (!_playerStartedGame && LobbyData.GetLobbyData(DataKeys.RelayCode.Key) != null &&
                !_refreshCore.PlayerIsHost)
            {
                JoinRelay();
            }
        }

        /// <summary>
        /// Actions when a Relay server start event is triggered
        /// </summary>
        /// <param name="localStatus">Relay server status, whether it is running</param>
        public void OnRelayStarted(bool localStatus)
        {
            if (!localStatus) return;
            TaskResult taskResult;
            if (_refreshCore.PlayerIsHost)
            {
                taskResult = _netcodeConnectorCore.StartHost(_relayCore.RelayServerData);
                if (taskResult.Success) _netcodeConnectorCore.LoadNetworkScene(new SceneInfo( 3 + lobbyInternalData.Level));
            }
            else
            {
                taskResult = _netcodeConnectorCore.StartClient(_relayCore.RelayServerData);
            }

            if (taskResult.Success)
            {
                gameStarted.Raise();
            }
        }

        /// <summary>
        /// Connecting to a Relay server by clients
        /// </summary>
        private async void JoinRelay()
        {
            _playerStartedGame = (await _relayCore.JoinRelay(
                LobbyData.GetLobbyData(DataKeys.RelayCode.Key))).Success;
            relayStarted.Raise(_playerStartedGame);
        }
    }
}
using System;
using NetworkFramework.Data;
using NetworkFramework.EventSystem.EventParameter;
using NetworkFramework.LobbyCore;
using NetworkFramework.NetcodeCore;
using NetworkFramework.RelayCore;
using NetworkFramework.SO;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class GameConnectorComponent : MonoBehaviour
    {
        [SerializeField] private PlayersReadyChecker readyChecker;
        [SerializeField] private GameEventBool relayStarted;
        [SerializeField] private GameEvent gameStarted;

        [SerializeField] private LobbyOptions lobbyOptions;

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
            _netcodeConnectorCore = new NetcodeConnectorCore();
        }

        public async void CreateRelay()
        {
            if (!readyChecker.PlayerReady() || LobbyData.GetLobbyData(DataKeys.RelayCode.Key) != null) return;
            if ((await _relayCore.CreateRelay(lobbyOptions.MaxPlayer)).Success)
            {
                relayStarted.Raise((await _dataUpdaterCore.UpdateLobbyData(DataKeys.RelayCode,
                    _relayCore.JoinCode)).Success);
            }
        }

        public void OnLobbyRefreshed()
        {
            if (!_playerStartedGame && LobbyData.GetLobbyData(DataKeys.RelayCode.Key) != null &&
                !_refreshCore.PlayerIsHost)
            {
                JoinRelay();
            }
        }

        public void OnRelayStarted(bool localStatus)
        {
            if (!localStatus) return;
            var taskStatus = _refreshCore.PlayerIsHost
                ? _netcodeConnectorCore.StartHost(_relayCore.RelayServerData)
                : _netcodeConnectorCore.StartClient(_relayCore.RelayServerData);
            if (taskStatus.Success)
            {
                gameStarted.Raise();
            }
        }

        private async void JoinRelay()
        {
            _playerStartedGame = (await _relayCore.JoinRelay(
                LobbyData.GetLobbyData(DataKeys.RelayCode.Key))).Success;
            relayStarted.Raise(_playerStartedGame);
        }
    }
}
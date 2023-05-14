using NetworkFramework.LobbyCore;
using NetworkFramework.SO;
using UnityEngine;

namespace NetworkFramework.MonoBehaviourComponents
{
    /// <summary>
    /// Component of sending data to the lobby
    /// </summary>
    public class LobbyUpdaterComponent : MonoBehaviour
    {
        /// <summary>
        /// Lobby player data
        /// </summary>
        [SerializeField] private LobbyInternalPlayerData internalPlayerData;

        private LobbyDataUpdaterCore _core;

        private void Awake()
        {
            _core ??= new LobbyDataUpdaterCore(true);
        }

        /// <summary>
        /// Changing player ready
        /// </summary>
        public async void ChangePlayerReady()
        {
            // Variant without automatic checking
            // if (!_core.PlayerDataEqual(DataKeysConstants.PlayerReady.Key, internalPlayerData.PlayerReady)) return;
            // internalPlayerData.PlayerReady = !internalPlayerData.PlayerReady;
            // await _core.UpdatePlayerData(DataKeysConstants.PlayerReady, internalPlayerData.PlayerReady);

            if ((await _core.UpdatePlayerData(DataKeys.PlayerReady, !internalPlayerData.PlayerReady)).Success)
            {
                internalPlayerData.PlayerReady = !internalPlayerData.PlayerReady;
            }
        }
    }
}
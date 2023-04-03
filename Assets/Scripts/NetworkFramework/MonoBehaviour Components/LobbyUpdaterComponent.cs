using System;
using NetworkFramework.Data;
using NetworkFramework.LobbyCore;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class LobbyUpdaterComponent : MonoBehaviour
    {
        [SerializeField] private LobbyInternalPlayerData internalPlayerData;
        
        private LobbyDataUpdaterCore _core;
        

        private void Awake()
        {
            _core ??= new LobbyDataUpdaterCore(true);
        }

        public async void ChangePlayerReady()
        {
            // Variant without automatic checking
            // if (!_core.PlayerDataEqual(DataKeysConstants.PlayerReady.Key, internalPlayerData.PlayerReady)) return;
            // internalPlayerData.PlayerReady = !internalPlayerData.PlayerReady;
            // await _core.UpdatePlayerData(DataKeysConstants.PlayerReady, internalPlayerData.PlayerReady);

            if ((await _core.UpdatePlayerData(DataKeysConstants.PlayerReady, !internalPlayerData.PlayerReady)).Success)
            {
                internalPlayerData.PlayerReady = !internalPlayerData.PlayerReady;
            }
        }
    }
}
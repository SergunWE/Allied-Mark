using NetworkFramework.Data;
using NetworkFramework.LobbyCore;
using NetworkFramework.SO;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class LobbyUpdaterComponent : MonoBehaviour
    {
        [SerializeField] private LobbyInternalPlayerData internalPlayerData;
        
        private LobbyDataUpdaterCore _core;

        //custom lobby data
        private static readonly LobbyDataInfo<DataObject.VisibilityOptions> LevelDifficulty =
            new("LevelDifficulty", DataObject.VisibilityOptions.Public);
        
        //custom player data
        private static readonly LobbyDataInfo<PlayerDataObject.VisibilityOptions> PlayerClass =
            new("PlayerClass", PlayerDataObject.VisibilityOptions.Member);

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

        public async void ChangeLevelDifficulty(LevelDifficulty difficulty)
        {
            await _core.UpdateLobbyData(LevelDifficulty, difficulty.DifficultName);
        }

        public async void ChangePlayerClass(PlayerClass @class)
        {
            await _core.UpdatePlayerData(PlayerClass, @class.ClassName);
        }
    }
}
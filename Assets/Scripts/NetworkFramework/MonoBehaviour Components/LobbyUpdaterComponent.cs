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
        public LobbyDataInfo<DataObject.VisibilityOptions> LevelDifficulty { get; private set; }

        //custom player data
        public LobbyDataInfo<PlayerDataObject.VisibilityOptions> PlayerClass { get; private set; }

        private void Awake()
        {
            _core ??= new LobbyDataUpdaterCore(true);
            LevelDifficulty = new LobbyDataInfo<DataObject.VisibilityOptions>("LevelDifficulty",
                DataObject.VisibilityOptions.Public);
            PlayerClass = new LobbyDataInfo<PlayerDataObject.VisibilityOptions>("PlayerClass", 
                PlayerDataObject.VisibilityOptions.Member);
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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkFramework.Data;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace NetworkFramework.LobbyCore
{
    /// <summary>
    /// A class for sending lobby data
    /// </summary>
    public class LobbyDataUpdaterCore : LobbyCore
    {
        /// <summary>
        /// Flag indicating whether the data should be validated before checking
        /// </summary>
        private readonly bool _checkData;
        
        /// <param name="checkData">Whether you need to check the input data</param>
        public LobbyDataUpdaterCore(bool checkData)
        {
            _checkData = checkData;
        }

        /// <summary>
        /// Function to send lobby data. Only the host can send data
        /// </summary>
        /// <param name="info">Lobby data information</param>
        /// <param name="newValue">Data value</param>
        /// <returns>Task with its progress status</returns>
        public async Task<TaskResult> UpdateLobbyData(LobbyDataInfo<DataObject.VisibilityOptions> info, object newValue)
        {
            if (_checkData && LobbyDataEqual(info.Key, newValue))
                return new TaskResult(false, new Exception("The data is equal, cannot update"));
            try
            {
                await LobbyService.Instance.UpdateLobbyAsync(CurrentLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>()
                    {
                        {info.Key, new DataObject(info.Visibility, newValue.ToString())}
                    }
                });
                return TaskResult.Ok;
            }
            catch (LobbyServiceException e)
            {
                Console.WriteLine(e);
                return new TaskResult(false, e);
            }
        }

        /// <summary>
        /// Function to send player data
        /// </summary>
        /// <param name="info">Player data information</param>
        /// <param name="newValue">Data value</param>
        /// <returns>Task with its progress status</returns>
        public async Task<TaskResult> UpdatePlayerData(LobbyDataInfo<PlayerDataObject.VisibilityOptions> info,
            object newValue)
        {
            if (_checkData && PlayerDataEqual(info.Key, newValue))
                return new TaskResult(false, new Exception("The data is equal, cannot update"));
            try
            {
                await LobbyService.Instance.UpdatePlayerAsync(CurrentLobby.Id, AuthenticationService.Instance.PlayerId,
                    new UpdatePlayerOptions
                    {
                        Data = new Dictionary<string, PlayerDataObject>()
                        {
                            {info.Key, new PlayerDataObject(info.Visibility, newValue.ToString())}
                        }
                    });
                return TaskResult.Ok;
            }
            catch (LobbyServiceException e)
            {
                Console.WriteLine(e);
                return new TaskResult(false, e);
            }
        }

        /// <summary>
        /// Function to check the synchronization of lobby data
        /// </summary>
        /// <param name="key">Dictionary key</param>
        /// <param name="value">Current value</param>
        /// <returns></returns>
        public bool LobbyDataEqual(string key, object value)
        {
            var data = CurrentLobby.Data;
            if (data == null) return false;
            return data.ContainsKey(key) && data[key].Value == value.ToString();
        }

        /// <summary>
        /// Function to check the synchronization of player data
        /// </summary>
        /// <param name="key">Dictionary key</param>
        /// <param name="value">Current value</param>
        /// <returns></returns>
        public bool PlayerDataEqual(string key, object value)
        {
            var data = CurrentLobby.Players.Find(player =>
                player.Id == AuthenticationService.Instance.PlayerId).Data;
            if (data == null) return false;
            return data.ContainsKey(key) && data[key].Value == value.ToString();
        }
        
        public override void Dispose()
        {
        }
    }
}
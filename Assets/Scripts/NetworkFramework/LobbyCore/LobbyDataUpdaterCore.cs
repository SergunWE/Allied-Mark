using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkFramework.Data;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace NetworkFramework.LobbyCore
{
    public class LobbyDataUpdaterCore : LobbyCore
    {
        private readonly bool _checkData;

        public LobbyDataUpdaterCore(bool checkData)
        {
            _checkData = checkData;
        }

        public async Task<TaskStatus> UpdateLobbyData(LobbyDataInfo<DataObject.VisibilityOptions> info, object newValue)
        {
            if (_checkData && LobbyDataEqual(info.Key, newValue))
                return new TaskStatus(false, new Exception("The data is equal, cannot update"));
            try
            {
                await LobbyService.Instance.UpdateLobbyAsync(CurrentLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>()
                    {
                        {info.Key, new DataObject(info.Visibility, newValue.ToString())}
                    }
                });
                return new TaskStatus(true);
            }
            catch (LobbyServiceException e)
            {
                Console.WriteLine(e);
                return new TaskStatus(false, e);
            }
        }

        public async Task<TaskStatus> UpdatePlayerData(LobbyDataInfo<PlayerDataObject.VisibilityOptions> info,
            object newValue)
        {
            if (_checkData && PlayerDataEqual(info.Key, newValue))
                return new TaskStatus(false, new Exception("The data is equal, cannot update"));
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
                return new TaskStatus(true);
            }
            catch (LobbyServiceException e)
            {
                Console.WriteLine(e);
                return new TaskStatus(false, e);
            }
        }

        public bool LobbyDataEqual(string key, object value)
        {
            var data = CurrentLobby.Data;
            if (data == null) return false;
            return data.ContainsKey(key) && data[key].Value == value.ToString();
        }

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
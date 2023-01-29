using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetworkFramework;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager
{
    private static Lobby _lobby;
    private Thread _heartbeatLobbyThread;
    private Thread _refreshLobbyThread;

    public LobbyManager()
    {
    }

    public async Task<bool> CreateLobbyAsync(int maxPlayers, bool isPrivate, Dictionary<string, string> data)
    {
        if (!AuthenticationService.Instance.IsSignedIn) return false;
        Player player = new Player(AuthenticationService.Instance.PlayerId);

        string lobbyName = "new lobby";
        CreateLobbyOptions options = new CreateLobbyOptions
        {
            Player = player,
            IsPrivate = isPrivate
        };

        try
        {
            _lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
        }
        catch (Exception)
        {
            return false;
        }
        
        _heartbeatLobbyThread?.Abort();
        _heartbeatLobbyThread = new Thread(HeartbeatLobbyThread);
        _heartbeatLobbyThread.Start();
            
        _refreshLobbyThread?.Abort();
        _refreshLobbyThread = new Thread(RefreshLobbyThread);
        _refreshLobbyThread.Start();

        Debug.Log($"Lobby Id: {_lobby.Id}");
        Debug.Log($"Lobby Code: {_lobby.LobbyCode}");
        return true;
    }

    private static void HeartbeatLobbyThread()
    {
        while (true)
        {
            try
            {
                LobbyService.Instance.SendHeartbeatPingAsync(_lobby.Id);
                Thread.Sleep(GlobalConstants.HeartbeatLobbyDelayMilliseconds);
            }
            catch (Exception e)
            {
                Debug.Log($"The HeartbeatLobby thread ended with an error - {e.Message}");
                return;
            }
        }
    }

    private static void RefreshLobbyThread()
    {
        while (true)
        {
            try
            {
                Task<Lobby> task = LobbyService.Instance.GetLobbyAsync(_lobby.Id);
                task.Wait();
                Lobby newLobby = task.Result;
                if (_lobby.LastUpdated < newLobby.LastUpdated)
                {
                    _lobby = newLobby;
                }

                Thread.Sleep(GlobalConstants.RefreshLobbyDelayMilliseconds);
            }
            catch (Exception e)
            {
                Debug.Log($"The RefreshLobby thread ended with an error - {e.Message}");
                return;
            }
        }
    }
}
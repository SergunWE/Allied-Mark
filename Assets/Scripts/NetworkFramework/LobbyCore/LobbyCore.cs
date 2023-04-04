using System;
using NetworkFramework.Data;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;

namespace NetworkFramework.LobbyCore
{
    public abstract class LobbyCore : IDisposable
    {
        protected static Lobby CurrentLobby
        {
            get => LobbyData.Current;
            set => LobbyData.Current = value;
        }

        public virtual bool PlayerIsHost => CurrentLobby.HostId == AuthenticationService.Instance.PlayerId;

        public abstract void Dispose();
    }
}
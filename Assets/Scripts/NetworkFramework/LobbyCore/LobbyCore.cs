using System;
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

        public abstract void Dispose();
    }
}
using System;
using NetworkFramework.Data;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;

namespace NetworkFramework.LobbyCore
{
    /// <summary>
    /// A basic class for working with lobbies
    /// </summary>
    public abstract class LobbyCore : IDisposable
    {
        /// <summary>
        /// Current Lobby
        /// </summary>
        protected static Lobby CurrentLobby
        {
            get => LobbyData.Current;
            set => LobbyData.Current = value;
        }

        /// <summary>
        /// Whether the current user is a lobby host
        /// </summary>
        public virtual bool PlayerIsHost => CurrentLobby.HostId == AuthenticationService.Instance.PlayerId;

        /// <summary>
        /// Release of resources
        /// </summary>
        public abstract void Dispose();
    }
}
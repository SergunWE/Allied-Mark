namespace NetworkFramework
{
    /// <summary>
    /// Global variables used in the framework
    /// </summary>
    public static class GlobalConstants
    {
        /// <value>
        /// Heartbeat interval sent to the lobby by the host
        /// </value>
        public const int HeartbeatLobbyDelayMilliseconds = 20000;
        /// <value>
        /// Lobby data update interval
        /// </value>
        public const int RefreshLobbyDelayMilliseconds = 3000;
        /// <value>
        /// Maximum number of players in the created lobby
        /// </value>
        public const int MaxPlayersLobby = 2;
    }
}
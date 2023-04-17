using System;

namespace NetworkFramework.Data
{
    /// <summary>
    /// Structure for storing lobby data information
    /// </summary>
    /// <typeparam name="T">The enumeration that defines the visibility of the data</typeparam>
    public struct LobbyDataInfo<T> where T : Enum
    {
        /// <summary>
        /// Dictionary key to access the value
        /// </summary>
        public readonly string Key;
        /// <summary>
        /// Enumeration to set data visibility
        /// </summary>
        public readonly T Visibility;
        
        /// <param name="key">Dictionary key</param>
        /// <param name="visibility">Data visibility enumeration</param>
        public LobbyDataInfo(string key, T visibility)
        {
            Key = key;
            Visibility = visibility;
        }
    }
}
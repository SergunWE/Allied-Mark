namespace NetworkFramework.Data
{
    public struct LobbyData<T>
    {
        public readonly string Key;
        public readonly T Visibility;
        
        public LobbyData(string key, T visibility)
        {
            Key = key;
            Visibility = visibility;
        }
    }
}
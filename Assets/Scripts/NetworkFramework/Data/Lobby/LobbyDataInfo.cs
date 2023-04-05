namespace NetworkFramework.Data
{
    public struct LobbyDataInfo<T>
    {
        public readonly string Key;
        public readonly T Visibility;

        public LobbyDataInfo(string key, T visibility)
        {
            Key = key;
            Visibility = visibility;
        }
    }
}
namespace NetworkFramework.Data
{
    public struct LobbyDataInfo<T>
    {
        public readonly string Key;
        public readonly T Visibility;
        public  readonly string DefaultValue;
        
        public LobbyDataInfo(string key, T visibility, string defaultValue = null)
        {
            Key = key;
            Visibility = visibility;
            DefaultValue = defaultValue;
        }
    }
}
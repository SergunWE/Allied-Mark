using System;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetworkFramework.Data
{
    [CreateAssetMenu(menuName = "Lobby/Lobby Data")]
    public class LobbyInternalData : InternalData<DataObject, LobbyDictionaryElement>
    {
        [SerializeField] private int lobbyLevel;
        protected override void UpdateBasicData()
        {
            AddElement(DataKeysConstants.LobbyLevel.Key,
                new DataObject(DataKeysConstants.LobbyLevel.Visibility, lobbyLevel.ToString()));
        }

        public override void AddCustomElement(string key, LobbyDictionaryElement element)
        {
            Dictionary.Add(key, new DataObject(element.visibility, element.value));
            onDataUpdated.Raise();
        }

        public void SetLevel(int level)
        {
            if (level == lobbyLevel) return;
            AddElement(DataKeysConstants.LobbyLevel.Key,
                new DataObject(DataKeysConstants.LobbyLevel.Visibility, lobbyLevel.ToString()));
            lobbyLevel = level;
            onDataUpdated.Raise();
        }
    }

    [Serializable]
    public class LobbyDictionaryElement
    {
        [SerializeField] public string value;
        [SerializeField] public DataObject.VisibilityOptions visibility;
    }
}
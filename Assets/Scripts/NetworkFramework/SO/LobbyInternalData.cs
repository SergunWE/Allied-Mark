using System;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace NetworkFramework.SO
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
            Dictionary.Add(key, new DataObject(element.visibility, element.startValue));
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
        [FormerlySerializedAs("value")] [SerializeField] public string startValue;
        [SerializeField] public DataObject.VisibilityOptions visibility;
    }
}
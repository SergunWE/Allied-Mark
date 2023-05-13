using System;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace NetworkFramework.SO
{
    /// <summary>
    /// ScriptableObject lobby data
    /// </summary>
    [CreateAssetMenu(menuName = "Lobby/Lobby Data")]
    public class LobbyInternalData : InternalData<DataObject, LobbyDictionaryElement>
    {
        [SerializeField] private int lobbyLevel;
        protected override void UpdateBasicData()
        {
            AddElement(DataKeys.LobbyLevel.Key,
                new DataObject(DataKeys.LobbyLevel.Visibility, lobbyLevel.ToString()));
            AddElement(DataKeys.RelayCode.Key, new DataObject(DataKeys.RelayCode.Visibility));
        }

        public override void AddCustomElement(string key, LobbyDictionaryElement element)
        {
            Dictionary.Add(key, new DataObject(element.visibility, element.startValue));
            onDataUpdated.Raise();
        }

        public void SetLevel(int level)
        {
            if (level == lobbyLevel) return;
            AddElement(DataKeys.LobbyLevel.Key,
                new DataObject(DataKeys.LobbyLevel.Visibility, lobbyLevel.ToString()));
            lobbyLevel = level;
            onDataUpdated.Raise();
        }

        public int Level => lobbyLevel;
    }

    [Serializable]
    public class LobbyDictionaryElement
    {
        [FormerlySerializedAs("value")] [SerializeField] public string startValue;
        [SerializeField] public DataObject.VisibilityOptions visibility;
    }
}
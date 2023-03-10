using System;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetworkFramework.Data
{
    [CreateAssetMenu(menuName = "Lobby/Lobby Player Data")]
    public class LobbyInternalPlayerData : InternalData<PlayerDataObject, PlayerDictionaryElement>
    {
        [SerializeField] private string playerName = "";
        [SerializeField] private bool playerReady = false;

        protected override void UpdateBasicData()
        {
            AddElement(DataKeys.PlayerName, 
                new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName));
            AddElement(DataKeys.PlayerReady, 
                new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerReady.ToString()));
        }

        public override void AddCustomElement(string key, PlayerDictionaryElement element)
        {
            Dictionary[key] = new PlayerDataObject(element.visibility, element.value);
            onDataUpdated.Raise();
        }

        public void SetName(string newName)
        {
            if (newName == null || newName == playerName) return;
            playerName = newName;
            AddElement(DataKeys.PlayerName, 
                new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName));
            onDataUpdated.Raise();
        }

        public void SetReady(bool ready)
        {
            if(ready == playerReady) return;
            playerReady = ready;
            AddElement(DataKeys.PlayerReady, 
                new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerReady.ToString()));
            onDataUpdated.Raise();
        }
    }

    [Serializable]
    public class PlayerDictionaryElement
    {
        [SerializeField]
        public string value;
        [SerializeField]
        public PlayerDataObject.VisibilityOptions visibility;
    }
}
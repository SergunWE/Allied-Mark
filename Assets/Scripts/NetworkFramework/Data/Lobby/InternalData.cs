using System.Collections.Generic;
using NetworkFramework.EventSystem.Event;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetworkFramework.Data
{
    public abstract class InternalData<TDict, TCs> : ScriptableObject
    {
        [SerializeField] protected GameEvent onDataUpdated;
        [SerializeField]
        private List<DictionaryElement<TCs>> customData;
        protected Dictionary<string, TDict> Dictionary;
        
        private void OnValidate()
        {
            InitDictionary();
        }

        private void OnEnable()
        {
            InitDictionary();
        }

        private void InitDictionary()
        {
            if(Dictionary != null) return;
            Dictionary ??= new Dictionary<string, TDict>();

            UpdateBasicData();

            foreach (var data in customData)
            {
                if (Dictionary.ContainsKey(data.key)) continue;
                var value = data.value;
                AddCustomElement(data.key, value);
            }
        }
        
        protected void AddElement(string key, TDict element)
        {
            Dictionary.Add(key, element);
        }
        
        protected abstract void UpdateBasicData();
        public abstract void AddCustomElement(string key, TCs element);
        public Dictionary<string, TDict> GetDictionary => Dictionary;
    }
}
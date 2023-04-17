using System.Collections.Generic;
using UnityEngine;

namespace NetworkFramework.SO
{
    /// <summary>
    /// Base class for ScriptableObject data lobby
    /// </summary>
    /// <typeparam name="TDict">Structure for storing data in the lobby</typeparam>
    /// <typeparam name="TCs"><see cref="DictionaryElement{TCs}"/></typeparam>
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
            Dictionary[key] =  element;
        }
        
        protected abstract void UpdateBasicData();
        public abstract void AddCustomElement(string key, TCs element);
        public Dictionary<string, TDict> GetDictionary => Dictionary;
    }
}
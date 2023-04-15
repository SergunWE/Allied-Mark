using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class DataHandler<T> : MonoBehaviour
{
    [SerializeField] protected List<T> data;
    public List<T> Get => data;
    
    private Dictionary<string, T> _dataDictionary;

    public Dictionary<string, T> DataDictionary
    {
        get
        {
            if (_dataDictionary == null)
            {
                Awake();
            }

            return _dataDictionary;
        }
        
    }

    private void Awake()
    {
        _dataDictionary = new Dictionary<string, T>();
        foreach (var info in data)
        {
            _dataDictionary[GetKey(info)] = info;
        }
    }

    protected abstract string GetKey(T value);
}
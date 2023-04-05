using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class DataHandler<T> : MonoBehaviour
{
    [SerializeField] protected List<T> data;
    public List<T> Get => data;
}
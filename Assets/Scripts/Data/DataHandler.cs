using System;
using UnityEngine;

[Serializable]
public abstract class DataHandler<T> : MonoBehaviour
{
    [SerializeField] protected T[] data;
    public T[] Get => data;
}
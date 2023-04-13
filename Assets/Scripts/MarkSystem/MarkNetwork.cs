using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MarkNetwork : NetworkBehaviour
{
    private NetworkList<MarkInfoStruct> _objectsThatMarked;

    private void Awake()
    {
        _objectsThatMarked = new NetworkList<MarkInfoStruct>(new List<MarkInfoStruct>());
    }

    public override void OnNetworkSpawn()
    {
        _objectsThatMarked.OnListChanged += OnMarkedObjectChanged;
    }

    public override void OnNetworkDespawn()
    {
        _objectsThatMarked.OnListChanged -= OnMarkedObjectChanged;
    }

    [ServerRpc]
    public void SetMarkServerRpc(MarkInfoStruct reference)
    {
        _objectsThatMarked.Add(reference);
    }
    
    [ServerRpc]
    public void UnSetMarkServerRpc(MarkInfoStruct reference)
    {
        _objectsThatMarked.Remove(reference);
    }

    private void OnMarkedObjectChanged(NetworkListEvent<MarkInfoStruct> value)
    {
        Debug.Log("MarkListChanged" + value.Value.MarkName + " " + _objectsThatMarked.Count + " " + gameObject.name);
    }
}

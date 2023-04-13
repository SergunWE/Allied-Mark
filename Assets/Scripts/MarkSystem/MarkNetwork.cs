using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MarkNetwork : NetworkBehaviour
{
    [SerializeField] private MarkViewer markViewer;
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

    [ServerRpc(RequireOwnership = false)]
    public void SetMarkServerRpc(MarkInfoStruct reference)
    {
        _objectsThatMarked.Add(reference);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void UnSetMarkServerRpc(MarkInfoStruct reference)
    {
        _objectsThatMarked.Remove(reference);
    }

    private void OnMarkedObjectChanged(NetworkListEvent<MarkInfoStruct> value)
    {
        if (markViewer == null) return;
        switch (value.Type)
        {
            case NetworkListEvent<MarkInfoStruct>.EventType.Add:
                markViewer.SetMark(value.Value.MarkName.Value);
                break;
            case NetworkListEvent<MarkInfoStruct>.EventType.RemoveAt:
                markViewer.UnsetMark(value.Value.MarkName.Value);
                break;
            case NetworkListEvent<MarkInfoStruct>.EventType.Remove:
                markViewer.UnsetMark(value.Value.MarkName.Value);
                break;
        }
    }
}

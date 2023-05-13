using System;
using System.Collections.Generic;
using Unity.Netcode;

public class MarkNetwork : NetworkBehaviour
{
    public event Action<MarkInfoNetwork> MarkSet;
    public event Action<MarkInfoNetwork> MarkUnset;
    private NetworkList<MarkInfoNetwork> _objectsThatMarked;

    private void Awake()
    {
        _objectsThatMarked = new NetworkList<MarkInfoNetwork>(new List<MarkInfoNetwork>());
    }

    private void Start()
    {
        foreach (var mark in _objectsThatMarked)
        {
            MarkSet?.Invoke(mark);
        }
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
    public void SetMarkServerRpc(MarkInfoNetwork reference)
    {
        _objectsThatMarked.Add(reference);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void UnSetMarkServerRpc(MarkInfoNetwork reference)
    {
        _objectsThatMarked.Remove(reference);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {
        NetworkObject.Despawn();
    }

    private void OnMarkedObjectChanged(NetworkListEvent<MarkInfoNetwork> value)
    {
        switch (value.Type)
        {
            case NetworkListEvent<MarkInfoNetwork>.EventType.Add:
                MarkSet?.Invoke(value.Value);
                break;
            case NetworkListEvent<MarkInfoNetwork>.EventType.RemoveAt:
                MarkUnset?.Invoke(value.Value);
                break;
            case NetworkListEvent<MarkInfoNetwork>.EventType.Remove:
                MarkUnset?.Invoke(value.Value);
                break;
        }
    }
}

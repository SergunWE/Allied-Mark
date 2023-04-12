using Unity.Netcode;
using UnityEngine;

public class MarkNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<NetworkObjectReference> _markedObjectId = new();

    public override void OnNetworkSpawn()
    {
        _markedObjectId.OnValueChanged += OnMarkedObjectChanged;
    }

    public override void OnNetworkDespawn()
    {
        _markedObjectId.OnValueChanged -= OnMarkedObjectChanged;
    }

    [ServerRpc]
    public void SetMarkServerRpc(NetworkObjectReference reference)
    {
        _markedObjectId.Value = reference;
    }

    private void OnMarkedObjectChanged(NetworkObjectReference oldValue, NetworkObjectReference newValue)
    {
        Debug.Log($"{OwnerClientId} marked " +
                  $"{newValue}");
        if (oldValue.TryGet(out var oldMark))
        {
            oldMark.GetComponent<MarkViewer>().SetMark(0);
        }
        if (newValue.TryGet(out var newMark))
        {
            newMark.GetComponent<MarkViewer>().SetMark(1);
        }
    }
}

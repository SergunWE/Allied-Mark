using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MarkNetwork : NetworkBehaviour
{
    [SerializeField] private MarkHandler markHandler;
    public event Action MarkChanged;
    
    public readonly List<(NetworkObject, MarkInfo)> LocalMarks = new(10);
    private NetworkList<MarkInfoNetwork> _markList;

    private void Awake()
    {
        _markList = new NetworkList<MarkInfoNetwork>(new List<MarkInfoNetwork>());
    }

    private void Start()
    {
        foreach (var t in _markList)
        {
            AddMarkLocal(t);
        }

        MarkChanged?.Invoke();
    }

    public override void OnNetworkSpawn()
    {
        _markList.OnListChanged += OnMarkedObjectChanged;
    }

    public override void OnNetworkDespawn()
    {
        _markList.OnListChanged -= OnMarkedObjectChanged;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetMarkServerRpc(MarkInfoNetwork reference)
    {
        _markList.Add(reference);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void UnSetMarkServerRpc(MarkInfoNetwork reference)
    {
        _markList.Remove(reference);
    }

    private void OnMarkedObjectChanged(NetworkListEvent<MarkInfoNetwork> value)
    {
        switch (value.Type)
        {
            case NetworkListEvent<MarkInfoNetwork>.EventType.Add:
                AddMarkLocal(value.Value);
                MarkChanged?.Invoke();
                break;
            case NetworkListEvent<MarkInfoNetwork>.EventType.RemoveAt:
                RemoveMarkLocal(value.Value);
                MarkChanged?.Invoke();
                break;
            case NetworkListEvent<MarkInfoNetwork>.EventType.Remove:
                RemoveMarkLocal(value.Value);
                MarkChanged?.Invoke();
                break;
        }
    }

    private void AddMarkLocal(MarkInfoNetwork info)
    {
        if (!info.Sender.TryGet(out var sender)) return;
        var markInfo = markHandler.DataDictionary[info.MarkName.Value];
        LocalMarks.Add((sender, markInfo));
    }

    private void RemoveMarkLocal(MarkInfoNetwork info)
    {
        if (!info.Sender.TryGet(out var sender)) return;
        var markInfo = markHandler.DataDictionary[info.MarkName.Value];
        LocalMarks.Remove((sender, markInfo));
    }
}

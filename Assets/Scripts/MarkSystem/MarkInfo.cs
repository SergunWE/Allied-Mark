using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(menuName = "Mark")]
public class MarkInfo : ScriptableObject
{
    public Material markColor;
    public string markName;
    public int maxMarkCount = 1;
}

public struct MarkInfoNetwork : IEquatable<MarkInfoNetwork>, INetworkSerializable
{
    public NetworkObjectReference Sender;
    public FixedString128Bytes MarkName;

    public MarkInfoNetwork(NetworkObjectReference sender, MarkInfo markInfo)
    {
        Sender = sender;
        MarkName = markInfo.markName;
    }

    public bool Equals(MarkInfoNetwork other)
    {
        return Sender.Equals(other.Sender) && MarkName.Equals(other.MarkName);
    }

    public override bool Equals(object obj)
    {
        return obj is MarkInfoNetwork other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Sender, MarkName);
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Sender);
        serializer.SerializeValue(ref MarkName);
    }
}
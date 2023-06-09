using System;
using System.Collections.Generic;
using Unity.Netcode;

public struct PuzzleCell : INetworkSerializable, IEquatable<PuzzleCell>, IEqualityComparer<PuzzleCell>
{
   public int Value;

   public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
   {
      serializer.SerializeValue(ref Value);
   }

   public bool Equals(PuzzleCell other)
   {
      return Value == other.Value;
   }

   public override bool Equals(object obj)
   {
      return obj is PuzzleCell other && Equals(other);
   }

   public override int GetHashCode()
   {
      return Value;
   }

   public bool Equals(PuzzleCell x, PuzzleCell y)
   {
      return x.Value == y.Value;
   }

   public int GetHashCode(PuzzleCell obj)
   {
      return obj.Value;
   }
}
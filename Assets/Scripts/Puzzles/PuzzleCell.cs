using System;
using System.Collections.Generic;
using Unity.Netcode;

public struct PuzzleCell : INetworkSerializable, IEquatable<PuzzleCell>, IEqualityComparer<PuzzleCell>
{
   public int CellValue;

   public PuzzleCell(int cellValue)
   {
      CellValue = cellValue;
   }

   public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
   {
      serializer.SerializeValue(ref CellValue);
   }

   public bool Equals(PuzzleCell other)
   {
      return CellValue == other.CellValue;
   }

   public override bool Equals(object obj)
   {
      return obj is PuzzleCell other && Equals(other);
   }

   public override int GetHashCode()
   {
      return CellValue;
   }

   public bool Equals(PuzzleCell x, PuzzleCell y)
   {
      return x.CellValue == y.CellValue;
   }

   public int GetHashCode(PuzzleCell obj)
   {
      return obj.CellValue;
   }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Random = UnityEngine.Random;

public class PicturePuzzleNetwork : PuzzleNetwork
{
    private NetworkList<PuzzleCell> _targetGrid;
    public PuzzleCell[] LocalTargetGrid { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _targetGrid = new NetworkList<PuzzleCell>(new List<PuzzleCell>());
    }

    protected override void Start()
    {
        if (IsOwner)
        {
            for (int i = 0; i < cellCount; i++)
            {
                _targetGrid.Add(new PuzzleCell(GetRandomCellValue()));
            }
        }

        if (IsClient)
        {
            LocalTargetGrid = new PuzzleCell[cellCount];
            for (int i = 0; i < cellCount; i++)
            {
                LocalTargetGrid[i] = _targetGrid[i];
            }
        }

        base.Start();
    }

    public void ChangeCell(int index)
    {
        ChangeCellServerRpc(index);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeCellServerRpc(int index, ServerRpcParams serverRpcParams = default)
    {
        //check who is playing the turn
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        if (LastPlayerId.Value == clientId) return;
        if (NetworkManager.ConnectedClients.Count > 1)
        {
            LastPlayerId.Value = clientId;
        }

        //changes the state of the cell
        CurrentGrid.Insert(index,
            CurrentGrid[index].CellValue == 3
                ? new PuzzleCell(0)
                : new PuzzleCell(CurrentGrid[index].CellValue + 1));
        //if it is the first turn, set the start time of the puzzle
        if (StartPuzzleTimeTicks.Value == 0)
        {
            StartPuzzleTimeTicks.Value = DateTime.UtcNow.Ticks;
        }
    }

    protected override int GetRandomCellValue()
    {
        return Random.Range(0, 4);
    }

    protected override bool IsPuzzleComplete()
    {
        return LocalCurrentGrid.SequenceEqual(LocalTargetGrid);
    }
}
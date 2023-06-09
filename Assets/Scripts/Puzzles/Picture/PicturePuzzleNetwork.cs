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
            for (var i = 0; i < cellCount; i++)
            {
                _targetGrid.Add(new PuzzleCell {Value = GetRandomCellValue()});
            }
        }

        if (IsClient)
        {
            LocalTargetGrid = new PuzzleCell[cellCount];
            for (var i = 0; i < cellCount; i++)
            {
                LocalTargetGrid[i] = _targetGrid[i];
            }
        }
        
        base.Awake();
    }

    public void ChangeCell(int index)
    {
        ChangeCellServerRpc(index);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeCellServerRpc(int index, ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (LastPlayerId.Value == clientId) return;
        if (NetworkManager.ConnectedClients.Count > 1)
        {
            LastPlayerId.Value = clientId;
        }

        if (CurrentGrid[index].Value == 3)
        {
            CurrentGrid[index] = new PuzzleCell {Value = 0};
        }
        else
        {
            CurrentGrid[index] = new PuzzleCell {Value = CurrentGrid[index].Value + 1};
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
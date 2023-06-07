using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class PicturePuzzleNetwork : NetworkBehaviour
{
    private NetworkList<PuzzleCell> _currentGrid;
    private NetworkList<PuzzleCell> _targetGrid;
    public readonly NetworkVariable<ulong> LastPlayerId = new(ulong.MaxValue);

    public PuzzleCell[] LocalCurrentGrid { get; private set; }
    public PuzzleCell[] LocalTargetGrid { get; private set; }

    public event Action<int> OnCurrentGridChanged;
    public event Action OnGridCreated;
    public event Action OnPuzzleComplete;

    private const int CellCount = 16;

    public override void OnNetworkDespawn()
    {
        _currentGrid.OnListChanged -= OnGridChanged;
    }

    private void Awake()
    {
        _currentGrid = new NetworkList<PuzzleCell>(new List<PuzzleCell>());
        _targetGrid = new NetworkList<PuzzleCell>(new List<PuzzleCell>());
    }

    private void Start()
    {
        if (IsOwner)
        {
            for (int i = 0; i < CellCount; i++)
            {
                _currentGrid.Add(new PuzzleCell {Value = Random.Range(0, 4)});
                _targetGrid.Add(new PuzzleCell {Value = Random.Range(0, 4)});
            }
        }

        if (IsClient)
        {
            LocalCurrentGrid = new PuzzleCell[CellCount];
            LocalTargetGrid = new PuzzleCell[CellCount];
            for (int i = 0; i < CellCount; i++)
            {
                LocalCurrentGrid[i] = _currentGrid[i];
                LocalTargetGrid[i] = _targetGrid[i];
            }

            OnGridCreated?.Invoke();
        }

        _currentGrid.OnListChanged += OnGridChanged;
    }

    public void ChangeCell(int index)
    {
        ChangeCellServerRpc(index);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeCellServerRpc(int index, ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        if (LastPlayerId.Value == clientId) return;
        if (NetworkManager.ConnectedClients.Count > 1)
        {
            LastPlayerId.Value = clientId;
        }

        if (_currentGrid[index].Value == 3)
        {
            _currentGrid[index] = new PuzzleCell {Value = 0};
        }
        else
        {
            _currentGrid[index] = new PuzzleCell {Value = _currentGrid[index].Value + 1};
        }
    }

    private void OnGridChanged(NetworkListEvent<PuzzleCell> networkListEvent)
    {
        LocalCurrentGrid[networkListEvent.Index] = networkListEvent.Value;
        OnCurrentGridChanged?.Invoke(networkListEvent.Index);

        if (!LocalCurrentGrid.SequenceEqual(LocalTargetGrid)) return;
        Debug.Log("Puzzle complete");
        OnPuzzleComplete?.Invoke();
    }
}
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class PuzzleNetwork : NetworkBehaviour
{
    [SerializeField] protected int cellCount;
    [field:SerializeField] public int PuzzleTimeSeconds { get; private set; }
    
    protected NetworkList<PuzzleCell> CurrentGrid;
    public readonly NetworkVariable<ulong> LastPlayerId = new(ulong.MaxValue);
    public readonly NetworkVariable<long> StartPuzzleTimeTicks = new();

    public event Action<int> OnCurrentGridChanged;
    public event Action OnGridCreated;
    public event Action OnPuzzleComplete;
    
    public PuzzleCell[] LocalCurrentGrid { get; protected set; }

    public override void OnNetworkDespawn()
    {
        CurrentGrid.OnListChanged -= OnGridChanged;
    }

    protected virtual void Awake()
    {
        CurrentGrid = new NetworkList<PuzzleCell>(new List<PuzzleCell>());
    }

    protected virtual void Start()
    {
        if (IsOwner)
        {
            for (int i = 0; i < cellCount; i++)
            {
                CurrentGrid.Add(new PuzzleCell {CellValue = GetRandomCellValue()});
            }
        }

        if (IsClient)
        {
            LocalCurrentGrid = new PuzzleCell[cellCount];
            for (int i = 0; i < cellCount; i++)
            {
                LocalCurrentGrid[i] = CurrentGrid[i];
            }

            OnGridCreated?.Invoke();
        }

        CurrentGrid.OnListChanged += OnGridChanged;
    }

    protected abstract int GetRandomCellValue();
    protected abstract bool IsPuzzleComplete();

    protected virtual void OnGridChanged(NetworkListEvent<PuzzleCell> networkListEvent)
    {
        LocalCurrentGrid[networkListEvent.Index] = networkListEvent.Value;
        OnCurrentGridChanged?.Invoke(networkListEvent.Index);
        
        if (!IsPuzzleComplete()) return;
        Debug.Log("Puzzle complete");
        OnPuzzleComplete?.Invoke();
    }
}
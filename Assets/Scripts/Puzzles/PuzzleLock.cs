using Unity.Netcode;
using UnityEngine;

public class PuzzleLock : NetworkBehaviour
{
    [SerializeField] private PicturePuzzleNetwork puzzleNetwork;
    [SerializeField] private GameEvent puzzleComplete;

    private Transform _playerCameraTransform;


    private void Start()
    {
        
    }

    private void OnEnable()
    {
        puzzleNetwork.OnPuzzleComplete += OnPuzzleComplete;
    }

    private void OnDisable()
    {
        puzzleNetwork.OnPuzzleComplete -= OnPuzzleComplete;
    }

    private void OnPuzzleComplete()
    {
        if (IsOwner)
        {
            NetworkObject.Despawn();
        }
        puzzleComplete.Raise();
    }
}
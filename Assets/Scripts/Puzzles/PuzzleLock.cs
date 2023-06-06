using Unity.Netcode;
using UnityEngine;

public class PuzzleLock : NetworkBehaviour
{
    [SerializeField] private PicturePuzzleNetwork puzzleNetwork;
    [SerializeField] private GameEvent puzzleHidden;

    private Transform _playerCameraTransform;

    public void ShowPuzzle()
    {
        puzzleNetwork.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HidePuzzle()
    {
        puzzleNetwork.transform.GetChild(0).gameObject.SetActive(false);
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

        HidePuzzle();
        puzzleHidden.Raise();
    }
}
using Unity.Netcode;
using UnityEngine;

public abstract class PuzzleBase : NetworkBehaviour
{
    [SerializeField] protected PicturePuzzleNetwork puzzleNetwork;
    [SerializeField] protected GameEvent puzzleHidden;

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

    protected abstract void OnPuzzleComplete();
}
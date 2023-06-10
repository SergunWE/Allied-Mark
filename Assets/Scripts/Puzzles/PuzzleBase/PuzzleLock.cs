public class PuzzleLock : PuzzleBase
{
    protected override void OnPuzzleComplete()
    {
        if (IsOwner)
        {
            NetworkObject.Despawn();
        }

        HidePuzzle();
        puzzleHidden.Raise();
    }
}
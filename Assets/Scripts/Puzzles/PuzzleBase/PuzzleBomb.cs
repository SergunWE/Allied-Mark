using UnityEngine;

public class PuzzleBomb : PuzzleBase
{
    [SerializeField] private MeshRenderer bombMeshRenderer;
    [SerializeField] private Material defusedBombMaterial;
    
    protected override void OnPuzzleComplete()
    {
        HidePuzzle();
        puzzleHidden.Raise();
        bombMeshRenderer.material = defusedBombMaterial;
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleControlManager : MonoBehaviour
{
    [SerializeField] private GameEvent puzzleDisplayed;
    [SerializeField] private GameEvent puzzleHidden;
    
    private Transform _playerCameraTransform;
    private PuzzleLock _lastPuzzle;
    
    protected void Start()
    {
        _playerCameraTransform = CameraHelper.GetPlayerCamera.transform;
    }
    
    public void OnStartPuzzleClicked(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        var obj = RaycastHelper.GetObject(_playerCameraTransform, 3f);
        if(obj == null) return;
        var component = obj.GetComponentInParent<PuzzleLock>();
        if (component == null) return;
        _lastPuzzle = component;
        _lastPuzzle.ShowPuzzle();
        puzzleDisplayed.Raise();
    }

    public void OnStopPuzzleClicked(InputAction.CallbackContext context)
    {
        if (!context.started || _lastPuzzle == null) return;
        _lastPuzzle.HidePuzzle();
        puzzleHidden.Raise();
        _lastPuzzle = null;
    }

    public void OnPuzzleStarted()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnPuzzleCompleted()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
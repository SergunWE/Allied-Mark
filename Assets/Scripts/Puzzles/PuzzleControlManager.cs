using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleControlManager : MonoBehaviour
{
    [SerializeField] private GameEvent puzzleStarted;
    [SerializeField] private PlayerInput playerInput;
    private Transform _playerCameraTransform;
    
    protected void Start()
    {
        _playerCameraTransform = CameraHelper.GetPlayerCamera.transform;
    }
    
    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        var obj = RaycastHelper.GetObject(_playerCameraTransform, 5f);
        if(obj == null) return;
        var component = obj.GetComponentInParent<PuzzleLock>();
        if (component == null) return;
        puzzleStarted.Raise();
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
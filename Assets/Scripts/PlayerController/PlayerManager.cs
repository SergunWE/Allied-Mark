using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CharacterCamera characterCamera;

    private PlayerCharacterInputs _inputs;
    private Vector2 _viewAxis = Vector2.zero;
    private bool _readyHandle;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (characterController != null && characterCamera != null)
        {
            SetPlayerCharacter(characterController, characterCamera);
        }
    }
    
    private void Update()
    {
        if (_readyHandle)
        {
            HandleCharacterInput();
        }
    }

    private void LateUpdate()
    {
        if (_readyHandle)
        {
            HandleCameraInput();
        }
    }

    public void SetPlayerCharacter(CharacterController playerCharacter, CharacterCamera playerCamera)
    {
        characterController = playerCharacter;
        characterCamera = playerCamera;
        characterCamera.SetFollowTransform(characterController.CameraFollowPoint);
        characterCamera.IgnoredColliders.Clear();
        characterCamera.IgnoredColliders.AddRange(characterController.GetComponentsInChildren<Collider>());
        _readyHandle = true;
    }

    public void OnMovementChanged(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _inputs.MoveAxisForward = direction.y;
        _inputs.MoveAxisRight = direction.x;
    }
    
    public void OnViewChanged(InputAction.CallbackContext context)
    {
        _viewAxis = context.ReadValue<Vector2>();
    }
    
    private void HandleCharacterInput()
    {
        _inputs.CameraRotation = characterCamera.Transform.rotation;
        characterController.SetInputs(ref _inputs);
    }
    
    private void HandleCameraInput()
    {
        float mouseLookAxisUp = _viewAxis.y;
        float mouseLookAxisRight = _viewAxis.x;
        var lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            lookInputVector = Vector3.zero;
        }
        characterCamera.UpdateWithInput(Time.deltaTime, 1f, lookInputVector);
    }
}
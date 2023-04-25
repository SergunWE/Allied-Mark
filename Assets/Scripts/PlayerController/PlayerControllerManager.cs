using NetworkFramework.Netcode_Components;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerManager : NetworkComponentManager<MovementCharacterController>
{
    [SerializeField] private CharacterCamera characterCamera;

    private PlayerCharacterInputs _inputs;
    private Vector2 _viewAxis = Vector2.zero;

    private bool _readyHandle;

    protected override void Start()
    {
        base.Start();
        if (networkComponent != null && characterCamera != null)
        {
            SetPlayerCharacter(networkComponent, characterCamera);
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

    public void SetPlayerCharacter(MovementCharacterController playerMovementCharacter, CharacterCamera playerCamera)
    {
        networkComponent = playerMovementCharacter;
        characterCamera = playerCamera;
        characterCamera.SetFollowTransform(networkComponent.cameraFollowPoint);
        characterCamera.ignoredColliders.Clear();
        characterCamera.ignoredColliders.AddRange(networkComponent.GetComponentsInChildren<Collider>());
        Cursor.lockState = CursorLockMode.Locked;
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

    public void OnJumpChanged(InputAction.CallbackContext context)
    {
        _inputs.JumpDown = !context.canceled;
    }

    private void HandleCharacterInput()
    {
        //if (!IsOwner) return;
        _inputs.CameraRotation = characterCamera.Transform.rotation;
        networkComponent.SetInputs(ref _inputs);
    }

    private void HandleCameraInput()
    {
        //if (!IsOwner) return;
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
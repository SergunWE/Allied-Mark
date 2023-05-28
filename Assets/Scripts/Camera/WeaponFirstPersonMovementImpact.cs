using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponFirstPersonMovementImpact : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float maxRotation = 10f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float bobSpeed = 1;
    [SerializeField] private Vector3 bobLimit;
    [SerializeField] private Vector3 travelLimit;
    [SerializeField] private float smoothPosition = 15f;
    
    private float _curveTime;
    private Quaternion _swayEulerRotation;
    private Quaternion _lastCameraRotation;
    private Vector3 _lastCameraPosition;
    private Vector3 _startPosition;
    private Vector2 _movementDirection;
    private bool _isPlayerGrounded = true;
    private Vector3 _playerVelocity;
    private Vector3 _bobPosition;

    private void Awake()
    {
        _startPosition = transform.localPosition;
    }

    private void Update()
    {
        Sway();
        BobOffset();
        _lastCameraRotation = cameraTransform.localRotation;
        _lastCameraPosition = cameraTransform.position;
    }

    public void OnPlayerGroundedChanged(bool state)
    {
        _isPlayerGrounded = state;
    }

    public void OnMovementChanged(InputAction.CallbackContext context)
    {
        _movementDirection = context.ReadValue<Vector2>();
    }

    private void Sway()
    {
        var view = (_lastCameraRotation * Quaternion.Inverse(cameraTransform.localRotation)).eulerAngles;

        if (view != Vector3.zero)
        {
            view.x = ClampAngle(view.x, -maxRotation, maxRotation);
            view.y = ClampAngle(view.y, -maxRotation, maxRotation);
            _swayEulerRotation = Quaternion.Euler(view.x, view.y, view.y);
        }

        transform.localRotation = Damp(transform.localRotation, _swayEulerRotation,
            rotationSpeed,Time.deltaTime);
    }

    private void BobOffset()
    {
        _playerVelocity = _lastCameraPosition - cameraTransform.position;
        if (_isPlayerGrounded && _playerVelocity.normalized.magnitude > 0.5f)
        {
            _curveTime += _playerVelocity.normalized.magnitude * bobSpeed * Time.deltaTime;
            float x = Mathf.Cos(_curveTime) * bobLimit.x - _movementDirection.x * travelLimit.x;
            float y = Mathf.Sin(2f * _curveTime) * bobLimit.y;
            float z = Mathf.Sin(_curveTime * 2f) * bobLimit.z - _movementDirection.y * travelLimit.z;
            _bobPosition = _startPosition + new Vector3(x, y, z);
        }
        
        transform.localPosition = Vector3.Lerp(transform.localPosition, _bobPosition, Time.deltaTime * smoothPosition);
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < 0f)
            angle += 360f;
        return angle > 180f ? Mathf.Max(angle, 360f + min) : Mathf.Min(angle, max);
    }

    private static Vector3 Damp(Vector3 source, Vector3 target, float smoothing, float dt)
    {
        return Vector3.Lerp(source, target, 1 - Mathf.Pow(smoothing, dt));
    }
    
    private static Quaternion Damp(Quaternion a, Quaternion b, float lambda, float dt)
    {
        return Quaternion.LerpUnclamped(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
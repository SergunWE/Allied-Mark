using System.Collections.Generic;
using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
    [Header("Framing")] [SerializeField] private Camera cameraComponent;
    [SerializeField] private Vector2 followPointFraming = new(0f, 0f);
    [SerializeField] private float followingSharpness = 10000f;
    [Header("Distance")] [SerializeField] private float defaultDistance = 6f;
    [SerializeField] private float minDistance = 0f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float distanceMovementSpeed = 5f;
    [SerializeField] private float distanceMovementSharpness = 10f;
    [Header("Rotation")] [SerializeField] private bool invertX = false;
    [SerializeField] private bool invertY = false;
    [Range(-90f, 90f)] [SerializeField] private float defaultVerticalAngle = 20f;
    [Range(-90f, 90f)] [SerializeField] private float minVerticalAngle = -90f;
    [Range(-90f, 90f)] [SerializeField] private float maxVerticalAngle = 90f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float rotationSharpness = 10000f;

    [Header("Obstruction")] [SerializeField]
    private float obstructionCheckRadius = 0.2f;

    [SerializeField] private LayerMask obstructionLayers = -1;
    [SerializeField] private float obstructionSharpness = 10000f;

    public List<Collider> ignoredColliders = new();

    public Transform Transform { get; private set; }
    public Transform FollowTransform { get; private set; }

    public Vector3 PlanarDirection { get; set; }
    public float TargetDistance { get; set; }

    private bool _distanceIsObstructed;
    private float _currentDistance;
    private float _targetVerticalAngle;
    private RaycastHit _obstructionHit;
    private int _obstructionCount;
    private RaycastHit[] _obstructions = new RaycastHit[MaxObstructions];
    private float _obstructionTime;
    private Vector3 _currentFollowPosition;

    private const int MaxObstructions = 32;

    private void OnValidate()
    {
        defaultDistance = Mathf.Clamp(defaultDistance, minDistance, maxDistance);
        defaultVerticalAngle = Mathf.Clamp(defaultVerticalAngle, minVerticalAngle, maxVerticalAngle);
    }

    private void Awake()
    {
        Transform = this.transform;

        _currentDistance = defaultDistance;
        TargetDistance = _currentDistance;

        _targetVerticalAngle = 0f;

        PlanarDirection = Vector3.forward;
    }

    // Set the transform that the camera will orbit around
    public void SetFollowTransform(Transform t)
    {
        FollowTransform = t;
        PlanarDirection = FollowTransform.forward;
        _currentFollowPosition = FollowTransform.position;
    }

    public void UpdateWithInput(float deltaTime, float zoomInput, Vector3 rotationInput)
    {
        if (!FollowTransform) return;
        if (invertX)
        {
            rotationInput.x *= -1f;
        }

        if (invertY)
        {
            rotationInput.y *= -1f;
        }

        // Process rotation input
        var followTransformUp = FollowTransform.up;
        var rotationFromInput = Quaternion.Euler(followTransformUp * (rotationInput.x * rotationSpeed));
        PlanarDirection = rotationFromInput * PlanarDirection;
        PlanarDirection = Vector3.Cross(followTransformUp, Vector3.Cross(PlanarDirection, followTransformUp));
        var planarRot = Quaternion.LookRotation(PlanarDirection, followTransformUp);

        _targetVerticalAngle -= (rotationInput.y * rotationSpeed);
        _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, minVerticalAngle, maxVerticalAngle);
        var verticalRot = Quaternion.Euler(_targetVerticalAngle, 0, 0);
        var targetRotation = Quaternion.Slerp(Transform.rotation, planarRot * verticalRot,
            1f - Mathf.Exp(-rotationSharpness * deltaTime));

        // Apply rotation
        Transform.rotation = targetRotation;

        // Process distance input
        if (_distanceIsObstructed && Mathf.Abs(zoomInput) > 0f)
        {
            TargetDistance = _currentDistance;
        }

        TargetDistance += zoomInput * distanceMovementSpeed;
        TargetDistance = Mathf.Clamp(TargetDistance, minDistance, maxDistance);

        // Find the smoothed follow position
        _currentFollowPosition = Vector3.Lerp(_currentFollowPosition, FollowTransform.position,
            1f - Mathf.Exp(-followingSharpness * deltaTime));

        // Handle obstructions
        {
            var closestHit = new RaycastHit
            {
                distance = Mathf.Infinity
            };
            _obstructionCount = Physics.SphereCastNonAlloc(_currentFollowPosition, obstructionCheckRadius,
                -Transform.forward, _obstructions, TargetDistance, obstructionLayers,
                QueryTriggerInteraction.Ignore);
            for (int i = 0; i < _obstructionCount; i++)
            {
                bool isIgnored = false;
                for (int j = 0; j < ignoredColliders.Count; j++)
                {
                    if (ignoredColliders[j] != _obstructions[i].collider) continue;
                    isIgnored = true;
                    break;
                }

                for (int j = 0; j < ignoredColliders.Count; j++)
                {
                    if (ignoredColliders[j] != _obstructions[i].collider) continue;
                    isIgnored = true;
                    break;
                }

                if (!isIgnored && _obstructions[i].distance < closestHit.distance && _obstructions[i].distance > 0)
                {
                    closestHit = _obstructions[i];
                }
            }

            // If obstructions detecter
            if (closestHit.distance < Mathf.Infinity)
            {
                _distanceIsObstructed = true;
                _currentDistance = Mathf.Lerp(_currentDistance, closestHit.distance,
                    1 - Mathf.Exp(-obstructionSharpness * deltaTime));
            }
            // If no obstruction
            else
            {
                _distanceIsObstructed = false;
                _currentDistance = Mathf.Lerp(_currentDistance, TargetDistance,
                    1 - Mathf.Exp(-distanceMovementSharpness * deltaTime));
            }
        }

        // Find the smoothed camera orbit position
        var targetPosition = _currentFollowPosition - ((targetRotation * Vector3.forward) * _currentDistance);

        // Handle framing
        targetPosition += Transform.right * followPointFraming.x;
        targetPosition += Transform.up * followPointFraming.y;

        // Apply position
        Transform.position = targetPosition;
    }
}
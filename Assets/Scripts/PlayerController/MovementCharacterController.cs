using System;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class MovementCharacterController : MonoBehaviour, ICharacterController
{
    [SerializeField] private KinematicCharacterMotor motor;

    [Header("Stable Movement")] [SerializeField]
    private float maxStableMoveSpeed = 10f;

    [SerializeField] private float stableMovementSharpness = 15f;
    [SerializeField] private float orientationSharpness = 10f;
    [SerializeField] private OrientationMethod orientationMethod = OrientationMethod.TowardsCamera;

    [Header("Air Movement")] [SerializeField]
    private float maxAirMoveSpeed = 5f;

    [SerializeField] private float airAccelerationSpeed = 15f;
    [SerializeField] private float drag = 0.1f;

    [Header("Jumping")] [SerializeField] private bool allowJumpingWhenSliding;
    [SerializeField] private float jumpUpSpeed = 10f;
    [SerializeField] private float jumpScalableForwardSpeed = 10f;
    [SerializeField] private float jumpPreGroundingGraceTime = 0f;
    [SerializeField] private float jumpPostGroundingGraceTime = 0f;

    [Header("Misc")] [SerializeField] private List<Collider> ignoredColliders = new();
    [SerializeField] private BonusOrientationMethod bonusOrientationMethod = BonusOrientationMethod.None;
    [SerializeField] private float bonusOrientationSharpness = 10f;
    [SerializeField] private Vector3 gravity = new(0, -30f, 0);
    [SerializeField] private Transform meshRoot;

    public Transform cameraFollowPoint;
    public CharacterState CurrentCharacterState { get; private set; }

    private Collider[] _probedColliders = new Collider[8];
    private RaycastHit[] _probedHits = new RaycastHit[8];
    private Vector3 _moveInputVector;
    private Vector3 _lookInputVector;
    private bool _jumpRequested = false;
    private bool _jumpConsumed = false;
    private bool _jumpedThisFrame = false;
    private float _timeSinceJumpRequested = Mathf.Infinity;
    private float _timeSinceLastAbleToJump = 0f;
    private Vector3 _internalVelocityAdd = Vector3.zero;

    private Vector3 _lastInnerNormal = Vector3.zero;
    private Vector3 _lastOuterNormal = Vector3.zero;

    private void Awake()
    {
        // Handle initial state
        TransitionToState(CharacterState.Default);

        // Assign the characterController to the motor
        motor.CharacterController = this;
    }

    /// <summary>
    /// Handles movement state transitions and enter/exit callbacks
    /// </summary>
    public void TransitionToState(CharacterState newState)
    {
        var tmpInitialState = CurrentCharacterState;
        OnStateExit(tmpInitialState, newState);
        CurrentCharacterState = newState;
        OnStateEnter(newState, tmpInitialState);
    }

    /// <summary>
    /// Event when entering a state
    /// </summary>
    public void OnStateEnter(CharacterState state, CharacterState fromState)
    {
        switch (state)
        {
            case CharacterState.Default:
            {
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, "Incorrect state");
        }
    }

    /// <summary>
    /// Event when exiting a state
    /// </summary>
    public void OnStateExit(CharacterState state, CharacterState toState)
    {
        switch (state)
        {
            case CharacterState.Default:
            {
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, "Incorrect state");
        }
    }

    /// <summary>
    /// This is called every frame by ExamplePlayer in order to tell the character what its inputs are
    /// </summary>
    public void SetInputs(ref PlayerCharacterInputs inputs)
    {
        // Clamp input
        Vector3 moveInputVector =
            Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

        // Calculate camera direction and rotation on the character plane
        var cameraPlanarDirection =
            Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, motor.CharacterUp).normalized;
        if (cameraPlanarDirection.sqrMagnitude == 0f)
        {
            cameraPlanarDirection =
                Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, motor.CharacterUp).normalized;
        }

        var cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, motor.CharacterUp);

        switch (CurrentCharacterState)
        {
            case CharacterState.Default:
            {
                // Move and look inputs
                _moveInputVector = cameraPlanarRotation * moveInputVector;

                _lookInputVector = orientationMethod switch
                {
                    OrientationMethod.TowardsCamera => cameraPlanarDirection,
                    OrientationMethod.TowardsMovement => _moveInputVector.normalized,
                    _ => _lookInputVector
                };

                // Jumping input
                if (inputs.JumpDown)
                {
                    _timeSinceJumpRequested = 0f;
                    _jumpRequested = true;
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// This is called every frame by the AI script in order to tell the character what its inputs are
    /// </summary>
    public void SetInputs(ref AICharacterInputs inputs)
    {
        _moveInputVector = inputs.MoveVector;
        _lookInputVector = inputs.LookVector;
    }

    private Quaternion _tmpTransientRot;

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is called before the character begins its movement update
    /// </summary>
    public void BeforeCharacterUpdate(float deltaTime)
    {
    }

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is where you tell your character what its rotation should be right now. 
    /// This is the ONLY place where you should set the character's rotation
    /// </summary>
    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        switch (CurrentCharacterState)
        {
            case CharacterState.Default:
            {
                if (_lookInputVector.sqrMagnitude > 0f && orientationSharpness > 0f)
                {
                    // Smoothly interpolate from current to target look direction
                    var smoothedLookInputDirection = Vector3.Slerp(motor.CharacterForward, _lookInputVector,
                        1 - Mathf.Exp(-orientationSharpness * deltaTime)).normalized;

                    // Set the current rotation (which will be used by the KinematicCharacterMotor)
                    currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, motor.CharacterUp);
                }

                var currentUp = (currentRotation * Vector3.up);
                switch (bonusOrientationMethod)
                {
                    case BonusOrientationMethod.TowardsGravity:
                    {
                        // Rotate from current up to invert gravity
                        var smoothedGravityDir = Vector3.Slerp(currentUp, -gravity.normalized,
                            1 - Mathf.Exp(-bonusOrientationSharpness * deltaTime));
                        currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        break;
                    }
                    case BonusOrientationMethod.TowardsGroundSlopeAndGravity
                        when motor.GroundingStatus.IsStableOnGround:
                    {
                        var initialCharacterBottomHemiCenter =
                            motor.TransientPosition + (currentUp * motor.Capsule.radius);

                        var smoothedGroundNormal = Vector3.Slerp(motor.CharacterUp,
                            motor.GroundingStatus.GroundNormal, 1 - Mathf.Exp(-bonusOrientationSharpness * deltaTime));
                        currentRotation = Quaternion.FromToRotation(currentUp, smoothedGroundNormal) * currentRotation;

                        // Move the position to create a rotation around the bottom hemi center instead of around the pivot
                        motor.SetTransientPosition(initialCharacterBottomHemiCenter +
                                                   (currentRotation * Vector3.down * motor.Capsule.radius));
                        break;
                    }
                    case BonusOrientationMethod.TowardsGroundSlopeAndGravity:
                    {
                        var smoothedGravityDir = Vector3.Slerp(currentUp, -gravity.normalized,
                            1 - Mathf.Exp(-bonusOrientationSharpness * deltaTime));
                        currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        break;
                    }
                    case BonusOrientationMethod.None:
                    default:
                    {
                        var smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up,
                            1 - Mathf.Exp(-bonusOrientationSharpness * deltaTime));
                        currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        break;
                    }
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is where you tell your character what its velocity should be right now. 
    /// This is the ONLY place where you can set the character's velocity
    /// </summary>
    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        switch (CurrentCharacterState)
        {
            case CharacterState.Default:
            {
                // Ground movement
                if (motor.GroundingStatus.IsStableOnGround)
                {
                    float currentVelocityMagnitude = currentVelocity.magnitude;

                    var effectiveGroundNormal = motor.GroundingStatus.GroundNormal;

                    // Reorient velocity on slope
                    currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) *
                                      currentVelocityMagnitude;

                    // Calculate target velocity
                    var inputRight = Vector3.Cross(_moveInputVector, motor.CharacterUp);
                    var reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized *
                                          _moveInputVector.magnitude;
                    var targetMovementVelocity = reorientedInput * maxStableMoveSpeed;

                    // Smooth movement Velocity
                    currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity,
                        1f - Mathf.Exp(-stableMovementSharpness * deltaTime));
                }
                // Air movement
                else
                {
                    // Add move input
                    if (_moveInputVector.sqrMagnitude > 0f)
                    {
                        var addedVelocity = _moveInputVector * (airAccelerationSpeed * deltaTime);

                        var currentVelocityOnInputsPlane =
                            Vector3.ProjectOnPlane(currentVelocity, motor.CharacterUp);

                        // Limit air velocity from inputs
                        if (currentVelocityOnInputsPlane.magnitude < maxAirMoveSpeed)
                        {
                            // clamp addedVel to make total vel not exceed max vel on inputs plane
                            var newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity,
                                maxAirMoveSpeed);
                            addedVelocity = newTotal - currentVelocityOnInputsPlane;
                        }
                        else
                        {
                            // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                            if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                            {
                                addedVelocity = Vector3.ProjectOnPlane(addedVelocity,
                                    currentVelocityOnInputsPlane.normalized);
                            }
                        }

                        // Prevent air-climbing sloped walls
                        if (motor.GroundingStatus.FoundAnyGround)
                        {
                            if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                            {
                                var perpendicularObstructionNormal = Vector3
                                    .Cross(Vector3.Cross(motor.CharacterUp, motor.GroundingStatus.GroundNormal),
                                        motor.CharacterUp).normalized;
                                addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpendicularObstructionNormal);
                            }
                        }

                        // Apply added velocity
                        currentVelocity += addedVelocity;
                    }

                    // Gravity
                    currentVelocity += gravity * deltaTime;

                    // Drag
                    currentVelocity *= (1f / (1f + (drag * deltaTime)));
                }

                // Handle jumping
                _jumpedThisFrame = false;
                _timeSinceJumpRequested += deltaTime;
                if (_jumpRequested)
                {
                    // See if we actually are allowed to jump
                    if (!_jumpConsumed &&
                        ((allowJumpingWhenSliding
                             ? motor.GroundingStatus.FoundAnyGround
                             : motor.GroundingStatus.IsStableOnGround) ||
                         _timeSinceLastAbleToJump <= jumpPostGroundingGraceTime))
                    {
                        // Calculate jump direction before ungrounding
                        var jumpDirection = motor.CharacterUp;
                        if (motor.GroundingStatus is {FoundAnyGround: true, IsStableOnGround: false})
                        {
                            jumpDirection = motor.GroundingStatus.GroundNormal;
                        }

                        // Makes the character skip ground probing/snapping on its next update. 
                        // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                        motor.ForceUnground();

                        // Add to the return velocity and reset jump state
                        currentVelocity += (jumpDirection * jumpUpSpeed) -
                                           Vector3.Project(currentVelocity, motor.CharacterUp);
                        currentVelocity += (_moveInputVector * jumpScalableForwardSpeed);
                        _jumpRequested = false;
                        _jumpConsumed = true;
                        _jumpedThisFrame = true;
                    }
                }

                // Take into account additive velocity
                if (_internalVelocityAdd.sqrMagnitude > 0f)
                {
                    currentVelocity += _internalVelocityAdd;
                    _internalVelocityAdd = Vector3.zero;
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is called after the character has finished its movement update
    /// </summary>
    public void AfterCharacterUpdate(float deltaTime)
    {
        switch (CurrentCharacterState)
        {
            case CharacterState.Default:
            {
                // Handle jump-related values
                {
                    // Handle jumping pre-ground grace period
                    if (_jumpRequested && _timeSinceJumpRequested > jumpPreGroundingGraceTime)
                    {
                        _jumpRequested = false;
                    }

                    if (allowJumpingWhenSliding
                            ? motor.GroundingStatus.FoundAnyGround
                            : motor.GroundingStatus.IsStableOnGround)
                    {
                        // If we're on a ground surface, reset jumping values
                        if (!_jumpedThisFrame)
                        {
                            _jumpConsumed = false;
                        }

                        _timeSinceLastAbleToJump = 0f;
                    }
                    else
                    {
                        // Keep track of time since we were last able to jump (for grace period)
                        _timeSinceLastAbleToJump += deltaTime;
                    }
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void PostGroundingUpdate(float deltaTime)
    {
        switch (motor.GroundingStatus.IsStableOnGround)
        {
            // Handle landing and leaving ground
            case true when !motor.LastGroundingStatus.IsStableOnGround:
                OnLanded();
                break;
            case false when motor.LastGroundingStatus.IsStableOnGround:
                OnLeaveStableGround();
                break;
        }
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        if (ignoredColliders.Count == 0)
        {
            return true;
        }

        return !ignoredColliders.Contains(coll);
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport)
    {
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport)
    {
    }

    public void AddVelocity(Vector3 velocity)
    {
        switch (CurrentCharacterState)
        {
            case CharacterState.Default:
            {
                _internalVelocityAdd += velocity;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
        Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
    }

    protected void OnLanded()
    {
    }

    protected void OnLeaveStableGround()
    {
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
    }

    private void OnDestroy()
    {
        Destroy(motor);
    }
}
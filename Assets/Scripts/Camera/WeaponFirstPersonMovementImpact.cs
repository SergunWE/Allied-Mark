using UnityEngine;

public class WeaponFirstPersonMovementImpact : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float maxRotation = 5f;
    [SerializeField] private float smoothRotation = 12f;
    [SerializeField] private float zAxisMultiply = 1f;

    private Vector3 _swayEulerRotation;
    private Quaternion _lastCameraRotation;

    private void Start()
    {
        _lastCameraRotation = cameraTransform.localRotation;
    }

    private void Update()
    {
        Sway();
        _lastCameraRotation = cameraTransform.localRotation;
    }

    private void Sway()
    {
        var invertView = (_lastCameraRotation * Quaternion.Inverse(cameraTransform.localRotation)).eulerAngles;
        invertView.x = ClampAngle(invertView.x, -maxRotation, maxRotation);
        invertView.y = ClampAngle(invertView.y, -maxRotation, maxRotation);

        _swayEulerRotation = new Vector3(invertView.x, invertView.y, invertView.y * zAxisMultiply);
        
        var targetRotation = Quaternion.Euler(_swayEulerRotation);
        transform.localRotation = Quaternion.SlerpUnclamped(transform.localRotation, targetRotation, 
            smoothRotation * Time.deltaTime);
    }
    
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < 0f)
            angle += 360f;
        return angle > 180f ? Mathf.Max(angle, 360f + min) : Mathf.Min(angle, max);
    }
}
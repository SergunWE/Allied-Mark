using UnityEngine;

public static class RaycastHelper
{
    private const float MaxDistance = 100f;
    
    public static GameObject GetObject(Transform shootingObject, float maxDistance = MaxDistance)
    {
        var ray = new Ray(shootingObject.position, shootingObject.forward);
        return Physics.Raycast(ray, out var hit, MaxDistance) ? hit.collider.gameObject : null;
    }
}
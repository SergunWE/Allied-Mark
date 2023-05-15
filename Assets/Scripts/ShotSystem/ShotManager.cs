using Unity.Netcode;
using UnityEngine;

public class ShotManager : MonoBehaviour
{
    public static void TakeShot(Transform shotObject, int damage, NetworkObject sender)
    {
        var target = RaycastHelper.GetObject(shotObject);
        if (target == null) return;
        var comp = ComponentHelper.FindComponent<IDamaged>(target);
        comp.TakeDamage(damage, sender);
    }
}
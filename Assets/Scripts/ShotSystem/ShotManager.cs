using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ShotManager : MonoBehaviour
{
    private readonly Dictionary<GameObject, int> _hitObjects = new(50);
    
    public void TakeShot(Transform shotObject, int damage, int bulletCount, NetworkObject sender)
    {
        _hitObjects.Clear();

        for (int i = 0; i < bulletCount; i++)
        {
            var obj = RaycastHelper.GetObject(shotObject);
            if (obj == null) continue;
            _hitObjects.TryAdd(obj, 0);
            _hitObjects[obj] += damage;
        }

        foreach (var hitObject in _hitObjects)
        {
            var comp = ComponentHelper.FindComponent<IDamaged>(hitObject.Key);
            comp?.TakeDamage(hitObject.Value, sender);
        }
    }
}
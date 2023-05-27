using NetworkFramework.EventSystem.EventParameter;
using NetworkFramework.NetcodeComponents;
using UnityEngine;

public class HeathPlayerManager : NetworkComponentManager<PlayerHealthNetwork>
{
    [SerializeField] private GameEventString healthChanged;

    protected override void Start()
    {
        base.Start();
        networkComponent.ValueChanged += OnHealthChanged;
        networkComponent.ObjectDeath += OnDeath;
    }

    private void OnHealthChanged(int value)
    {
        healthChanged.Raise(value.ToString());
    }

    private void OnDeath()
    {
        healthChanged.Raise("Вы погибли");
    }

    private void OnDestroy()
    {
        networkComponent.ValueChanged -= OnHealthChanged;
    }
}
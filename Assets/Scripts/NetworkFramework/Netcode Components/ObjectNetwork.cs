using System;
using Unity.Netcode;

namespace NetworkFramework.Netcode_Components
{
    public abstract class ObjectNetwork<T> : NetworkBehaviour
    {
        public event Action<T> ValueChanged;
        protected readonly NetworkVariable<T> NetworkVariable = new();

        public override void OnNetworkSpawn()
        {
            NetworkVariable.OnValueChanged += OnVariableChanged;
        }

        public override void OnNetworkDespawn()
        {
            NetworkVariable.OnValueChanged -= OnVariableChanged;
        }

        protected void TriggerEvent(T value)
        {
            ValueChanged?.Invoke(value);
        }

        protected abstract void OnVariableChanged(T oldValue, T newValue);
    }
}
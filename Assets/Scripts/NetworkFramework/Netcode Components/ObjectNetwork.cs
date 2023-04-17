using System;
using Unity.Netcode;

namespace NetworkFramework.Netcode_Components
{
    /// <summary>
    /// A base class for network variables
    /// </summary>
    /// <typeparam name="T">Non reference types</typeparam>
    public abstract class ObjectNetwork<T> : NetworkBehaviour
    {
        /// <summary>
        /// Network variable change event
        /// </summary>
        public event Action<T> ValueChanged;
        /// <summary>
        /// Netcode library network variable
        /// </summary>
        protected readonly NetworkVariable<T> NetworkVariable = new();
        
        public override void OnNetworkSpawn()
        {
            NetworkVariable.OnValueChanged += OnVariableChanged;
        }

        public override void OnNetworkDespawn()
        {
            NetworkVariable.OnValueChanged -= OnVariableChanged;
        }

        /// <summary>
        /// Calling a network variable change event
        /// </summary>
        /// <param name="value">The value of the variable to be sent</param>
        protected void TriggerEvent(T value)
        {
            ValueChanged?.Invoke(value);
        }

        /// <summary>
        /// Checking a local value with a network value
        /// </summary>
        /// <param name="localValue">Local value</param>
        public abstract void CheckLocalChange(T localValue);
        /// <summary>
        /// Network variable change event function
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected abstract void OnVariableChanged(T oldValue, T newValue);
    }
}
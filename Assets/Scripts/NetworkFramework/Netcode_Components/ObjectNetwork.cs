using System;
using Unity.Netcode;

namespace NetworkFramework.Netcode_Components
{
    /// <summary>
    /// A base class for network variables
    /// </summary>
    /// <typeparam name="T">Non reference types</typeparam>
    public abstract class ObjectNetwork<T> : NetworkBehaviour where T : IComparable<T>
    {
        private T _localValue;

        public T LocalValue
        {
            get => _localValue;
            protected set
            {
                if(value.CompareTo(_localValue) == 0) return;
                _localValue = value;
                ValueChanged?.Invoke(value);
                VariableChangedMessage();
            }
        }

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

        private void Start()
        {
            LocalValue = NetworkVariable.Value;
        }

        /// <summary>
        /// Network variable change event function
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnVariableChanged(T oldValue, T newValue)
        {
            if (LocalValue.CompareTo(newValue) == 0) return;
            _localValue = newValue;
            ValueChanged?.Invoke(newValue);
            VariableChangedMessage();
        }

        protected virtual void VariableChangedMessage()
        {
        }
    }
}
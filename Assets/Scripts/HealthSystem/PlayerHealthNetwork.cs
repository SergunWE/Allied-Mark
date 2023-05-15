using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace HealthSystem
{
    public class PlayerHealthNetwork : HealthNetwork
    {
        [SerializeField] private PlayerClassNetwork playerClassNetwork;

        private void OnEnable()
        {
            playerClassNetwork.ValueChanged += SetHealth;
        }

        private void OnDisable()
        {
            playerClassNetwork.ValueChanged -= SetHealth;
        }

        private void SetHealth(FixedString128Bytes playerClassName)
        {
            if (playerClassName.IsEmpty) return;
            SetHealth(playerClassNetwork.PlayerClassInfo.Health);
        }

        public override void TakeDamage(int damage, NetworkObject sender)
        {
            float damageCoef = sender.IsPlayerObject ? 0.5f : 1f;
            TakeDamageServerRpc((int) (damage * damageCoef));
        }

        [ServerRpc]
        private void TakeDamageServerRpc(int damage)
        {
            if (NetworkVariable.Value <= damage)
            {
                Death();
                return;
            }

            NetworkVariable.Value -= damage;
        }

        protected override void VariableChangedMessage()
        {
            Debug.Log($"Client {OwnerClientId} have {LocalValue} HP");
        }
    }
}
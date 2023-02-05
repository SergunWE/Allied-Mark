using NetworkFramework.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class AuthenticationComponent : MonoBehaviour
    {
        public UnityEvent<bool> initComplete;
        public UnityEvent<bool> authComplete;
        
        private void Start()
        {
            InitAuthentication();
        }

        public async void InitAuthentication()
        {
            initComplete?.Invoke(await AuthenticationManager.InitializeAsync());
        }

        public async void SignInAnonymously()
        {
            authComplete?.Invoke(await AuthenticationManager.SignInAnonymouslyAsync());
        }
    }
}
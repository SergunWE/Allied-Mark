using NetworkFramework.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace NetworkFramework.MonoBehaviour_Components
{
    /// <summary>
    /// User authentication component
    /// </summary>
    public class AuthenticationComponent : MonoBehaviour
    {
        /// <summary>
        /// Authentication initiation event
        /// <remarks>
        ///The ScriptableObject event is not used here because it is not useful, it is used once per scene
        /// </remarks>
        /// </summary>
        public UnityEvent<bool> initComplete;
        /// <summary>
        /// Event of the end of the authentication
        /// <remarks>
        ///The ScriptableObject event is not used here because it is not useful, it is used once per scene
        /// </remarks>
        /// </summary>
        public UnityEvent<bool> authComplete;
        
        private void Start()
        {
            InitAuthentication();
        }

        /// <summary>
        /// Initializing authentication
        /// </summary>
        public async void InitAuthentication()
        {
            initComplete?.Invoke((await AuthenticationManager.InitializeAsync()).Success);
        }

        /// <summary>
        /// Anonymous authentication
        /// </summary>
        public async void SignInAnonymously()
        {
            authComplete?.Invoke((await AuthenticationManager.SignInAnonymouslyAsync()).Success);
        }
    }
}
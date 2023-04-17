using System;
using System.Threading.Tasks;
using NetworkFramework.Data;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace NetworkFramework.Managers
{
    /// <summary>
    /// User authentication class
    /// </summary>
    public static class AuthenticationManager
    {
        static AuthenticationManager()
        {
            UnityServices.InitializeAsync().Wait();
        }

        /// <summary>
        /// Initializes UnityServices for authentication
        /// </summary>
        /// <returns>Task with its progress status</returns>
        public static async Task<TaskResult> InitializeAsync()
        {
            if (UnityServices.State == ServicesInitializationState.Initialized)
            {
                return new TaskResult(true);
            }

            await UnityServices.InitializeAsync();
            return new TaskResult(UnityServices.State == ServicesInitializationState.Initialized,
                new Exception("ServicesInitializationState uninitialized"));
        }

        /// <summary>
        /// Anonymous user authentication
        /// </summary>
        /// <returns>Task with its progress status</returns>
        public static async Task<TaskResult> SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                return new TaskResult(true);
            }
            catch (Exception e)
            {
                return new TaskResult(false, e);
            }
        }
    }
}
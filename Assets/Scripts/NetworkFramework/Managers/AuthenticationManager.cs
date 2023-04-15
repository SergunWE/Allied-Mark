using System;
using System.Threading.Tasks;
using NetworkFramework.Data;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace NetworkFramework.Managers
{
    public static class AuthenticationManager
    {
        static AuthenticationManager()
        {
            UnityServices.InitializeAsync().Wait();
        }

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
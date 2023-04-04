using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using TaskStatus = NetworkFramework.Data.TaskStatus;

namespace NetworkFramework.Managers
{
    public static class AuthenticationManager
    {
        static AuthenticationManager()
        {
            UnityServices.InitializeAsync();
        }

        public static async Task<TaskStatus> InitializeAsync()
        {
            if (UnityServices.State == ServicesInitializationState.Initialized)
            {
                return new TaskStatus(true);
            }

            await UnityServices.InitializeAsync();
            return new TaskStatus(UnityServices.State == ServicesInitializationState.Initialized,
                new Exception("ServicesInitializationState uninitialized"));
        }

        public static async Task<TaskStatus> SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                return new TaskStatus(true);
            }
            catch (Exception e)
            {
                return new TaskStatus(false, e);
            }
        }
    }
}
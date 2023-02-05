using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;


namespace NetworkFramework.Managers
{
    public static class AuthenticationManager
    {
        static AuthenticationManager()
        {
            UnityServices.InitializeAsync();
        }

        public static async Task<bool> InitializeAsync()
        {
            if (UnityServices.State == ServicesInitializationState.Initialized)
            {
                return true;
            }

            await UnityServices.InitializeAsync();
            return UnityServices.State == ServicesInitializationState.Initialized;
        }

        public static async Task<bool> SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
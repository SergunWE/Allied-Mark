using System.Threading.Tasks;
using NetworkFramework;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthenticationManager
{
    private bool _isDebug = GlobalConstants.DebugMode;

    public AuthenticationManager()
    {
        UnityServices.InitializeAsync();
    }

    public async Task<bool> InitializeAsync()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            return true;
        }
        await UnityServices.InitializeAsync();
        switch (UnityServices.State)
        {
            case ServicesInitializationState.Initialized:
                SetDebug();
                return true;
            case ServicesInitializationState.Uninitialized:
            case ServicesInitializationState.Initializing:
            default:
                return false;
        }
    }

    public async Task<bool> SignInAnonymouslyAsync()
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

    private void SetDebug()
    {
        if (_isDebug)
        {
            SetAuthCallback();
        }
        else
        {
            UnsetAuthCallback();
        }
    }

    private static void SetAuthCallback()
    {
        AuthenticationService.Instance.SignedIn += OnSignedIn;
        AuthenticationService.Instance.SignInFailed += OnSignInFailed;
        AuthenticationService.Instance.SignedOut += OnSignedOut;
        AuthenticationService.Instance.Expired += OnExpired;
    }
    
    private static void UnsetAuthCallback()
    {
        AuthenticationService.Instance.SignedIn -= OnSignedIn;
        AuthenticationService.Instance.SignInFailed -= OnSignInFailed;
        AuthenticationService.Instance.SignedOut -= OnSignedOut;
        AuthenticationService.Instance.Expired -= OnExpired;
    }
    

    private static void OnSignedIn()
    {
        Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
    }

    private static void OnSignInFailed(RequestFailedException exception)
    {
        Debug.LogError(exception.Message);
    }

    private static void OnSignedOut()
    {
        Debug.Log("Player signed out");
    }
    
    private static void OnExpired()
    {
        Debug.Log("Player session could not be refreshed and expired");
    }
}

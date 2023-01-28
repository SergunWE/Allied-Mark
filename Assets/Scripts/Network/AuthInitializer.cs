using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthInitializer : MonoBehaviour
{
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            if (AuthenticationService.Instance.IsSignedIn)
            {
                SceneManager.LoadSceneAsync(1);
            }
        }
    }
}

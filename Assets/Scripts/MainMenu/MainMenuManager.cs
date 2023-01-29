using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private readonly AuthenticationManager _authenticationManager = new();
    private readonly LobbyManager _lobbyManager = new();

    private async void Awake()
    {
        bool authInit = await _authenticationManager.InitializeAsync();
        Debug.Log($"UnityServices Initialized - {authInit}");
    }

    public async void SignInAnonymously()
    {
        var signIn = await _authenticationManager.SignInAnonymouslyAsync();
        Debug.Log($"Sign In Anonymously - {signIn}");
    }

    public async void CreateLobby()
    {
        var create = await _lobbyManager.CreateLobbyAsync(4, true, null);
        if (create)
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}

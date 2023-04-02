using NetworkFramework.LobbyCore;
using TMPro;
using UnityEngine;

public class LobbyMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text lobbyCodeText;

    private void Start()
    {
        lobbyCodeText.text =  $"Lobby code:{LobbyData.Current.LobbyCode}";
    }
}

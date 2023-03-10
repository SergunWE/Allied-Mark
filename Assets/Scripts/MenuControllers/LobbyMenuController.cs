using NetworkFramework.LobbyCore;
using TMPro;
using UnityEngine;

public class LobbyMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text lobbyCodeText;
    
    void Start()
    {
        lobbyCodeText.text = LobbyData.Current.LobbyCode;
    }
}

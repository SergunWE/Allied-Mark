using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PicturePuzzleTurnView : MonoBehaviour
{
    [SerializeField] private PicturePuzzleNetwork picturePuzzleNetwork;
    [SerializeField] private TMP_Text notificationText;

    private void OnEnable()
    {
        picturePuzzleNetwork.LastPlayerId.OnValueChanged += OnCurrentGridChanged;
    }

    private void OnDisable()
    {
        picturePuzzleNetwork.LastPlayerId.OnValueChanged -= OnCurrentGridChanged;
    }

    private void OnCurrentGridChanged(ulong prevValue, ulong newValue)
    {
        notificationText.text = NetworkManager.Singleton.LocalClientId == newValue
            ? "Ход союзника"
            : "Ваш ход";
    }
}
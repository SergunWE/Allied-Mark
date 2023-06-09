using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PicturePuzzleTurnView : MonoBehaviour
{
    [SerializeField] private PicturePuzzleNetwork picturePuzzleNetwork;
    [SerializeField] private TMP_Text notificationText;

    private void OnEnable()
    {
        OnCurrentGridChanged(0, picturePuzzleNetwork.LastPlayerId.Value);
        picturePuzzleNetwork.LastPlayerId.OnValueChanged += OnCurrentGridChanged;
    }

    private void OnDisable()
    {
        picturePuzzleNetwork.LastPlayerId.OnValueChanged -= OnCurrentGridChanged;
    }

    private void OnCurrentGridChanged(ulong prevValue, ulong newValue)
    {
        if (newValue == ulong.MaxValue)
        {
            notificationText.text = "Ожидание хода";
            return;
        }
        notificationText.text = NetworkManager.Singleton.LocalClientId == newValue
            ? "Ход союзника"
            : "Ваш ход";
    }
}
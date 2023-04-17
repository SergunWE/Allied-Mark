using TMPro;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class PlayerDataDisplayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Renderer readyRenderer;

        public void SetData(string playerName, bool ready)
        {
            nameText.text = playerName;
            readyRenderer.material.color = ready ? Color.green : Color.red;
            gameObject.SetActive(true);
        }

        public void HidePlayer()
        {
            gameObject.SetActive(false);
        }
    }
}
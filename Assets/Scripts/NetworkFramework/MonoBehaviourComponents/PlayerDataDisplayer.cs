using TMPro;
using UnityEngine;

namespace NetworkFramework.MonoBehaviourComponents
{
    /// <summary>
    /// Player data display component
    /// </summary>
    public class PlayerDataDisplayer : MonoBehaviour
    {
        /// <summary>
        /// Player name text field
        /// </summary>
        [SerializeField] private TMP_Text nameText;
        /// <summary>
        /// Color element indicating the player's readiness
        /// </summary>
        [SerializeField] private Renderer readyRenderer;

        /// <summary>
        /// Data Setup
        /// </summary>
        /// <param name="playerName">Player Name</param>
        /// <param name="ready">Player Ready</param>
        public void SetData(string playerName, bool ready)
        {
            nameText.text = playerName;
            readyRenderer.material.color = ready ? Color.green : Color.red;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Hide player display
        /// </summary>
        public void HidePlayer()
        {
            gameObject.SetActive(false);
        }
    }
}
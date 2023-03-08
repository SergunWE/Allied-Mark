using System;
using TMPro;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class PlayerDataDisplayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private Renderer _readyRenderer;

        public void SetData(string playerName, bool ready)
        {
            _nameText.text = playerName;
            _readyRenderer.material.color = ready ? Color.green : Color.red;
            gameObject.SetActive(true);
        }

        public void HidePlayer()
        {
            gameObject.SetActive(false);
        }
    }
}
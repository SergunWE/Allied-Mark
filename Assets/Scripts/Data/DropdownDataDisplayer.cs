using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class DropdownDataDisplayer<T> : DataHandler<T> where T : IUIDisplayed
{
    [SerializeField] protected TMP_Dropdown dropdown;

    private void Awake()
    {
        if (dropdown == null) return;
        
        int levelsCount = data.Count;
        var options = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < levelsCount; i++)
        {
            var element = data[i];
            options.Add(new TMP_Dropdown.OptionData(element.DisplayName));
        }

        dropdown.options = options;
        
    }

    private void OnEnable()
    {
        if(dropdown == null) return;
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDisable()
    {
        if(dropdown == null) return;
        dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }

    protected abstract void OnDropdownValueChanged(int index);
    public abstract void OnLobbyRefreshed();
}
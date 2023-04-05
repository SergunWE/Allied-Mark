using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownDataDisplayer<T> : DataHandler<T> where T : IUIDisplayed
{
    [SerializeField] protected TMP_Dropdown dropdown;

    private void Awake()
    {
        if (dropdown == null) return;
        
        int levelsCount = data.Length;
        var options = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < levelsCount; i++)
        {
            var element = data[i];
            options.Add(new TMP_Dropdown.OptionData(element.DisplayName));
        }

        dropdown.options = options;
    }
}
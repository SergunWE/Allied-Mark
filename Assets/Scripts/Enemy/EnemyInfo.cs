using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Info")]
public class EnemyInfo : ScriptableObject, IUIDisplayed
{
    [field: SerializeField] public string EnemyName { get; private set; }
    [field: SerializeField] public string DisplayedEnemyName { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    public string DisplayName => DisplayedEnemyName;
}
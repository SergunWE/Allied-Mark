using UnityEngine;

public class MarkViewer : MonoBehaviour
{
    [SerializeField] private Material markMaterial;
    [SerializeField] private Material notMarkMaterial;

    [SerializeField] private Renderer markRenderer;

    private void Awake()
    {
        markRenderer.material = notMarkMaterial;
    }

    public void SetMark(int index)
    {
        if (index == 0)
        {
            markRenderer.material = notMarkMaterial;
            return;
        }
        markRenderer.material = markMaterial;
    }
}
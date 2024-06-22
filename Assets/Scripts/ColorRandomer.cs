using UnityEngine;

[RequireComponent (typeof(Renderer))]
public class ColorRandomer : MonoBehaviour
{
    private Renderer _renderer;
    private Color _default;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        _default = _renderer.material.color;
    }

    private void OnEnable()
    {
        SetDefault();
    }

    public void Change()
    {
        _renderer.material.color = UnityEngine.Random.ColorHSV();
    }

    private void SetDefault()
    {
        _renderer.material.color = _default;
    }
}

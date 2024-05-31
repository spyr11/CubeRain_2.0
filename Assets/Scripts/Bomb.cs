using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        // _renderer.material.color = Color.black;
    }

    private void Start()
    {
        StartCoroutine(FadeColor(3f));
    }

    private IEnumerator FadeColor(float seconds)
    {
        Color color = _renderer.material.color;

        for (float i = seconds; i >= 0; i -= Time.deltaTime)
        {
            float number = i / seconds;

            color.a = number;

            _renderer.material.color = color;

            yield return null;
        }
    }
}
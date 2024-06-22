using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ColorFader : MonoBehaviour
{
    private readonly float _destroyMinValue = 2f;
    private readonly float _destroyMaxValue = 6f;

    private Renderer _renderer;

    public event Action ColorChanged;

    public void Init()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = Color.black;
    }

    public void StartFading()
    {
        StartCoroutine(Fade(GetDelayTime()));
    }

    private IEnumerator Fade(float seconds)
    {
        Color color = _renderer.material.color;

        for (float i = seconds; i >= 0; i -= Time.deltaTime)
        {
            float number = i / seconds;

            color.a = number;

            _renderer.material.color = color;

            yield return null;
        }

        ColorChanged?.Invoke();
    }

    private float GetDelayTime()
    {
        return UnityEngine.Random.Range(_destroyMinValue, _destroyMaxValue);
    }
}

using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour, ISpawnable<Cube>
{
    private readonly float _destroyMinValue = 2f;
    private readonly float _destroyMaxValue = 6f;

    private Renderer _renderer;
    private Color _defaultColor;
    private bool _hasHit;

    public event Action<Cube> Disabled;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        _defaultColor = _renderer.material.color;
    }

    private void OnEnable()
    {
        _hasHit = false;

        SetDefaultColor();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<Platform>(out _))
        {
            if (_hasHit == false)
            {
                _hasHit = true;

                StartCoroutine(Hit());
            }
        }
    }

    private IEnumerator Hit()
    {
        ChangeColor();

        yield return new WaitForSeconds(GetDelayTime());

        Disabled?.Invoke(this);
    }

    private void ChangeColor()
    {
        _renderer.material.color = UnityEngine.Random.ColorHSV();
    }

    private void SetDefaultColor()
    {
        _renderer.material.color = _defaultColor;
    }

    private float GetDelayTime()
    {
        return UnityEngine.Random.Range(_destroyMinValue, _destroyMaxValue);
    }
}

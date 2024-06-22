using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorRandomer))]
public class Cube : MonoBehaviour, ISpawnable<Cube>
{
    private readonly float _destroyMinValue = 2f;
    private readonly float _destroyMaxValue = 6f;

    private ColorRandomer _colorRandomer;
    private bool _hasHit;

    public event Action<Cube> Disabled;

    private void Awake()
    {
        _colorRandomer = GetComponent<ColorRandomer>();
    }

    private void OnEnable()
    {
        _hasHit = false;
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
        _colorRandomer.Change();

        yield return new WaitForSeconds(GetDelayTime());

        Disabled?.Invoke(this);
    }

    private float GetDelayTime()
    {
        return UnityEngine.Random.Range(_destroyMinValue, _destroyMaxValue);
    }
}

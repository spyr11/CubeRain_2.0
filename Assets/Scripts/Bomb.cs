using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Renderer))]
public class Bomb : MonoBehaviour, ISpawnable<Bomb>
{
    private readonly float _destroyMinValue = 2f;
    private readonly float _destroyMaxValue = 6f;

    [SerializeField] private float _explosingForce;
    [SerializeField] private float _explosingRadius;

    private Renderer _renderer;

    public event Action<Bomb> Disabled;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = Color.black;
    }

    private void OnEnable()
    {
        StartCoroutine(FadeColor(GetDelayTime()));
    }

    private float GetDelayTime()
    {
        return UnityEngine.Random.Range(_destroyMinValue, _destroyMaxValue);
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

        BlowUp();
    }

    private void BlowUp()
    {
        Explode(GetExplosableObjects());

        Disabled?.Invoke(this);
    }

    private void Explode(List<Rigidbody> targets)
    {
        foreach (Rigidbody target in targets)
        {
            target.AddExplosionForce(_explosingForce, transform.position, _explosingRadius);
        }
    }

    private List<Rigidbody> GetExplosableObjects()
    {
        List<Rigidbody> hits = new List<Rigidbody>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosingRadius);

        hits.AddRange(colliders.Where(hit => hit.attachedRigidbody).Select(hit => hit.attachedRigidbody));

        return hits;
    }
}

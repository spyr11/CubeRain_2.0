using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(ColorFader))]
public class Bomb : MonoBehaviour, ISpawnable<Bomb>
{
    [SerializeField] private float _explosingForce;
    [SerializeField] private float _explosingRadius;

    private ColorFader _colorFader;

    public event Action<Bomb> Disabled;

    private void Awake()
    {
        _colorFader = GetComponent<ColorFader>();

        _colorFader.Init();
    }

    private void OnEnable()
    {
        _colorFader.StartFading();

        _colorFader.Changed += BlowUp;
    }

    private void OnDisable()
    {
        _colorFader.Changed -= BlowUp;
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

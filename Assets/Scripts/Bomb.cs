using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Bomb : MonoBehaviour, ISpawnable
{
    [SerializeField] private float _explosingForce;
    [SerializeField] private float _explosingRadius;

    private Renderer _renderer;

    private float _timer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = Color.black;
    }

    private void Start()
    {
        StartCoroutine(FadeColor(_timer));
    }

    public void SetTimer(float timer)
    {
        _timer = timer;
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
        Explose(GetExplodableObjects());

        Destroy(gameObject);
    }

    private void Explose(List<Rigidbody> targets)
    {
        foreach (Rigidbody target in targets)
        {
            target.AddExplosionForce(_explosingForce, transform.position, _explosingRadius);
        }
    }

    private List<Rigidbody> GetExplodableObjects()
    {
        List<Rigidbody> hits = new List<Rigidbody>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosingRadius);

        foreach (Collider hit in colliders)
        {
            if (hit.attachedRigidbody)
            {
                hits.Add(hit.attachedRigidbody);
            }
        }

        return hits;
    }
}

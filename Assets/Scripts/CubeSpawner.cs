using System;
using System.Collections;
using UnityEngine;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private Collider _floorCollider;
    [SerializeField] private float _delay;

    public event Action<Vector3> Destroyed;

    private void Start()
    {
        StartCoroutine(Spawn(_delay));
    }

    protected override void ActionOnRelease(Cube obj)
    {
        base.ActionOnRelease(obj);

        Destroyed?.Invoke(obj.gameObject.transform.position);
    }

    private IEnumerator Spawn(float delay)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        bool isExecute = true;

        while (isExecute)
        {
            StartSpawn(GetPosition());

            yield return waitForSeconds;
        }
    }

    private Vector3 GetPosition()
    {
        Bounds bound = _floorCollider.bounds;

        float positionX = UnityEngine.Random.Range(bound.min.x, bound.max.x);
        float positionZ = UnityEngine.Random.Range(bound.min.z, bound.max.z);

        return new Vector3(positionX, transform.position.y, positionZ);
    }
}
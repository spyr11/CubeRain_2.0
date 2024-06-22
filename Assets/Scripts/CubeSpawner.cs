using System;
using System.Collections;
using UnityEngine;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private Collider _floorCollider;
    [SerializeField] private float _delay;

    private float _offset;

    public event Action<Vector3> Destroyed;

    private void Start()
    {
        _offset = 0.5f;

        StartCoroutine(Spawn(_delay));
    }

    protected override void ReleaseObject(Cube obj)
    {
        base.ReleaseObject(obj);

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

        float positionX = UnityEngine.Random.Range(bound.min.x + _offset, bound.max.x - _offset);
        float positionZ = UnityEngine.Random.Range(bound.min.z + _offset, bound.max.z - _offset);

        return new Vector3(positionX, transform.position.y, positionZ);
    }
}
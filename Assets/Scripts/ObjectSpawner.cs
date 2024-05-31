using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectSpawner<T> : MonoBehaviour where T : Component
{
    [SerializeField] private Collider _floor;
    [SerializeField] private T _object;
    [SerializeField, Range(0, int.MaxValue)] private float _spawnDelay;

    private int _poolCapacity;
    private int _poolMaxSize;
    private ObjectPool<T> _pool;
    private Vector3 _spawnArea;

    private void Awake()
    {
        _poolCapacity = 10;
        _poolMaxSize = 10;

        _pool = new ObjectPool<T>(
               createFunc: () => Instantiate(_object),
               actionOnGet: (obj) => ActionOnGet(obj),
               actionOnDestroy: (obj) => Destroy(obj.gameObject),
               defaultCapacity: _poolCapacity,
               maxSize: _poolMaxSize
           );
    }

    private void Start()
    {
        StartCoroutine(SpawnObject(_spawnDelay));
    }

    private IEnumerator SpawnObject(float delay)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        bool isExecute = true;

        while (isExecute)
        {
            _pool.Get();

            yield return waitForSeconds;
        }
    }

    private void ActionOnGet(T obj)
    {
        //obj.Disabled += OnDisabled;

        obj.gameObject.SetActive(true);
        obj.gameObject.transform.position = GetPosition();
    }

    private void OnDisabled(T obj)
    {
        //obj.Disabled -= OnDisabled;

        _pool.Release(obj);
    }

    private Vector3 GetPosition()
    {
        Bounds bound = _floor.bounds;

        float positionX = Random.Range(bound.min.x, bound.max.x);
        float positionZ = Random.Range(bound.min.z, bound.max.z);

        return new Vector3(positionX, transform.position.y, positionZ);
    }
}
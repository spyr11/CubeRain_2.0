using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : Component, ISpawnable<T>
{
    [SerializeField] private TextMeshProUGUI _textObjectCount;
    [SerializeField] private T _objectPrefab;

    private int _objectCount;

    private ObjectPool<T> _pool;

    private Vector3 _newPosition;

    private void Awake()
    {
        int poolCapacity = 10;
        int poolMaxSize = 10;

        _pool = new ObjectPool<T>(
                          createFunc: () => Instantiate(_objectPrefab),
                          actionOnGet: (obj) => ActionOnGet(obj),
                          actionOnRelease: (obj) => ActionOnRelease(obj),
                          actionOnDestroy: (obj) => Destroy(obj.gameObject),
                          defaultCapacity: poolCapacity,
                          maxSize: poolMaxSize
                      );
    }

    protected virtual void ActionOnRelease(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected void StartSpawn(Vector3 newPosition)
    {
        _newPosition = newPosition;

        _pool.Get();
    }

    private void ActionOnGet(T obj)
    {
        obj.Disabled += OnDisabled;
        obj.gameObject.SetActive(true);
        obj.gameObject.transform.position = _newPosition;

        ReadObjectCount(obj);
    }

    private void OnDisabled(T obj)
    {
        obj.Disabled -= OnDisabled;

        _pool.Release(obj);
    }

    private void ReadObjectCount(T obj)
    {
        _objectCount++;

        _textObjectCount.text = $" {obj.GetType().ToString()} Total: {_objectCount}  Actve: {_pool.CountActive}";
    }
}
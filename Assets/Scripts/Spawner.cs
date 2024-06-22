using System;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour, ICounter where T : Component, ISpawnable<T>
{
    [SerializeField] private TextMeshProUGUI _textObjectCount;
    [SerializeField] private T _objectPrefab;

    private Vector3 _newPosition;
    private ObjectPool<T> _pool;
    private string _objectName;
    private int _objectCount;

    public event Action<int> ChangedActive;
    public event Action<int> ChangedTotal;

    public string Name => _objectName;

    private void Awake()
    {
        int poolCapacity = 10;
        int poolMaxSize = 10;

        _pool = new ObjectPool<T>(
                          createFunc: InstantiateObject,
                          actionOnGet: SetObjectState,
                          actionOnRelease: ReleaseObject,
                          actionOnDestroy: DestroyObjects,
                          defaultCapacity: poolCapacity,
                          maxSize: poolMaxSize
                      );

        _objectName = _objectPrefab.GetType().ToString();
    }

    protected virtual void ReleaseObject(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected void StartSpawn(Vector3 newPosition)
    {
        _newPosition = newPosition;

        _pool.Get();
    }

    private T InstantiateObject()
    {
        T obj = Instantiate(_objectPrefab);

        obj.Disabled += OnDisabled;

        return obj;
    }

    private void SetObjectState(T obj)
    {
        obj.gameObject.SetActive(true);
        obj.gameObject.transform.position = _newPosition;

        CountObjects(obj);
    }

    private void DestroyObjects(T obj)
    {
        obj.Disabled -= OnDisabled;

        Destroy(obj.gameObject);
    }

    private void OnDisabled(T obj)
    {
        _pool.Release(obj);
    }

    private void CountObjects(T obj)
    {
        _objectCount++;

        ChangedActive?.Invoke(_pool.CountActive);
        ChangedTotal?.Invoke(_objectCount);
    }
}
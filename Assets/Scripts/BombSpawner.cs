using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private void OnEnable()
    {
        _cubeSpawner.Destroyed += StartSpawn;
    }

    private void OnDisable()
    {
        _cubeSpawner.Destroyed -= StartSpawn;
    }
}

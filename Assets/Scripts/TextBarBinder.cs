using UnityEngine;

public class TextBarBinder : MonoBehaviour
{
    [SerializeField] private Spawner _bombSpawner;
    [SerializeField] private Spawner _cubeSpawner;

    [Header("Bomb Bars")]
    [SerializeField] private TextBar _bombInstantiated;
    [SerializeField] private TextBar _bombSpawned;
    [SerializeField] private TextBar _bombActive;

    [Header("Cube Bars")]
    [SerializeField] private TextBar _cubeInstantiated;
    [SerializeField] private TextBar _cubeSpawned;
    [SerializeField] private TextBar _cubeActive;

    private void Awake()
    {
        _bombInstantiated.Bind(
            () => _bombSpawner.ObjectsInstantiated,
            h => _bombSpawner.ObjectsInstantiatedChanged += h,
            h => _bombSpawner.ObjectsInstantiatedChanged -= h
        );

        _bombSpawned.Bind(
            () => _bombSpawner.ObjectsSpawned,
            h => _bombSpawner.ObjectsSpawnedChanged += h,
            h => _bombSpawner.ObjectsSpawnedChanged -= h
        );

        _bombActive.Bind(
            () => _bombSpawner.ObjectsActive,
            h => _bombSpawner.ObjectsActiveChanged += h,
            h => _bombSpawner.ObjectsActiveChanged -= h
        );

        _cubeInstantiated.Bind(
            () => _cubeSpawner.ObjectsInstantiated,
            h => _cubeSpawner.ObjectsInstantiatedChanged += h,
            h => _cubeSpawner.ObjectsInstantiatedChanged -= h
        );

        _cubeSpawned.Bind(
            () => _cubeSpawner.ObjectsSpawned,
            h => _cubeSpawner.ObjectsSpawnedChanged += h,
            h => _cubeSpawner.ObjectsSpawnedChanged -= h
        );

        _cubeActive.Bind(
            () => _cubeSpawner.ObjectsActive,
            h => _cubeSpawner.ObjectsActiveChanged += h,
            h => _cubeSpawner.ObjectsActiveChanged -= h
        );
    }
}

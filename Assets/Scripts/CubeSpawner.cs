using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 10;
    [SerializeField] private float _repeatRate = 3f;
    [SerializeField] private float _spawnHeight = 15f;
    [SerializeField] private Platform _mainPlatform;
    [SerializeField] private float _spawnOffset = 0.5f;
    [SerializeField] private Destroyer _cubeDestroyer;

    private float _spawnPositionMaxX;
    private float _spawnPositionMinX;
    private float _spawnPositionMaxZ;
    private float _spawnPositionMinZ;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => ActionOnRelease(cube),
            actionOnDestroy: (cube) => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
            );

        Renderer platformRenderer = _mainPlatform.GetComponent<Renderer>();

        _spawnPositionMaxX = platformRenderer.bounds.max.x - _spawnOffset;
        _spawnPositionMinX = platformRenderer.bounds.min.x + _spawnOffset;
        _spawnPositionMaxZ = platformRenderer.bounds.max.z - _spawnOffset;
        _spawnPositionMinZ = platformRenderer.bounds.min.z + _spawnOffset;
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0f, _repeatRate);
    }

    public void ReleaseCubeIntoPool(Cube cube)
    {
        _pool.Release(cube);
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.Collided -= _cubeDestroyer.DestroyCube;
        cube.gameObject.SetActive(false);
    }

    private void ActionOnGet(Cube cube)
    {
        Vector3 startPosition = new Vector3(
            Random.Range(_spawnPositionMinX, _spawnPositionMaxX),
            _spawnHeight,
            Random.Range(_spawnPositionMinZ, _spawnPositionMaxZ)
            );
        cube.transform.position = startPosition;
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.gameObject.SetActive(true);

        cube.Collided += _cubeDestroyer.DestroyCube;
    }

    private void GetCube()
    {
        _pool.Get();
    }
}

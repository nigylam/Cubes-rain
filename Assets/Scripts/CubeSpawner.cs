using UnityEngine;

public class CubeSpawner : Spawner
{
    [SerializeField] private float _repeatRate = 3f;
    [SerializeField] private float _spawnHeight = 15f;
    [SerializeField] private Platform _mainPlatform;
    [SerializeField] private float _spawnOffset = 0.5f;

    private float _spawnPositionMaxX;
    private float _spawnPositionMinX;
    private float _spawnPositionMaxZ;
    private float _spawnPositionMinZ;

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0f, _repeatRate);
    }

    protected override void Initialize()
    {
        base.Initialize();

        Renderer platformRenderer = _mainPlatform.GetComponent<Renderer>();
        _spawnPositionMaxX = platformRenderer.bounds.max.x - _spawnOffset;
        _spawnPositionMinX = platformRenderer.bounds.min.x + _spawnOffset;
        _spawnPositionMaxZ = platformRenderer.bounds.max.z - _spawnOffset;
        _spawnPositionMinZ = platformRenderer.bounds.min.z + _spawnOffset;
    }

    protected override void ActionOnGet(DestroyableObject destroyableObject)
    {
        var cube = destroyableObject as Cube;

        Vector3 startPosition = new Vector3(
            Random.Range(_spawnPositionMinX, _spawnPositionMaxX),
            _spawnHeight,
            Random.Range(_spawnPositionMinZ, _spawnPositionMaxZ)
            );

        cube.transform.position = startPosition;
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.gameObject.SetActive(true);

        cube.Collided += DestroyCube;
    }

    protected override void ActionOnRelease(DestroyableObject destroyableObject)
    {
        var cube = destroyableObject as Cube;
        cube.Collided -= DestroyCube;
        cube.gameObject.SetActive(false);
    }

    private void DestroyCube(Cube cube, ContactPoint contact)
    {
        if (cube.IsCollided == false)
            ObjectDestroyer.StartDestroying(cube);
    }

    private void GetCube()
    {
        Pool.Get();
    }
}

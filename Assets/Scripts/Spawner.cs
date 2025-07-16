using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] private DestroyableObject _objectPrefab;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 10;
    [SerializeField] private Destroyer _cubeDestroyer;

    private ObjectPool<DestroyableObject> _pool;

    private void Awake()
    {
        Initialize();
    }
    public void ReleaseObjectIntoPool(DestroyableObject destroyableObject)
    {
        _pool.Release(destroyableObject);
    }

    protected virtual void Initialize()
    {
        _pool = new ObjectPool<DestroyableObject>(
            createFunc: () => Instantiate(_objectPrefab),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => ActionOnRelease(obj),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
            );
    }

    protected virtual void ActionOnRelease(DestroyableObject destroyableObject)
    {
        destroyableObject.gameObject.SetActive(false);
    }

    protected virtual void ActionOnGet(DestroyableObject cube)
    {
        cube.gameObject.SetActive(true);
    }
}

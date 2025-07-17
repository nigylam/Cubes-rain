using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected Destroyer ObjectDestroyer;

    [SerializeField] private DestroyableObject _objectPrefab;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 10;

    protected ObjectPool<DestroyableObject> Pool;

    private void Awake()
    {
        Initialize();
    }

    public void ReleaseObjectIntoPool(DestroyableObject destroyableObject)
    {
        Pool.Release(destroyableObject);
    }

    protected virtual void Initialize()
    {
        Pool = new ObjectPool<DestroyableObject>(
            createFunc: () => Instantiate(_objectPrefab),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => ActionOnRelease(obj),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
            );
    }

    protected virtual void ActionOnGet(DestroyableObject destroyableObject)
    {
        destroyableObject.gameObject.SetActive(true);
    }

    protected virtual void ActionOnRelease(DestroyableObject destroyableObject)
    {
        destroyableObject.gameObject.SetActive(false);
    }
}

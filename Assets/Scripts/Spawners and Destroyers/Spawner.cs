using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected Destroyer ObjectDestroyer;

    [SerializeField] private DestroyableObject _objectPrefab;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 10;

    public int ObjectsInstantiated 
    {
        get
        {
            return _objectsInstantiated;
        } 
        private set
        {
            _objectsInstantiated = value;
            ObjectsInstantiatedChanged?.Invoke(_objectsInstantiated);
        }
    }

    public int ObjectsSpawned 
    {
        get
        {
            return _objectsSpawned;
        }
        private set
        {
            _objectsSpawned = value;
            ObjectsSpawnedChanged?.Invoke(_objectsSpawned);
        } 
    }
    public int ObjectsActive 
    {
        get
        {
            return _objectsActive;
        } 
        private set
        {
            _objectsActive = value;
            ObjectsActiveChanged?.Invoke(_objectsActive);
        }
    }

    public event Action<int> ObjectsInstantiatedChanged;
    public event Action<int> ObjectsSpawnedChanged;
    public event Action<int> ObjectsActiveChanged;

    protected ObjectPool<DestroyableObject> Pool;

    private int _objectsInstantiated = 0;
    private int _objectsSpawned = 0;
    private int _objectsActive = 0;

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
            createFunc: () => ActionOnCreate(),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => ActionOnRelease(obj),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
            );
    }

    private DestroyableObject ActionOnCreate()
    {
        ObjectsInstantiated++;
        return Instantiate(_objectPrefab);
    }

    protected virtual void ActionOnGet(DestroyableObject destroyableObject)
    {
        destroyableObject.gameObject.SetActive(true);
        ObjectsSpawned++;
        ObjectsActive++;
    }

    protected virtual void ActionOnRelease(DestroyableObject destroyableObject)
    {
        destroyableObject.gameObject.SetActive(false);
        ObjectsActive--;
    }
}

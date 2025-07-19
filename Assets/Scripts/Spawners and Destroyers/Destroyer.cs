using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Destroyer : MonoBehaviour
{
    [SerializeField] private Spawner _objectSpawner;
    [SerializeField] private ExplosionEffect _explosionEffectPrefab;
    [SerializeField] private float _destroyTimeMin = 2f;
    [SerializeField] private float _destroyTimeMax = 5f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 20;
    [SerializeField] private float _effectDeleateTime = 4f;

    public event Action<Vector3> Destroyed;

    private ObjectPool<ExplosionEffect> _explosionPrefabPool;
    private float __destroyIterationDelay = 0.1f;
    private WaitForSeconds _effectDeleatingTimeWait;
    private WaitForSeconds _objectDestroyingTimeWait;

    private void Awake()
    {
        Initialize();
    }

    public virtual void StartDestroying(DestroyableObject destroyableObject)
    {
        float timeDelay = UnityEngine.Random.Range(_destroyTimeMin, _destroyTimeMax);
        destroyableObject.SetDeleatingTime(timeDelay);

        StartCoroutine(DestroyAfterDelay(destroyableObject, timeDelay));
    }

    protected virtual void Initialize()
    {
        _objectDestroyingTimeWait = new WaitForSeconds(__destroyIterationDelay);
        _effectDeleatingTimeWait = new WaitForSeconds(_effectDeleateTime);

        _explosionPrefabPool = new ObjectPool<ExplosionEffect>(
            createFunc: () => Instantiate(_explosionEffectPrefab),
            actionOnGet: (explosionEffect) => explosionEffect.gameObject.SetActive(true),
            actionOnRelease: (explosionEffect) => explosionEffect.gameObject.SetActive(false),
            actionOnDestroy: (explosionEffect) => Destroy(explosionEffect.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
            );
    }

    protected virtual void DestroyObject(DestroyableObject destroyableObject)
    {
        Vector3 position = destroyableObject.transform.position;
        _objectSpawner.ReleaseObjectIntoPool(destroyableObject);
        SpawnExplosion(position);
        Destroyed?.Invoke(position);
    }

    private void SpawnExplosion(Vector3 position)
    {
        ExplosionEffect explosion = _explosionPrefabPool.Get();
        explosion.PlayEffect(position);
        StartCoroutine(DeactivateAfterDelay(explosion));
    }

    private IEnumerator DestroyAfterDelay(DestroyableObject destroyableObject, float timeDelay)
    {
        int iterationsCount = Convert.ToInt32(timeDelay / __destroyIterationDelay);

        for (int i = 0; i < iterationsCount; i++)
            yield return _objectDestroyingTimeWait;

        DestroyObject(destroyableObject);
    }

    private IEnumerator DeactivateAfterDelay(ExplosionEffect explosion)
    {
        yield return _effectDeleatingTimeWait;
        _explosionPrefabPool.Release(explosion);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Destroyer : MonoBehaviour
{
    [SerializeField] private Spawner _objectSpawner;
    [SerializeField] private ExplosionEffect _explosionEffectPrefab;
    [SerializeField] private float _deleatingTimeMin = 2f;
    [SerializeField] private float _deleatingTimeMax = 5f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 20;
    [SerializeField] private float _effectDeleatingTime = 4f;

    public event Action<Vector3> Destroyed;

    private List<WaitForSeconds> _objectDeleatingTimes;
    private WaitForSeconds _effectDeleatingTimeWait;
    private ObjectPool<ExplosionEffect> _explosionPrefabPool;

    private void Awake()
    {
        _effectDeleatingTimeWait = new WaitForSeconds(_effectDeleatingTime);

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

    private void Start()
    {
        _objectDeleatingTimes = new List<WaitForSeconds>();

        for (float i = _deleatingTimeMin; i <= _deleatingTimeMax; i++)
        {
            _objectDeleatingTimes.Add(new WaitForSeconds(i));
        }
    }

    public void StartDestroying(DestroyableObject destroyableObject)
    {
        StartCoroutine(DestroyAfterDelay(destroyableObject));
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

    private IEnumerator DestroyAfterDelay(DestroyableObject destroyableObject)
    {
        yield return _objectDeleatingTimes[UnityEngine.Random.Range(0, _objectDeleatingTimes.Count)];
        DestroyObject(destroyableObject);
    }

    private IEnumerator DeactivateAfterDelay(ExplosionEffect explosion)
    {
        yield return _effectDeleatingTimeWait;
        _explosionPrefabPool.Release(explosion);
    }
}
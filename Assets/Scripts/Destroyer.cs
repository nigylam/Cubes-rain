using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private CubeDisappearingEffect _explosionEffectPrefab;
    [SerializeField] private float _cubeDeleatingTimeMin = 2f;
    [SerializeField] private float _cubeDeleatingTimeMax = 5f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 20;
    [SerializeField] private float _effectDeleatingTime = 4f;

    private List<WaitForSeconds> _cubeDeleatingTimes;
    private WaitForSeconds _effectDeleatingTimeWait;

    private ObjectPool<CubeDisappearingEffect> _explosionPrefabPool;

    private void Awake()
    {
        _effectDeleatingTimeWait = new WaitForSeconds(_effectDeleatingTime);

        _explosionPrefabPool = new ObjectPool<CubeDisappearingEffect>(
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
        _cubeDeleatingTimes = new List<WaitForSeconds>();

        for (float i = _cubeDeleatingTimeMin; i <= _cubeDeleatingTimeMax; i++)
        {
            _cubeDeleatingTimes.Add(new WaitForSeconds(i));
        }
    }

    public void DestroyCube(Cube cube, ContactPoint contact)
    {
        if (cube.IsCollided == false)
        {
            StartCoroutine(DestroyAfterDelay(cube));
        }
    }

    private void SpawnExplosion(Vector3 position)
    {
        CubeDisappearingEffect explosion = _explosionPrefabPool.Get();
        explosion.PlayEffect(position);
        StartCoroutine(DeactivateAfterDelay(explosion));
    }

    private IEnumerator DestroyAfterDelay(Cube cube)
    {
        yield return _cubeDeleatingTimes[Random.Range(0, _cubeDeleatingTimes.Count)];
        Vector3 cubePosition = cube.transform.position;
        _cubeSpawner.ReleaseCubeIntoPool(cube);
        SpawnExplosion(cubePosition);
    }

    private IEnumerator DeactivateAfterDelay(CubeDisappearingEffect explosion)
    {
        yield return _effectDeleatingTimeWait;
        _explosionPrefabPool.Release(explosion);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CubeDestroyer : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private CubeExplosionEffect _explosionEffectPrefab;
    [SerializeField] private float _deleatingTimeMin = 2f;
    [SerializeField] private float _deleatingTimeMax = 5f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 20;

    private List<WaitForSeconds> _deleatingTimes;

    private ObjectPool<CubeExplosionEffect> _explosionPrefabPool;

    private void Awake()
    {
        _explosionPrefabPool = new ObjectPool<CubeExplosionEffect>(
            createFunc: () => CreateExplosionEffect(),
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
        _deleatingTimes = new List<WaitForSeconds>();

        for (float i = _deleatingTimeMin; i <= _deleatingTimeMax; i++)
        {
            _deleatingTimes.Add(new WaitForSeconds(i));
        }
    }

    private CubeExplosionEffect CreateExplosionEffect()
    {
        CubeExplosionEffect explosionEfect = Instantiate(_explosionEffectPrefab);
        explosionEfect.SetPool(_explosionPrefabPool);
        return explosionEfect;
    }

    public void DestroyCube(Cube cube, ContactPoint contact)
    {
        if (cube.IsCollided == false)
        {
            StartCoroutine(DestroyAfterDelay(cube));
        }
    }

    private IEnumerator DestroyAfterDelay(Cube cube)
    {
        yield return _deleatingTimes[Random.Range(0, _deleatingTimes.Count)];
        Vector3 cubePosition = cube.transform.position;
        _cubeSpawner.ReleaseCubeIntoPool(cube);
        SpawnExplosion(cubePosition);
    }

    private void SpawnExplosion(Vector3 position)
    {
        CubeExplosionEffect explosion = _explosionPrefabPool.Get();
        explosion.PlayEffect(position);
    }
}

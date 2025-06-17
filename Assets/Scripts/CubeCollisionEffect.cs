using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Cube))]
public class CubeCollisionEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private float _impulseDecrease = 0.001f;

    private int _poolCapacity = 20;
    private int _poolMaxSize = 30;
    private float _effectDestroyingDelay = 0.6f;
    private WaitForSeconds _effectDestroyingDelayWait;

    private Cube _cube;

    private ObjectPool<ParticleSystem> _effectPool;

    private void Awake()
    {
        _cube = GetComponent<Cube>();
        _effectDestroyingDelayWait = new WaitForSeconds(_effectDestroyingDelay);

        _effectPool = new ObjectPool<ParticleSystem>(
            createFunc: () => Instantiate(_effect),
            actionOnGet: (effect) => effect.gameObject.SetActive(true),
            actionOnRelease: (effect) => effect.gameObject.SetActive(false),
            actionOnDestroy: (effect) => Destroy(effect.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
            );
    }

    private void OnEnable()
    {
        _cube.Collided += _spawnEffect;
    }

    private void OnDisable()
    {
        _cube.Collided -= _spawnEffect;
    }

    private void _spawnEffect(Cube cube, ContactPoint contact)
    {
        ParticleSystem effect = _effectPool.Get();
        effect.transform.position = contact.point;
        ParticleSystem.MainModule effectMain = effect.main;
        effectMain.startSize = contact.impulse.magnitude * _impulseDecrease;
        StartCoroutine(DestroyEffectAfterDelay(effect));
    }

    private IEnumerator DestroyEffectAfterDelay(ParticleSystem effect)
    {
        yield return _effectDestroyingDelayWait;
        _effectPool.Release(effect);
    }
}

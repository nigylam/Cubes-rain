using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CubeExplosionEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _deleatingTime = 4f;

    private ObjectPool<CubeExplosionEffect> _pool;
    private WaitForSeconds _deleatingTimeWait;

    private void Awake()
    {
        _deleatingTimeWait = new WaitForSeconds(_deleatingTime);
    }

    public void SetPool(ObjectPool<CubeExplosionEffect> pool)
    {
        _pool = pool;
    }

    public void PlayEffect(Vector3 position)
    {
        transform.position = position;
        _particleSystem.Play();
        _audioSource.Play();

        StartCoroutine(DeactivateAfterDelay());
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return _deleatingTimeWait;
        _pool.Release(this);
    }
}

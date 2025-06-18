using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubeExplosionEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private AudioSource _audioSource;

    public void PlayEffect(Vector3 position)
    {
        transform.position = position;
        _particleSystem.Play();
        _audioSource.Play();
    }
}

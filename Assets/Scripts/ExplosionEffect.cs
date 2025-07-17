using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;

    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayEffect(Vector3 position)
    {
        transform.position = position;
        _particleSystem.Play();
        _audioSource.Play();
    }
}

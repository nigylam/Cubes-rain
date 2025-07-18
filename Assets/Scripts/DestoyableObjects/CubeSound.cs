using UnityEngine;

[RequireComponent(typeof(Cube))]
[RequireComponent(typeof(AudioSource))]
public class CubeSound : MonoBehaviour
{
    [SerializeField] private AudioClip _collisionClip;
    [SerializeField] private float _collisionPitchMin;
    [SerializeField] private float _collisionPitchMax;

    private Cube _cube;
    private AudioSource _audioSource;

    private void Awake()
    {
        _cube = GetComponent<Cube>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _audioSource.Stop();
        _cube.Collided += PlayCollisionSound;
    }

    private void OnDisable()
    {
        _cube.Collided -= PlayCollisionSound;
    }

    private void PlayCollisionSound(Cube cube, ContactPoint contact)
    {
        _audioSource.clip = _collisionClip;
        _audioSource.pitch = Random.Range(_collisionPitchMin, _collisionPitchMax);
        _audioSource.Play();
    }
}

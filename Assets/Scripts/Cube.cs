using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private readonly string IsCollidedName = "isCollided";

    public event Action<Cube, ContactPoint> Collided;
    public event Action Disabled;

    private Animator _animator;

    private bool _isCollided = false;

    public bool IsCollided => _isCollided;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(IsCollidedName, _isCollided);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Platform>() == null)
            return;

        Collided?.Invoke(this, collision.GetContact(0));

        _isCollided = true;
    }

    private void OnDisable()
    {
        Disabled?.Invoke();
        _isCollided = false;
    }
}

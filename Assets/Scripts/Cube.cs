using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    public event Action<Cube, ContactPoint> Collided;
    public event Action Disabled;

    private bool _isCollided = false;

    public bool IsCollided => _isCollided;

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

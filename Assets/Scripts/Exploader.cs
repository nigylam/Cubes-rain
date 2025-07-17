using System.Collections.Generic;
using UnityEngine;

public class Exploader : MonoBehaviour
{
    [SerializeField] private float _force = 500f;
    [SerializeField] private float _radius = 10f;

    public void Explode(Vector3 position)
    {
        List<Rigidbody> objectsToExplode = GetExplodableObjects(position, _radius);

        foreach (Rigidbody obj in objectsToExplode)
        {
            if (obj != null && obj.gameObject.activeInHierarchy)
            {
                obj.AddExplosionForce(_force, position, _radius);
            }
        }
    }

    private List<Rigidbody> GetExplodableObjects(Vector3 position, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius);

        List<Rigidbody> objectsToExplode = new();

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out DestroyableObject destroyableObject))
                objectsToExplode.Add(destroyableObject.Rigidbody);
        }

        return objectsToExplode;
    }
}

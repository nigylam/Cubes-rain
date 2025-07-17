using UnityEngine;

public class BombDestroyer : Destroyer
{
    [SerializeField] private Exploader _exploader;

    protected override void DestroyObject(DestroyableObject destroyableObject)
    {
        Vector3 position = destroyableObject.transform.position;
        base.DestroyObject(destroyableObject);
        _exploader.Explode(position);
    }
}

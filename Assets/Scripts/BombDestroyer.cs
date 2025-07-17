using UnityEngine;

public class BombDestroyer : Destroyer
{
    public override void DestroyObject(DestroyableObject destroyableObject)
    {
        StartCoroutine(DestroyAfterDelay(destroyableObject));
    }
}

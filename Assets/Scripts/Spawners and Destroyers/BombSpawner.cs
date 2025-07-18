using UnityEngine;

public class BombSpawner : Spawner
{
    [SerializeField] private CubeDestroyer _cubeDestroyer;

    private Vector3 _spawnPosition;

    private void OnEnable()
    {
        _cubeDestroyer.Destroyed += Spawn;
    }

    private void OnDisable()
    {
        _cubeDestroyer.Destroyed -= Spawn;
    }

    protected override void ActionOnGet(DestroyableObject destroyableObject)
    {
        base.ActionOnGet(destroyableObject);
        destroyableObject.gameObject.transform.position = _spawnPosition;
        ObjectDestroyer.StartDestroying(destroyableObject);
    }

    private void Spawn(Vector3 position)
    {
        _spawnPosition = position;
        Pool.Get();
    }
}

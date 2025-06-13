using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroyer : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private float _deleatingTimeMin = 2f;
    [SerializeField] private float _deleatingTimeMax = 5f;

    private List<WaitForSeconds> _deleatingTimes;

    private void Start()
    {
        _deleatingTimes = new List<WaitForSeconds>();

        for (float i = _deleatingTimeMin; i <= _deleatingTimeMax; i++)
        {
            _deleatingTimes.Add(new WaitForSeconds(i));
        }
    }

    public void DestroyCube(Cube cube)
    {
        if (cube.IsCollided == false)
        {
            StartCoroutine(DestroyAfterDelay(cube));
        }
    }

    private IEnumerator DestroyAfterDelay(Cube cube)
    {
        yield return _deleatingTimes[Random.Range(0, _deleatingTimes.Count)];
        _cubeSpawner.ReleaseCubeIntoPool(cube);
    }
}

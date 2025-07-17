using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroyer : Destroyer
{
    public override void DestroyObject(DestroyableObject destroyableObject)
    {
        var cube = destroyableObject as Cube;

        if (cube.IsCollided == false)
        {
            StartCoroutine(DestroyAfterDelay(cube));
        }
    }
}

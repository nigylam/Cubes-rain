using System;
using System.Collections;
using UnityEngine;

public class BombDestroyer : Destroyer
{
    [SerializeField] private Exploader _exploader;

    private WaitForSeconds _transparencyingOneStepDelay;
    private float _transparencyingOneStep = 0.1f;

    public override void StartDestroying(DestroyableObject destroyableObject)
    {
        base.StartDestroying(destroyableObject);

        var bomb = destroyableObject as Bomb;
        StartCoroutine(SmoothlyMakeTransparent(bomb.DeleatingTime, bomb.Material));
    }

    protected override void Initialize()
    {
        base.Initialize();
        _transparencyingOneStepDelay = new WaitForSeconds(_transparencyingOneStep);
    }

    protected override void DestroyObject(DestroyableObject destroyableObject)
    {
        Vector3 position = destroyableObject.transform.position;
        var bomb = destroyableObject as Bomb;
        bomb.Material.color = new Color(bomb.Material.color.r, bomb.Material.color.g, bomb.Material.color.b, 1f);
        RenderingModeChanger.SetMaterialRenderingMode(bomb.Material, RenderingMode.Opaque);

        base.DestroyObject(destroyableObject);

        _exploader.Explode(position);
    }

    private IEnumerator SmoothlyMakeTransparent(float seconds, Material material)
    {
        RenderingModeChanger.SetMaterialRenderingMode(material, RenderingMode.Transparent);

        int numberOfSteps = Convert.ToInt32(seconds / _transparencyingOneStep);
        float stepTransparencyIncrease = material.color.a / numberOfSteps;

        for (int i = 0; i < numberOfSteps; i++)
        {
            Color color = material.color;
            color.a -= stepTransparencyIncrease;
            material.color = color;
            yield return _transparencyingOneStepDelay;
        }
    }
}

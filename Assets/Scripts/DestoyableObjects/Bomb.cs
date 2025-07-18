using UnityEngine;

[RequireComponent (typeof(MeshRenderer))]
public class Bomb : DestroyableObject 
{
    public Material Material { get; private set; }

    protected override void Initialize()
    {
        base.Initialize();
        Material = GetComponent<MeshRenderer>().material;
    }
}

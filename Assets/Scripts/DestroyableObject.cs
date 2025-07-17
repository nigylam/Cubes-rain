using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DestroyableObject : MonoBehaviour
{
    public Rigidbody Rigidbody {  get; private set; }

    private void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class DestroyableObject : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    public float DeleatingTime { get; private set; }

    private void Awake()
    {
        Initialize();
    }

    public void SetDeleatingTime(float deleatingTime)
    {
        DeleatingTime = deleatingTime;
    }

    protected virtual void Initialize()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
}

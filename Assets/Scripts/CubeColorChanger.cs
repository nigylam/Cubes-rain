using UnityEngine;

[RequireComponent(typeof(Cube))]
[RequireComponent(typeof(Animator))]
public class CubeColorChanger : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private readonly string IsCollidedName = "isCollided";

    private Cube _cube;

    private void Awake()
    {
        _cube = GetComponent<Cube>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _cube.Collided += SetDestroyingColor;
        _cube.Disabled += SetBaseColor;
    }

    private void OnDisable()
    {
        _cube.Collided -= SetDestroyingColor;
        _cube.Disabled -= SetBaseColor;
    }

    private void SetDestroyingColor(Cube cube, ContactPoint contact)
    {
        _animator.SetBool(IsCollidedName, true);
    }

    private void SetBaseColor()
    {
        _animator.SetBool(IsCollidedName, false);
    }
}

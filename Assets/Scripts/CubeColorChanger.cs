using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cube))]
public class CubeColorChanger : MonoBehaviour
{
    [SerializeField] private Material _destroyingColorMaterial;
    [SerializeField] private Material _baseColorMaterial;

    private Cube _cube;
    private Renderer _renderer;

    private void Awake()
    {
        _cube = GetComponent<Cube>();
        _renderer = GetComponent<Renderer>();
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
        if (_renderer.material == _destroyingColorMaterial)
            return;

        _renderer.material = _destroyingColorMaterial;
    }

    private void SetBaseColor()
    {
        _renderer.material = _baseColorMaterial;
    }
}

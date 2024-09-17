using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialChanger : MonoBehaviour
{
    private Renderer _renderer;

    private Color _originalColor;
    private Color _selectedColor = Color.green;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
    }

    public void Highlight() =>
        _renderer.material.color = _selectedColor;

    public void RemoveHighlight() =>
        _renderer.material.color = _originalColor;
}

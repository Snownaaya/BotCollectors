using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private const int SelectMouseButton = 0;
    private const int DeselectMouseButton = 1;

    [SerializeField] private Camera _camera;

    [SerializeField] private LayerMask _baseLayer;
    [SerializeField] private LayerMask _groundLayer;

    private Flag _selectedFlag;
    private Base _selectedBase;
    private MaterialChanger _materialChanger;

    private bool _isSettingFlag = true;

    private void Update()
    {
        if (Input.GetMouseButtonDown(SelectMouseButton))
        {
            if (_isSettingFlag && _selectedBase != null)
                SetFlagOnGround();
            else
                SelectBase();
        }
        else if (Input.GetMouseButtonDown(DeselectMouseButton))
        {
            if (_selectedBase != null)
                CancelFlagPlacment();
        }
    }

    private void SelectBase()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _baseLayer))
        {
            if (hit.collider.TryGetComponent(out Base @base) && hit.collider.TryGetComponent(out MaterialChanger material))
            {
                _selectedBase = @base;
                _selectedFlag = @base.GetFlag;
                _materialChanger = material;
                _materialChanger.Highlight();
            }

            _isSettingFlag = true;
        }
    }

    private void CancelFlagPlacment()
    {
        _selectedFlag.TurnOff();
        _selectedBase.CanceledConstruction();

        DeselectBase();
    }

    private void DeselectBase()
    {
        _materialChanger.RemoveHighlight();
        _materialChanger = null;
        _selectedBase = null;
        _isSettingFlag = false;
    }

    private void SetFlagOnGround()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundLayer))
        {
            _selectedBase.SetFlagPosition(hit.point);
            _isSettingFlag = true;
            DeselectBase();
        }
    }
}
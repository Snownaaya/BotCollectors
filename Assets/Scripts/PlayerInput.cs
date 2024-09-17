using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private const int LeftMouseButton = 0;
    private const int RightMouseButton = 1;

    [SerializeField] private Camera _camera;
    [SerializeField] private FlagSpawner _flagSpawner;

    [SerializeField] private LayerMask _baseLayer;
    [SerializeField] private LayerMask _groundLayer;

    private Base _selectedBase;
    private bool _isSettingFlag = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(LeftMouseButton))
        {
            if (_isSettingFlag)
                SetFlagOnGround();
            else
                TrySelectBase();
        }
        else if(Input.GetMouseButtonDown(RightMouseButton))
        {
            DeselectBase();
        }
    }

    private void TrySelectBase()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _baseLayer))
        {
            Base newBase = hit.collider.GetComponent<Base>();

            if (newBase != null)
            {
                SelectBase(newBase);
            }
        }
    }

    private void SetFlagOnGround()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundLayer))
        {
            _selectedBase.SetFlagPosition(hit.point);
            _isSettingFlag = false;
        }
    }

    private void SelectBase(Base newBase)
    {
        if (_selectedBase != null)
            _selectedBase.SetSelect(false);

        _selectedBase = newBase;
        _selectedBase.SetSelect(true);

        _isSettingFlag = true;
    }

    private void DeselectBase()
    {
        if (_selectedBase != null)
        {
            _selectedBase.SetSelect(false);
            _selectedBase = null;
            _isSettingFlag = false;
        }
    }
}

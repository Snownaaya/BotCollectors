using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private FlagSpawner _flagSpawner;

    [SerializeField] private LayerMask _baseLayer;
    [SerializeField] private LayerMask _groundLayer;

    private Base _base;
    private bool _isSettingFlag = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isSettingFlag)
                SetFlagOnGround();
            else
                CheckBaseClick();
        }
    }

    private void CheckBaseClick()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _baseLayer))
        {
            _base = hit.collider.GetComponent<Base>();

            if (_base != null)
                _isSettingFlag = true;
        }
    }

    private void SetFlagOnGround()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundLayer))
        {
            _base.SetFlapPosition(hit.point);
            _isSettingFlag = false;
        }
    }
}
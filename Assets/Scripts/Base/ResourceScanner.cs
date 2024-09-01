using UnityEngine;
using System;

[RequireComponent(typeof(Base))]
public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private LayerMask _resourceLayer;
    [SerializeField] private float _scanRadius;

    private Base _base;

    public event Action<Resource> OnResourceFound;

    private void Awake() =>
        _base = GetComponent<Base>();

    public void ScanForResources()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _scanRadius, _resourceLayer);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Resource resource))
                OnResourceFound?.Invoke(resource);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }
}
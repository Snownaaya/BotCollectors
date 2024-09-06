using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Base))]
public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private LayerMask _resourceLayer;
    [SerializeField] private float _scanRadius;

    public List<Resource> ScanForResources()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _scanRadius, _resourceLayer);

        List<Resource> resources = new List<Resource>();

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Resource resource))
                resources.Add(resource);
        }

        return resources;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }
}
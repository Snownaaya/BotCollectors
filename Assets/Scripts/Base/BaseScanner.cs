using UnityEngine;

public class BaseScanner : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [SerializeField] private Base _base;

    [SerializeField] private float _scanRadius;

    public void ScanForResources()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _scanRadius);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Resource resource))
                _base.AssignBotToResource(resource);
        }
    }
}
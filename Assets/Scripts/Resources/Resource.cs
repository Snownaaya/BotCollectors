using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Resource : MonoBehaviour
{
    private Collider _collider;

    public int Amount { get; private set; }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        Amount = 1;
    }

    public void PickUp(Transform collector)
    {
        _collider.enabled = false;
        transform.SetParent(collector);
        transform.localPosition = Vector3.zero; 
        transform.localRotation = Quaternion.identity; 
    }

    public void Release()
    {
        _collider.enabled = true;
        transform.SetParent(null);
    }
}
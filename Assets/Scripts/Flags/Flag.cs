using UnityEngine;
using System;

public class Flag : MonoBehaviour
{
    public event Action OnSetFlag;

    public void SetPosition(Vector3 position)
    {
        transform.position = position; 
        OnSetFlag?.Invoke(); 
    }

    public void SetActive(bool isActive) =>
        gameObject.SetActive(isActive);

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }
}

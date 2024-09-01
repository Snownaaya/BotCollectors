using UnityEngine;
using System;

public class Flag : MonoBehaviour, IFlag
{
    public bool IsPlaced { get; private set; }

    public event Action OnSetFlag;

    public void SetPosition(Vector3 position)
    {
        IsPlaced = true;
        transform.position = position; 
        OnSetFlag?.Invoke(); 
        Debug.Log("Flag position set.");
    }

    public void SetActive(bool isActive) =>
        gameObject.SetActive(isActive);

    public void TurnOff()
    {
        IsPlaced = false;
        gameObject.SetActive(false);
    }
}

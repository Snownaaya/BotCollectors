using UnityEngine;
using System;

public class Flag : MonoBehaviour
{
    public event Action Setted;

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        Setted?.Invoke();
    }

    public void SetActive(bool isActive) =>
        gameObject.SetActive(isActive);

    public void TurnOff() =>
        gameObject.SetActive(false);
}

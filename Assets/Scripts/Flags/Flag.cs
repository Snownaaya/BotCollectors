using UnityEngine;
using System;

public class Flag : MonoBehaviour
{
    public event Action Setted;

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        Setted?.Invoke();
    }

    public void TurnOff() =>
        gameObject.SetActive(false);
}
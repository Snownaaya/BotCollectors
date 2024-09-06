using UnityEngine;

public class FlagHandler : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    public Flag CurrentFlag { get; private set; }

    private void Awake()
    {
        CurrentFlag = Instantiate(_flagPrefab);
        CurrentFlag.SetActive(false);
    }

    public void SetFlagPosition(Vector3 position)
    {
        CurrentFlag.SetPosition(position); 
        CurrentFlag.SetActive(true);
    }
}

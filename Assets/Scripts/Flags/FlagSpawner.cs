using UnityEngine;

public class FlagSpawner : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    public Flag CurrentFlag { get; private set; }

    public Flag Create()
    {
        CurrentFlag = Instantiate(_flagPrefab);
        CurrentFlag.gameObject.SetActive(false);
        return CurrentFlag;
    }
}

using UnityEngine;

public class FlagSpawner : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    public Flag Create()
    {
        Flag newFlag = Instantiate(_flagPrefab);
        newFlag.gameObject.SetActive(false);
        return newFlag;
    }
}

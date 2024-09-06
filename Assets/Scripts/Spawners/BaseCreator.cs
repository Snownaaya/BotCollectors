using UnityEngine;
using Zenject;

public class BaseCreator : MonoBehaviour
{
    [Inject] private DiContainer _container;

    [SerializeField] private Base _basePrefab;
    
    public Base CreateBase(Vector3 position)
    {
        Base newBase = _container.InstantiatePrefabForComponent<Base>(_basePrefab, position, Quaternion.identity, null);
        return newBase;
    }
}
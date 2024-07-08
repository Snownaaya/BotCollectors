using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using System;

public class ResourcePool : MonoBehaviour
{
    [SerializeField] private Resource _item;
    [SerializeField] private Transform _container;

    [SerializeField] private float _horizontalMinBounds;
    [SerializeField] private float _horizontalMaxBounds;
    [SerializeField] private float _verticalMinBounds;
    [SerializeField] private float _verticalMaxBounds;

    private ObjectPool<Resource> _pool;

    public event Action CountChaged;

    public int ResourceCount { get; private set; }

    private float _delay = 3f;

    private void Awake()
    {
        _pool = new ObjectPool<Resource>(() =>
        {
            return Instantiate(_item);
        }, shape =>
        {
            shape.gameObject.SetActive(true);
        }, shape =>
        {
            shape.gameObject.SetActive(false);
        });
    }

    private void Start() =>
        StartCoroutine(Spawn());

    public void ReturnItem(Resource item)
    {
        item.gameObject.SetActive(false);

        ResourceCount++;
        CountChaged?.Invoke();
    }

    private void SpawnItem()
    {
        Resource item = _pool.Get();

        item.transform.position = RandomPosition();

        item.transform.SetParent(_container);
    }

    private IEnumerator Spawn()
    {
        var wait = new WaitForSeconds(_delay);

        while (enabled)
        {
            SpawnItem();
            yield return wait;
        }
    }

    private Vector3 RandomPosition()
    {
        float positionX = UnityEngine.Random.Range(_horizontalMinBounds, _horizontalMaxBounds);
        float positionZ = UnityEngine.Random.Range(_verticalMinBounds, _verticalMaxBounds);

        Vector3 loacPosition = new Vector3(positionX, 0f, positionZ);

        return transform.TransformPoint(loacPosition);
    }
}
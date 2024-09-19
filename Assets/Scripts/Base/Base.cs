using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Zenject;
using System;

[RequireComponent(typeof(ResourceStorage), typeof(ResourceScanner), typeof(FlagSpawner))]
public class Base : MonoBehaviour
{
    [Inject]
    private ResourceSpawner _resourcePool;

    [SerializeField] private BotCreator _botCreateor;
    [SerializeField] private List<StateMachine> _bots = new List<StateMachine>();
    [SerializeField] private ResourceStorage _resourceStorage;

    [SerializeField] private int _requiredResourcesForBase = 5;
    [SerializeField] private int _resourcesForNewBot = 3;

    private HashSet<Resource> _assignedResources = new HashSet<Resource>();

    private FlagSpawner _flagSpawner;
    private ResourceScanner _resourceScanner;
    private WaitForSeconds _scanDelay;
    private Flag _currentFlag;

    private bool _isConstruct;

    public event Action CountChanged;

    public Flag GetFlag => _currentFlag;
    public int CurrentResource => _resourceStorage.CurrentResource;

    private void Awake()
    {
        _resourceScanner = GetComponent<ResourceScanner>();
        _resourceStorage = GetComponent<ResourceStorage>();
        _flagSpawner = GetComponent<FlagSpawner>();

        _scanDelay = new WaitForSeconds(1f);
    }

    private void Start()
    {
        StartCoroutine(ScanAndAssignResources());
        _currentFlag = _flagSpawner.Create();
        _currentFlag.Setted += OnSetFlag;
    }

    private void OnDestroy() =>
        _currentFlag.Setted -= OnSetFlag;

    public void Init(StateMachine initialBot, ResourceSpawner resource)
    {
        _bots.Clear();
        AddBot(initialBot);
        _resourcePool = resource;
    }

    public void CanceledConstruction() =>
        _isConstruct = false;

    public void OnSetFlag() =>
        _isConstruct = true;

    public void ResourceCollect(Resource resource)
    {
        _resourceStorage.AddResource(resource.Amount);
        _assignedResources.Remove(resource);
        _resourcePool.ReturnItem(resource);

        ProccesResourceCollect();
        CountChanged?.Invoke();
    }

    public void CompleteConstruction(StateMachine bot)
    {
        Base newBase = bot.GetComponent<Bot>().BuildNewBase(_currentFlag);

        bot.SetHome(newBase);

        _bots.RemoveAt(0);
        _isConstruct = false;
    }

    private void AddBot(StateMachine bot)
    {
        if (!_bots.Contains(bot))
            _bots.Add(bot);
    }

    public void SetFlagPosition(Vector3 position) =>
        _currentFlag.SetPosition(position);

    private void ProccesResourceCollect()
    {
        if (_isConstruct)
        {
            if (CurrentResource >= _requiredResourcesForBase)
                SetBaseToConstruct();
        }
        else
        {
            if (CurrentResource >= _resourcesForNewBot)
                CreateNewBot();
        }
    }

    private void SetBaseToConstruct()
    {
        if (_currentFlag == null)
            return;

        _isConstruct = true;

        if (_bots.Count > 0)
            _bots[0].StartConstructingBase(this, _currentFlag);

        _resourceStorage.SpendResource(_requiredResourcesForBase);
    }

    private void CreateNewBot()
    {
        StateMachine newBot = _botCreateor.CreateBot();

        Vector3 spawnPosition = transform.position + new Vector3(0, 0, 1);
        newBot.transform.position = spawnPosition;

        _resourceStorage.SpendResource(_resourcesForNewBot);
        newBot.SetHome(this);
        AddBot(newBot);
    }

    private IEnumerator ScanAndAssignResources()
    {
        while (enabled)
        {
            List<Resource> resources = _resourceScanner.ScanForResources();

            foreach (var resource in resources)
                AssignBotToResource(resource);

            yield return _scanDelay;
        }
    }

    private void AssignBotToResource(Resource resource)
    {
        if (_assignedResources.Contains(resource))
            return;

        foreach (StateMachine bot in _bots)
        {
            if (bot.IsIdle && _resourceStorage.TryRequestResource(resource))
            {
                _assignedResources.Add(resource);
                bot.StartMove(resource);
            }
        }
    }
}
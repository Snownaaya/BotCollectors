using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;
using System;

[RequireComponent(typeof(ResourceStorage), typeof(ResourceScanner), typeof(FlagSpawner))]
[RequireComponent(typeof(Renderer))]
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

    private Renderer _renderer;
    private Color _originalColor;
    private Color _selectedColor = Color.green;

    private int _collectedResources = 0;
    private bool IsConstruct;

    public event Action CountChanged;

    public int CollectedResource => _collectedResources;
    private Flag CurrentFlag => _flagSpawner?.CurrentFlag;


    private void Awake()
    {
        _resourceScanner = GetComponent<ResourceScanner>();
        _resourceStorage = GetComponent<ResourceStorage>();
        _flagSpawner = GetComponent<FlagSpawner>();
        _renderer = GetComponent<Renderer>();

        _originalColor = _renderer.material.color;
        _scanDelay = new WaitForSeconds(1f);
    }

    private void Start()
    {
        StartCoroutine(ScanAndAssignResources());
        CurrentFlag.Setted += OnSetFlag;
    }

    private void OnDisable() =>
        CurrentFlag.Setted -= OnSetFlag;

    public void SetSelect(bool isSelect)
    {
        if (isSelect)
        {
            _renderer.material.color = _selectedColor;
        }
        else
        {
            _renderer.material.color = _originalColor;
            CurrentFlag.TurnOff();
        }
    }

    public void Init(StateMachine initialBot, ResourceSpawner resource)
    {
        _bots.Clear();
        AddBot(initialBot);
        _resourcePool = resource;
    }

    public void OnSetFlag() =>
        IsConstruct = true;

    public void ResourceCollect(Resource resource)
    {
        _collectedResources++;

        _resourceStorage.AddResource(resource.Amount);
        _assignedResources.Remove(resource);
        _resourcePool.ReturnItem(resource);

        ProccesResourceCollect();
        CountChanged?.Invoke();
    }

    public void AddBot(StateMachine bot)
    {
        if (!_bots.Contains(bot))
            _bots.Add(bot);
    }

    public void CompleteConstruction(StateMachine bot)
    {
        Base newBase = bot.GetComponent<Bot>().BuildNewBase(CurrentFlag);
        newBase.Init(bot, _resourcePool);

        _resourceStorage.SpendResource(_requiredResourcesForBase);
        bot.SetHome(newBase);
        newBase.AddBot(bot);

        _bots.RemoveAt(0);
        IsConstruct = false;
    }

    public void SetFlagPosition(Vector3 position) =>
        _flagSpawner.SetFlagPosition(position);

    private void ProccesResourceCollect()
    {
        if (IsConstruct)
        {
            if (CurrentFlag != null && _collectedResources % _requiredResourcesForBase == 0)
                SetBaseToConstruct();
        }
        else
        {
            if (_collectedResources % _resourcesForNewBot == 0)
                CreateNewBot();
        }
    }

    private void SetBaseToConstruct()
    {
        if (CurrentFlag == null)
            return;

        IsConstruct = false;

        if (_bots.Count > 0)
            _bots[0].StartConstructingBase(this, CurrentFlag);
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
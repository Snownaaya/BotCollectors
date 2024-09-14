using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;
using System;

[RequireComponent(typeof(ResourceStorage), typeof(ResourceScanner), typeof(FlagSpawner))]
public class Base : MonoBehaviour
{
    [Inject]
    private ResourceSpawner _resourcePool;

    public const int RequiredResourcesForBase = 5;
    public const int ResourcesForNewBot = 3;

    [SerializeField] private BotCreator _botCreateor;
    [SerializeField] private List<StateMachine> _bots = new List<StateMachine>();
    [SerializeField] private ResourceStorage _resourceStorage;

    private HashSet<Resource> _assignedResources = new HashSet<Resource>();

    private FlagSpawner _flagSpawner;
    private ResourceScanner _resourceScanner;
    private WaitForSeconds _scanDelay;

    private int _collectedResources = 0;
    private bool IsConstructing;

    public event Action CountChanged;

    private Flag CurrentFlag => _flagSpawner?.CurrentFlag;
    public int CollectedResource => _collectedResources;


    private void Awake()
    {
        _resourceScanner = GetComponent<ResourceScanner>();
        _resourceStorage = GetComponent<ResourceStorage>();
        _flagSpawner = GetComponent<FlagSpawner>();

        _scanDelay = new WaitForSeconds(3f);
    }

    private void Start()
    {
        _resourceStorage.AddResource(8);
        StartCoroutine(SvanAndAssignResources());
        CurrentFlag.Setted += OnSetFlag;
    }

    private void OnDisable() =>
        CurrentFlag.Setted -= OnSetFlag;

    public void Init(StateMachine initialBot, ResourceSpawner resource)
    {
        _bots.Clear();
        AddBot(initialBot);
        _resourcePool = resource;
    }

    public void OnSetFlag() =>
        IsConstructing = true;

    public void AssignBotToResource(Resource resource)
    {
        if (_resourceStorage.RequestResource(resource))
        {
            List<StateMachine> idleBots = GetIdleBots();

            if (idleBots.Count > 0)
            {
                _assignedResources.Add(resource);
                StateMachine bot = idleBots[0];
                bot.StartMove(resource);
            }
        }
    }

    public void ResourceCollect(Resource resource)
    {
        _collectedResources++;
        ProccesResourceCollect();

        _resourceStorage.ReturnResource(resource);
        _assignedResources.Remove(resource);
        _resourcePool.ReturnItem(resource);

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

        _resourceStorage.SpendResource(RequiredResourcesForBase);
        bot.SetHome(newBase);
        newBase.AddBot(bot);
        RemoveBot(bot);
        IsConstructing = false;
    }

    public void SetFlapPosition(Vector3 position) =>
        _flagSpawner.SetFlagPosition(position);

    private void ProccesResourceCollect()
    {
        if (IsConstructing)
        {
            if (CurrentFlag != null && _collectedResources % RequiredResourcesForBase == 0)
                SetBaseToConstruct();
        }
        else
        {
            if (_collectedResources % ResourcesForNewBot == 0)
                CreateNewBot();
        }
    }

    private void SetBaseToConstruct()
    {
        if (CurrentFlag == null)
            return;

        IsConstructing = false;
        List<StateMachine> idleBots = GetIdleBots();

        if (idleBots.Count > 0)
        {
            StateMachine bot = idleBots[0];
            bot.StartConstructingBase(this, CurrentFlag);
        }
    }

    private List<StateMachine> GetIdleBots() =>
        _bots.Where(bot => bot != null && bot.IsIdle).ToList();

    private void CreateNewBot()
    {
        StateMachine newBot = _botCreateor.CreateBot();

        Vector3 spawnPosition = transform.position + new Vector3(0, 0, 1);
        newBot.transform.position = spawnPosition;

        _resourceStorage.SpendResource(ResourcesForNewBot);
        newBot.SetHome(this);
        AddBot(newBot);
    }
    private void RemoveBot(StateMachine bot) =>
        _bots.Remove(bot);

    private IEnumerator SvanAndAssignResources()
    {
        while (enabled)
        {
            List<Resource> resources = _resourceScanner.ScanForResources();

            foreach (var resource in resources)
                AssignBotToResource(resource);

            yield return _scanDelay;
        }
    }
}
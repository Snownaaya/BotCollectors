using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;
using System;

[RequireComponent(typeof(ResourceHandler),typeof(ResourceScanner), typeof(FlagHandler))]
public class Base : MonoBehaviour
{
    [Inject]
    private ResourcePool _resourcePool;

    public const int RequiredResourcesForBase = 5;
    public const int ResourcesForNewBot = 3;

    [SerializeField] private BotCreator _botCreateor;
    [SerializeField] private UICounter _uiCounter;
    [SerializeField] private List<StateMachine> _bots = new List<StateMachine>();

    private HashSet<Resource> _assignedResources = new HashSet<Resource>();

    private FlagHandler _flagHandler;
    private ResourceScanner _resourceScanner;
    private WaitForSeconds _scanDelay;
    private ResourceHandler _resourceHandler;
    private ScoreView _cuurentScoreView;

    public int ResourceCount => _collectedResources;

    public event Action<int> CountChanged;
    private Flag _currentFlag => _flagHandler?.CurrentFlag;

    private int _collectedResources = 0;
    private bool IsConstructing;

    private void Awake()
    {
        _resourceHandler = GetComponent<ResourceHandler>();
        _resourceScanner = GetComponent<ResourceScanner>();
        _scanDelay = new WaitForSeconds(3f);
        _flagHandler = GetComponent<FlagHandler>();
    }

    private void Start()
    {
        StartCoroutine(ScaningResources());
        _cuurentScoreView = _uiCounter.Generate(transform.position, _collectedResources);
        _currentFlag.OnSetFlag += OnSetFlag;
    }

    private void OnDisable() =>
        _currentFlag.OnSetFlag -= OnSetFlag;

    public void Init(StateMachine initialBot, ResourcePool resource)
    {
        _bots.Clear();
        AddBot(initialBot);
        _resourcePool = resource;
    }

    public void OnSetFlag() =>
        IsConstructing = true;

    public void AssignBotToResource(Resource resource)
    {
        if (_assignedResources.Contains(resource)) return;

        if (_resourceHandler.TryAssignResource(resource))
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
        _cuurentScoreView.UpdateScore(_collectedResources);

        _resourceHandler.ReleaseResource(resource);
        _assignedResources.Remove(resource);
        _resourcePool.ReturnItem(resource);

        CountChanged?.Invoke(_collectedResources);
    }

    public void AddBot(StateMachine bot)
    {
        if (!_bots.Contains(bot))
            _bots.Add(bot);
    }

    public void CompleteConstruction(StateMachine bot)
    {
        Base newBase = bot.GetComponent<Bot>().BuildNewBase(_currentFlag);
        newBase.Init(bot, _resourcePool);

        bot.SetHome(newBase);
        newBase.AddBot(bot);
        RemoveBot(bot);
        IsConstructing = false;
    }

    private void ProccesResourceCollect()
    {
        if (IsConstructing)
        {
            if (_currentFlag != null && _collectedResources % RequiredResourcesForBase == 0)
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
        if (_currentFlag == null)
            return;

        IsConstructing = false;
        List<StateMachine> idleBots = GetIdleBots();

        if (idleBots.Count > 0)
        {
            StateMachine bot = idleBots[0];
            bot.StartConstructingBase(this, _currentFlag);
        }
    }

    private List<StateMachine> GetIdleBots() =>
        _bots.Where(bot => bot != null && bot.IsIdle).ToList();

    private void CreateNewBot()
    {
        StateMachine newBot = _botCreateor.CreateBot();
        newBot.SetHome(this);
        AddBot(newBot);
    }

    private void RemoveBot(StateMachine bot) =>
        _bots.Remove(bot);

    private IEnumerator ScaningResources()
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
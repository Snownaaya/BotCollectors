using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Base : MonoBehaviour
{
    [Inject] private ResourcePool _resourcePool;

    public const int RequiredResourcesForBase = 5;
    public const int ResourcesForNewBot = 3;

    [SerializeField] private BotCreator _botCreateor;

    [SerializeField] private List<StateMachine> _bots = new List<StateMachine>();

    private HashSet<Resource> _assignedResources = new HashSet<Resource>();
    private FlagHandler _flagHandler;
    private Flag _currentFlag => _flagHandler.CurrentFlag;

    private ResourceScanner _resourceScanner;

    private int _collectedResources = 0;

    private bool _isConstructingNewBase = false;
    private BaseStatus _currentStatus = BaseStatus.CollectingResource;

    public enum BaseStatus
    {
        CollectingResource,
        ConstructingBase,
    }

    private void Awake()
    {
        _resourceScanner = GetComponent<ResourceScanner>();
        _flagHandler = GetComponent<FlagHandler>();
    }

    private void Start()
    {
        _resourceScanner.OnResourceFound += AssignBotToResource;
        _currentFlag.OnSetFlag += OnSetFlag;
    }

    private void OnDisable()
    {
        _resourceScanner.OnResourceFound -= AssignBotToResource;
        _currentFlag.OnSetFlag -= OnSetFlag;
    }

    public void Init(StateMachine initialBot, ResourcePool resource)
    {
        _bots.Clear(); 
        AddBot(initialBot);
        _resourcePool = resource;
    }

    public void OnSetFlag() =>
        _currentStatus = BaseStatus.ConstructingBase;

    public void AssignBotToResource(Resource resource)
    {
        if (_assignedResources.Contains(resource))
            return;

        List<StateMachine> idleBots = GetIdleBots();

        if (idleBots.Count > 0)
        {
            _assignedResources.Add(resource);
            StateMachine bot = idleBots[0];
            bot.SetBotAsBusy();
            bot.StartMove(resource);
        }
    }

    public void ResourceCollect(Resource resource, StateMachine bot)
    {
        _collectedResources++;

        bot.SetBotAsFree();
        _resourcePool.ReturnItem(resource);
        _assignedResources.Remove(resource);

        ProccesResourceCollect();
    }

    public void AddBot(StateMachine bot)
    {
        if (!_bots.Contains(bot))
            _bots.Add(bot);
    }

    public void CompleteConstruction(StateMachine bot)
    {
        if (_currentFlag.IsPlaced)
        {
            Vector3 flagPosition = _currentFlag.transform.position;
            bot.GetComponent<Bot>().BuildNewBase(bot, _currentFlag, flagPosition);

            bot.SetBotAsFree();
            RemoveBot(bot);
            _currentStatus = BaseStatus.CollectingResource;
        }
    }

    private void ProccesResourceCollect()
    {
        switch (_currentStatus)
        {
            case BaseStatus.CollectingResource:
                if (_collectedResources % ResourcesForNewBot == 0)
                    CreateNewBot();
                break;

            case BaseStatus.ConstructingBase:
                if (_currentFlag != null && _collectedResources % RequiredResourcesForBase == 0)
                    SetBaseToConstruct();
                break;
        }
    }

    private void SetBaseToConstruct()
    {
        if (_currentFlag == null)
            return;

        _currentStatus = BaseStatus.ConstructingBase;
        List<StateMachine> idleBots = GetIdleBots();

        if (idleBots.Count > 0)
        {
            StateMachine bot = idleBots[0];
            bot.SetBotAsBusy();
            bot.StartConstructingBase(this, _currentFlag);
            _isConstructingNewBase = true;
        }
    }

    private List<StateMachine> GetIdleBots() =>
        _bots.FindAll(bot => bot != null && bot.CurrentState == typeof(BotIdleState));

    private void CreateNewBot()
    {
        StateMachine newBot = _botCreateor.CreateBot();
        AddBot(newBot);
    }

    private void RemoveBot(StateMachine bot) =>
        _bots.Remove(bot);
}
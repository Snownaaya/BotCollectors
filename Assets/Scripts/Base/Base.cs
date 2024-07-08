using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private List<StateMachine> _bots = new List<StateMachine>();

    private List<Resource> _resources = new List<Resource>();

    public List<StateMachine> GetIdleBots() => _bots.FindAll(bot => bot.CurrentState == typeof(BotIdleState));

    public void AssignBotToResource(Resource resource)
    {
        if (_resources.Contains(resource))
            return;

        List<StateMachine> idleBots = GetIdleBots();

        if (idleBots.Count > 0)
        {
            _resources.Add(resource);

            StateMachine bot = idleBots[0];
            bot.StartMove(resource);
            bot.SetBotAsBusy();
        }
    }

    public void ResourceCollected(Resource resource)
    {
        _resources.Remove(resource);

        foreach (StateMachine bot in _bots)
            bot.SetBotAsFree();
    }
}
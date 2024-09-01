using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class BaseCreator : MonoBehaviour
{
    [Inject] private ResourcePool _resourcePool;

    [SerializeField] private Base _base;
    [SerializeField] private ResourceScanner _resourceScanner;
    
    public Base CreateBase(Vector3 position, StateMachine tranfferingBot)
    {
        Base newBase = Instantiate(_base, position, Quaternion.identity);
        newBase.Init(tranfferingBot, _resourcePool);
        newBase.AddBot(tranfferingBot);

        tranfferingBot.Init(newBase, _resourceScanner);
        return newBase;
    }
}
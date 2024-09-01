using UnityEngine;
using System;

public interface IStateMachine
{
    Type CurrentState { get; }

    void Init(Base baseObj, ResourceScanner resourceScanner);

    bool SetBotAsBusy();
    bool SetBotAsFree();

    void ChangeState(Type type);

    void StartSearchingForResource(Resource resource);
    void StartMove(Resource resource);
    void StartCollect();
    void MoveToBase();
    void FinishWork();
    void CompleteBaseConstruction();
    void StartConstructingBase(Base @base, Flag flag);
    void SetCurrentResource(Resource resource);
}

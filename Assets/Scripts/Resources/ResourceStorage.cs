using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceStorage : MonoBehaviour
{
    private int _totalResources;

    private List<Resource> _assignedResources = new List<Resource>();

    public void SpendResource(int amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException("cost should be positive");

        _totalResources -= amount;
    }

    public void AddResource(int amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException("cost should be positive");

        _totalResources += amount;
    }

    public bool RequestResource(Resource requestedResource)
    {
        if (_assignedResources.Contains(requestedResource))
            return false; 

        _assignedResources.Add(requestedResource);
        return true;
    }

    public void ReturnResource(Resource resource)
    {
        if (_assignedResources.Contains(resource) == false)
            return;

        _assignedResources.Remove(resource);
    }
}

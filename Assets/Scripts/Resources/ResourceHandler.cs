using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceHandler : MonoBehaviour
{
    private Dictionary<Resource, bool> _resourceStatus = new Dictionary<Resource, bool>();

    public bool TryAssignResource(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource), "Resource cannot be null.");

        if (_resourceStatus.ContainsKey(resource))
        {
            if (_resourceStatus[resource] == false)
            {
                _resourceStatus[resource] = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            _resourceStatus[resource] = true;
            return true;
        }
    }

    public void ReleaseResource(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource), "Resource cannot be null.");

        if (_resourceStatus.ContainsKey(resource))
            _resourceStatus[resource] = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTriggersActivated
{

    #region Attributes

    HashSet<string> readyTriggers;

    #endregion

    #region Costructor

    public MovableTriggersActivated()
    {
        readyTriggers = new HashSet<string>();
    }

    #endregion

    #region Common

    public void AddActivatedTrigger(string name)
    {
        if (!readyTriggers.Contains(name))
        {
            readyTriggers.Add(name);
        }
    }

    public void Reset()
    {
        readyTriggers = new HashSet<string>();
    }

    public List<string> GetMovableTriggerMessages()
    {
        List<string> messages = new List<string>();
        foreach (string activatedTrigger in readyTriggers)
        {
            string message = "ActivateTrigger/" + activatedTrigger;
            messages.Add(message);
        }
        return messages;
    }

    #endregion
}


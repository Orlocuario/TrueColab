using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedColliderZones
{

    #region Attributes

    HashSet<string> activatedColliders;

    #endregion

    #region Constructor

    public ActivatedColliderZones()
    {
        activatedColliders = new HashSet<string>();
    }

    #endregion

    #region Common

    public void AddColliderDeactivator(string name, string playerId)
    {
        if (!activatedColliders.Contains(name))
        {
            string trueName = name + "/" + playerId;
            activatedColliders.Add(name + "/" + playerId);
        }
    }

    public void RemoveActivatedZone(string name, string playerId)
    {
        if (!activatedColliders.Contains(name + "/" + playerId))
        {
            activatedColliders.Remove(name + "/" + playerId);
            Debug.Log("REMOVÍ DE LA LISTA EL STRING QL");
        }
    }

    public void Reset()
    {
        activatedColliders = new HashSet<string>();
    }

    public List<string> GetActivatedColliderMessage()

    {
        List<string> messages = new List<string>();
        char[] separator = new char[1] { '/' };

        foreach (string cActivator in activatedColliders)
        {
            string[] cActivatormsg = cActivator.Split(separator);
            string cDeactivator = cActivatormsg[0];
            string playerId = cActivatormsg[1];
            string message = "ColliderDeactivatorSet/" + cDeactivator + "/" + playerId;
            messages.Add(message);
        }
        return messages;
    }

    #endregion
}

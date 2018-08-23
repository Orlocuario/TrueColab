using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedTeleporters
{
    #region Attributes

    HashSet<string> activatedTeleporters;

    #endregion

    #region Costructor

    public ActivatedTeleporters()
    {
        activatedTeleporters = new HashSet<string>();
    }

    #endregion

    #region Common

    public void AddTeleporter(string teleporterId, string playerId)
    {
        if (!activatedTeleporters.Contains(teleporterId))
        {
            activatedTeleporters.Add(teleporterId + "/" + playerId);
        }
    }

    public void Reset()
    {
        activatedTeleporters = new HashSet<string>();
    }

    public List<string> GetActivatedTeleporterMessages()
    {
        List<string> messages = new List<string>();
        char[] separator = new char[1] { '/' };

        foreach (string teleporter in activatedTeleporters)
        {
            string[] teleporterMessage = teleporter.Split(separator);
            string teleportId = teleporterMessage[0];
            string playerId = teleporterMessage[1];

            string message = "ActivateThisTeleporter" + "/" + teleportId + "/" + playerId;
            Debug.Log("My message is: " + message);
            messages.Add(message);
        }

        return messages;
    }

    #endregion

}

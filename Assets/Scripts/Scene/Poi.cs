using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poi : MonoBehaviour {

    public int id;
    public int relatedTask;
    public PlayerController[] playersNeeded;
    private int playersArrived;
    public bool poiReady;
    public bool anyPlayerIsValid;

    private void Start()
    {
        CheckParameters();
        playersArrived = 0;
        poiReady = false;
    } 
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (CheckIfIsLocalPlayer(collider.gameObject))
        {
            if (anyPlayerIsValid)
            {
                PlayerController pController = collider.GetComponent<PlayerController>();
                string pName = pController.gameObject.name;
                string messageId = id.ToString();
                SendPoiEnterToServer(messageId, pName);
                return;
            }

            if (IsPlayerNeeded(collider.gameObject))
            {
                if (poiReady)
                {
                    return;
                }

                PlayerController pController = collider.GetComponent<PlayerController>();
                string pName = pController.gameObject.name;
                string messageId = id.ToString();

                SendPoiEnterToServer(messageId, pName);

                if (relatedTask != -1)
                {
                    TaskManager tManager = GameObject.Find("Task" + relatedTask.ToString()).GetComponent<TaskManager>();
                    tManager.HandlePlayerUsedPoi(id, pController.playerId);
                }

                playersArrived++;
                if (playersArrived == playersNeeded.Length)
                {
                    LevelManager lManager = FindObjectOfType<LevelManager>();
                    if (lManager.GetLocalPlayerController().controlOverEnemies)
                    {
                        SendPoiIsReadyToServer(messageId);
                    }

                    poiReady = true;
                }

            }
        }
    }

    public void HandlePoiEnterFromServer(string poiId, GameObject incomingPlayer)
    {
        PlayerController incomingPController = incomingPlayer.GetComponent<PlayerController>();

        if (IsPlayerNeeded(incomingPlayer))
        {
            if (relatedTask != -1)
            {
                TaskManager tManager = GameObject.Find("Task" + relatedTask.ToString()).GetComponent<TaskManager>();
                tManager.HandlePlayerUsedPoi(id, incomingPController.playerId);
            }
            playersArrived++;
            if (playersArrived == playersNeeded.Length)
            {
                LevelManager lManager = FindObjectOfType<LevelManager>();
                if (lManager.GetLocalPlayerController().controlOverEnemies)
                {
                    SendPoiIsReadyToServer(poiId);
                    poiReady = true;
                }
            }
        }
    }

    public void HandlePoiReadyFromServer()
    {
        if (poiReady)
        {
            return;
        }
        else
        {
            poiReady = true;
        }
    }

    private bool CheckIfIsLocalPlayer(GameObject gObject)
    { 
        if (gObject.GetComponent<PlayerController>())
        {
            return gObject.GetComponent<PlayerController>().localPlayer;
        }
        else
        {
            return false;
        }
    }

    private bool IsPlayerNeeded(GameObject player)
    {
        PlayerController pController = player.GetComponent<PlayerController>();

        for (int i = 0; i < playersNeeded.Length; i++)
        {
            if (playersNeeded[i] != null)
            {
                if (pController.GetType().Equals(playersNeeded[i].GetType()))
                {
                    playersNeeded[i] = null;
                    return true;
                }
            }
        }
        return false;
    }

    public void SendPoiEnterToServer(string poiId, string playerWhoEntered)
    {
        Client.instance.SendMessageToServer("EnterPOI/" + poiId + "/" + playerWhoEntered, true);
    }

    public void SendPoiEnterButNobodyCaresToServer(string poiId, string playerWhoEntered)
    {
        Client.instance.SendMessageToServer("EnterButDontCare/" + poiId + "/" + playerWhoEntered, true);
    }

    public void SendPoiIsReadyToServer(string poiId)
    {
        Client.instance.SendMessageToServer("ReadyPoi/" + poiId, true);
    }

    private void CheckParameters()
    {
        if (id == 0)
        {
            Debug.LogError("A poi needs an ID");
        }

        if (relatedTask == 0)
        {
            Debug.LogError("Poi number " + id + " needs a related Task");
        }

        if (playersNeeded == null)
        {
            Debug.LogError("A poi needs players to be ready");
        }
    }
}

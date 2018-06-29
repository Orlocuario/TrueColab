using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poi : MonoBehaviour {

    public int id;
    public PlayerController[] playersNeeded;
    private int playersArrived;
    public bool poiReady;


    private void Start()
    {
        Debug.Log("number of players needed is: " + playersNeeded.Length);
        CheckParameters();
        playersArrived = 0;
        poiReady = false;
    } 
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject.name + " Just Entered this Poi: " + id);
        if (poiReady)
        {
            Debug.Log("Poi is ready so FuckOf!!");
            return;
        }

        if (CheckIfIsLocalPlayer(collider.gameObject))
        {
            if (IsPlayerNeeded(collider.gameObject))
            {
                PlayerController pController = collider.GetComponent<PlayerController>();
                string pName = pController.gameObject.name;
                string messageId = id.ToString();

                Debug.Log("from Poi " + id + ": IM GONNA TELL Y'ALL SOMEONEENTERED");

                SendPoiEnterToServer(messageId, pName);

                playersArrived++;
                if (playersArrived == playersNeeded.Length)
                {
                    LevelManager lManager = FindObjectOfType<LevelManager>();
                    if (lManager.GetLocalPlayerController().controlOverEnemies)
                    {
                        Debug.Log("from Poi " + id + ":THIS SHIT IS READY!");

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
            Debug.Log("me llegó tu mensaje pero ya estoy listo. Server ql LENTO!");
            return;
        }
        else
        {
            Debug.Log("ME VOY A PITEAR AL POI PORQUE ME DIJERON DEL SERVER. El POI: " + id);
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
                    Debug.Log("from Poi " + id + ": It WAS THE PLAYER I NEEDED");

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

        if (playersNeeded == null)
        {
            Debug.LogError("A poi needs players to be ready");
        }
    }
}

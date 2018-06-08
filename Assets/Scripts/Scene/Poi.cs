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
    } 
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (poiReady)
        {
            return;
        }

        if (CheckIfIsPlayer(collider.gameObject))
        {
            if (NotEnteredBefore(collider.gameObject))
            {
                PlayerController pController = collider.GetComponent<PlayerController>();
                pController.SendPoiEnterToServer(id);

                playersArrived++;
                if (playersArrived == playersNeeded.Length)
                {
                    LevelManager lManager = FindObjectOfType<LevelManager>();
                    if (lManager.GetLocalPlayerController().controlOverEnemies)
                    {
                        pController.SendPoiIsReadyToServer(id);
                    }
                    poiReady = true;
                }

            }
        }
    }

    private bool CheckIfIsPlayer(GameObject gObject)
    {
        return gObject.GetComponent<PlayerController>();
    }

    private bool NotEnteredBefore(GameObject player)
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

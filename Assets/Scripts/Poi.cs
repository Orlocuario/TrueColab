using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poi : MonoBehaviour {

    public int id;
    public PlayerController[] playersNeeded;
    private int playersArrived;


    private void Start()
    {
        CheckParameters();
    } 
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (CheckIfIsPlayer(collider.gameObject))
        {
            PlayerController pController = collider.GetComponent<PlayerController>();
            pController.SendPoiEnterToServer(id);

            if (NotEnteredBefore(collider.gameObject))
            {
                playersArrived++;
                if (playersArrived == playersNeeded.Length)
                {
                    pController.SendPoiIsReadyToServer(id);
                    Destroy(gameObject);
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
                if (pController.GetType().Equals(playersNeeded[i]))
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

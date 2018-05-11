using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDeactivator : MonoBehaviour {

    public GameObject[] gObjects;
    public int numberOfPlayersIn;
    PlayerController[] pControllers;
	// Use this for initialization

	void Start ()
    {
        SetCollidersActive(false);
        numberOfPlayersIn = 0;
        pControllers = new PlayerController[3];
    }
	
    public void SetCollidersActive (bool active)
    {
        foreach (GameObject gObject in gObjects)
        {
            if (gObject.GetComponent<Collider2D>())
            {
                Collider2D[] colliders = gObject.GetComponents<Collider2D>();
                foreach (Collider2D collider in colliders)
                {
                    collider.enabled = active;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            OnEnterPlayer(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            OnExitPlayer(collision.gameObject);
        }
    }

   public void OnExitPlayer(GameObject player)
    {
        PlayerController pController = player.GetComponent<PlayerController>();
        int playerId = pController.playerId;
        if (pControllers[playerId] == null)
        {
            return;
        }
        else
        {

            pControllers[playerId] = null;
            numberOfPlayersIn--;
        }

        if (numberOfPlayersIn == 0)
        {
            SetCollidersActive(false);
        }
    }

    public void OnEnterPlayer(GameObject player)
    {
        PlayerController pController = player.GetComponent<PlayerController>();
        int playerId = pController.playerId;
        if (pControllers[playerId] != null)
        {
            return;
        }
        else
        {
            pControllers[playerId] = pController;
            numberOfPlayersIn++;
        }
        if (numberOfPlayersIn >= 1)
        {
            SetCollidersActive(true);
        }
    }
}

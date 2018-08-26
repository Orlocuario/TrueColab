using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    #region Attributes

    #endregion

    #region Events

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.respawnPosition = transform.position;
        }

    }

    #endregion

    #region Utils

    protected bool GameObjectIsPlayer(GameObject other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        return playerController && playerController.localPlayer;
    }

    #endregion


}

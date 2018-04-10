using UnityEngine;
using System.Collections;

public class EndOfScene : MonoBehaviour
{
    private PlayerController[] playerControllers;
    #region Attributes

    LevelManager levelManager;

    private int playersWhoArrived;
    public int playersToArrive;

    #endregion

    #region Start

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        playersWhoArrived = 0;

        playerControllers = new PlayerController[3];

        if (playersToArrive == 0)
        {
            Debug.Log("Theres an End of Scene without an amount of players needed");
        }
    }

    #endregion

    #region Utils

    protected bool GameObjectIsPlayer(GameObject other)
    {
        return other.GetComponent<PlayerController>();
    }

    #endregion

    #region Events

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            CheckIfPlayerEntered(other.gameObject);
            Debug.Log(other.gameObject.name + " reached the end of the scene");

            playersWhoArrived++;
            if (playersWhoArrived == playersToArrive)
            {
                levelManager.GoToNextScene();
            }

            else if (other.gameObject.GetComponent<PlayerController>().localPlayer)
            {
                levelManager.ActivateNPCFeedback("Asegúrate de que lleguen todos tus amigos");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            CheckIfPlayerAlreadyLeft(other.gameObject);
            --playersWhoArrived;
        }
    }

    #endregion

    protected void CheckIfPlayerAlreadyLeft(GameObject playerObject)
    {
        PlayerController player = playerObject.GetComponent<PlayerController>();
        int i = player.playerId;
        if (playerControllers[i] != null)
        {
            playerControllers[i] = null;
            playerControllers[i].availableEndOfScene = null;
        }
        else
        {
            return;
        }
    }

    protected void CheckIfPlayerEntered(GameObject playerObject)
    {
        PlayerController player = playerObject.GetComponent<PlayerController>();
        int i = player.playerId;
        if (playerControllers[i] == null)
        {
            playerControllers[i] = player;
            player.availableEndOfScene = gameObject;
        }
        else
        {
            return;
        }
    }


    public void ErasePlayerInEndOfScene(GameObject player)
    {
        PlayerController pController = player.GetComponent<PlayerController>();
        int playerID = pController.playerId;

        if (playerControllers[playerID] != null)
        {
            playerControllers[playerID] = null;
            playersWhoArrived--;
        }
    }
}

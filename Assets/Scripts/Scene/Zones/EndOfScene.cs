using UnityEngine;
using System.Collections;

public class EndOfScene : MonoBehaviour
{

    #region Attributes

    LevelManager levelManager;

    private int playersWhoArrived;
    private int playersToArrive;

    #endregion

    #region Start

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        playersWhoArrived = 0;
        playersToArrive = 3; // This could be managed dinamically
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

            Debug.Log(other.gameObject.name + " reached the end of the scene");

            if (++playersWhoArrived == playersToArrive)
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
            --playersWhoArrived;
        }
    }

    #endregion

}

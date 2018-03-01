using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{

    #region Attributes

    private LevelManager levelManager;
    public Vector2 teleportPosition;

    protected string playerToTeleport;
    public bool teleportAnyPlayer;

    #endregion

    #region Start

    protected virtual void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        CheckTeleportPosition();
    }

    #endregion

    #region Events

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (teleportAnyPlayer)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
				if (other.gameObject.GetComponent<PlayerController> ()) 
				{
					if (other.gameObject.GetComponent<PlayerController> ().localPlayer) 
					{
						other.gameObject.GetComponent<PlayerController> ().respawnPosition = teleportPosition;
						levelManager.Respawn ();
					}
				}
            }
        }
        
		else 
        {
			if (other.gameObject.name == playerToTeleport)
			{
				if (other.gameObject.GetComponent<PlayerController> ().localPlayer) 
				{
					levelManager.localPlayer.respawnPosition = teleportPosition;
					levelManager.Respawn ();
				}
			}
        }
    }

    #endregion

    #region Utils

    protected virtual void CheckTeleportPosition()
    {
        if (teleportPosition.Equals(default(Vector2)))
        {
            Debug.Log("you need a teleport position");
        }
    }

    #endregion

}
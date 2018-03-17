using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{

    #region Attributes

    private LevelManager levelManager;
    public Vector2 teleportPosition;

    protected string playerToTeleport;
    public bool teleportAnyPlayer;
    public bool mustDoSomething;
    public bool itsDone;
    public int id; 

    #endregion

    #region Start

    protected virtual void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        CheckTeleportPositionAndId();
    }

    #endregion

    #region Events

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (teleportAnyPlayer)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                if (other.gameObject.GetComponent<PlayerController>().localPlayer)
                {
                    ActivateTeleporter(other.gameObject);
                }

            }
        }

        else
        {
            if (playerToTeleport != null)
            {
                if (other.gameObject.name == playerToTeleport)
                {
                    if (other.gameObject.GetComponent<PlayerController>().localPlayer)
                    {
                        ActivateTeleporter(other.gameObject);
                    }
                }
            }
        }
    }

    #endregion

    #region Utils

    protected void ActivateTeleporter(GameObject other)
    {
        other.GetComponent<PlayerController>().respawnPosition = teleportPosition;
        levelManager.Respawn();
        if (mustDoSomething)
        {
            DoYourTeleportedThing(id);
        }
    }

    public virtual void DoYourTeleportedThing(int id)
    {
        switch (id)
        {
            case 1:
                HandleCase1();
                break;
            default:
                return;
        }
    }

    protected virtual void CheckTeleportPositionAndId()
    {
        if (teleportPosition.Equals(default(Vector2)))
        {
            Debug.Log("you need a teleport position");
        }

        if (teleportPosition.Equals(default(int)))
        {
            Debug.LogError("teleport: " + gameObject.name + "needs an Id");
        }
    }

    private void HandleCase1()
    {
        GameObject pFilter = GameObject.Find("ChangableMageFilter1");
        if(pFilter)
        {
            PlayerFilter playerFilter = pFilter.GetComponent<PlayerFilter>();
            playerFilter.allowedPlayers[0] = levelManager.GetMage();
        }

        ParticleSystem particles = pFilter.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule module = particles.main;
        module.startColor = new Color(0, 255, 213, 255);

        levelManager.InstantiatePortal("AnyPlayerTeleporter", new Vector2(-22.21f, -2.135f), new Vector2(-21f, 0.5f));

        GameObject switchObject = GameObject.Find("Switch (2)");
        Destroy(switchObject);
        Destroy(gameObject);
    }
    #endregion

}
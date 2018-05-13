using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{

    #region Attributes

    private LevelManager levelManager;
    public Vector2 teleportPosition;

    protected string playerToTeleport;
    public bool teleportAnyPlayer;
    public bool mustDoSomething;
    private bool didMyThing;
    public int id;

    #endregion

    #region Start

    protected virtual void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        CheckTeleportPositionAndId();
        didMyThing = false;
    }

    #endregion

    #region Events

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (teleportAnyPlayer)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                ActivateTeleporter(other.gameObject);
            }
        }

        else
        {
            if (playerToTeleport != null)
            {
                if (other.gameObject.name == playerToTeleport)
                {
                    if (other.gameObject.GetComponent<PlayerController>())
                    {
                        ActivateTeleporter(other.gameObject);
                    }
                }
            }
        }
    }

    #endregion

    #region Utils

    public bool DidYourThing()
    {
        return didMyThing;
    }

    protected void ActivateTeleporter(GameObject other)
    {
        if (other.GetComponent<PlayerController>().localPlayer)
        {
            other.GetComponent<PlayerController>().respawnPosition = teleportPosition;
            levelManager.Respawn();
        }

        else
        {
            other.GetComponent<PlayerController>().respawnPosition = teleportPosition;
        }

        if (mustDoSomething)
        {
            if (didMyThing)
            {
                return;
            }
            else
            {
                didMyThing = true;
                DoYourTeleportedThing(id, other.gameObject);
            }
        }
    }

    public virtual void DoYourTeleportedThing(int id, GameObject player)
    {
        switch (id)
        {
            case 9:
                HandleCase9(player);
                break;
            case 10:
                HandleCase10(player);
                break;
            case 11:
                HandleCase11(player);
                break;
            case 12:
                HandleCase12(player);
                break;
            case 13:
                HandleCase13(player);
                break;
            case 14:
                HandleCase14(player);
                break;
            default:
                return;
        }
    }

    public virtual void DoYourTeleportedThing(int id)
    {
        switch (id)
        {
            case 1:
                HandleCase1();
                break;
            case 2:
                HandleCase2();
                break;
            case 3:
                HandleCase3();
                break;
            case 4:
                HandleCase4();
                break;
            case 5:
                HandleCase5();
                break;
            case 6:
                HandleCase6();
                break;
            case 7:
                HandleCase7();
                break;
            case 8:
                HandleCase8();
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

    private void HandleCase1() //ChangeFilters in Warrior Zone -- Scene4
    {
        GameObject pFilter = GameObject.Find("ChangableMageFilter1");
        if (pFilter)
        {
            PlayerFilter playerFilter = pFilter.GetComponent<PlayerFilter>();
            playerFilter.allowedPlayers[0] = levelManager.GetMage();
        }

        ParticleSystem particles = pFilter.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule module = particles.main;
        module.startColor = new Color(0, 255, 213, 255);

        levelManager.InstantiatePortal("AnyPlayerTeleporter", new Vector2(-22.21f, -2.135f), new Vector2(-21f, 0.5f));
        levelManager.InstatiateSprite("Arrows/mageArrowDown", new Vector2(-22f, 0.9f));

        GameObject switchObject = GameObject.Find("Switch (2)");
        Destroy(switchObject);
        Destroy(gameObject);
    }

    private void HandleCase2()
    {
        GameObject magePathBlocker = GameObject.Find("ParedMetalZonaSecretMage");
        if (magePathBlocker)
        {
            Destroy(magePathBlocker);
        }
    }

    private void HandleCase3()
    {
        GameObject wSecretBlocker = GameObject.Find("ParedMetalZonaSecretWarrior");
        if (wSecretBlocker)
        {
            Destroy(wSecretBlocker);
        }
    }

    private void HandleCase4()
    {
        GameObject eSecretBlocker = GameObject.Find("ParedMetalZonaSecretEngin");
        if (eSecretBlocker)
        {
            Destroy(eSecretBlocker);
        }
    }

    private void HandleCase5()
    {


    }

    // The ones from scene 2

    private void HandleCase6()
    {
        GameObject engineerArrow = GameObject.Find("engineerArrowUp (Clone)");
        Destroy(engineerArrow);
        levelManager.InstatiateSprite("Arrows/engineerArrowRight", new Vector2(36.83f, -5.58f));
    }

    private void HandleCase7()
    {
        GameObject enginArrow = GameObject.Find("warriorArrowDown (Clone)");
        Destroy(enginArrow);
        levelManager.InstatiateSprite("Arrows/warriorArrowRight", new Vector2(35.95f, -6.3f));
    }

    private void HandleCase8()
    {
        //Activate Zone 1 Colliders
    }

    private void HandleCase9(GameObject player)
    {
        //Activate Zone 2 Colliders
        ColliderDeactivator cDeactivatorZone2 = GameObject.Find("Zone2").GetComponent<ColliderDeactivator>();
        cDeactivatorZone2.OnEnterPlayer(player);
    }

    private void HandleCase10 (GameObject player)
    {
        //Activate Zone 3 Colliders
        ColliderDeactivator cDeactivatorZone3 = GameObject.Find("Zone3").GetComponent<ColliderDeactivator>();
        cDeactivatorZone3.OnEnterPlayer(player);
    }

    private void HandleCase11(GameObject player)
    {
        //Activate Zone 4 Colliders
        ColliderDeactivator cDeactivator4 = GameObject.Find("Zone4").GetComponent<ColliderDeactivator>();
        cDeactivator4.OnEnterPlayer(player);

    }

    private void HandleCase12(GameObject player)
    {
        //Activate Zone 5 Colliders
        ColliderDeactivator cDeactivator5 = GameObject.Find("Zone5").GetComponent<ColliderDeactivator>();
        cDeactivator5.OnEnterPlayer(player);

        ColliderDeactivator cDeactivator5Engin = GameObject.Find("Zone5Engin").GetComponent<ColliderDeactivator>();
        cDeactivator5Engin.OnEnterPlayer(player);
    }

    private void HandleCase13(GameObject player)
    {
        //Activate Zone 6 Colliders
        ColliderDeactivator cDeactivator6 = GameObject.Find("Zone6").GetComponent<ColliderDeactivator>();
        cDeactivator6.OnEnterPlayer(player);
    }

    private void HandleCase14(GameObject player)
    {
        //Activate Secret Mage Zone Colliders
        ColliderDeactivator cDeactivatorMageZone = GameObject.Find("MageWeirdZone").GetComponent<ColliderDeactivator>();
        cDeactivatorMageZone.OnEnterPlayer(player);
    }

    private void HandleCase15(GameObject player)
    {
        // Activate Secret Warrior Zone Colliders
        ColliderDeactivator cDeactivatorWarriorZone = GameObject.Find("WarriorWeirdZone").GetComponent<ColliderDeactivator>();
        cDeactivatorWarriorZone.OnEnterPlayer(player);
    }

    private void HandleCase16(GameObject player)
    {
        // Activate Secret Engineer Zone Colliders
        ColliderDeactivator cDeactivatorEnginZone = GameObject.Find("EnginWeirdZone").GetComponent<ColliderDeactivator>();
        cDeactivatorEnginZone.OnEnterPlayer(player);
    }

    private void HandleCase17(GameObject player) //Return Zone Colliders 
    {

    }

    private void HandleCase18(GameObject player)
    {

    }

    private void HandlerCase19(GameObject player)
    {

    }

    private void HandlerCase20(GameObject player)
    {

    }

    public bool CheckIfDidThing()
    {
        return didMyThing;
    }

    public void PlayerReturned()
    {
        SendMessageToServer("CoordinatePlayerTeleporter" + "/" + gameObject.name + "/" + id, true);
    }

    private void SendMessageToServer(string message, bool secure)
    {
        if (Client.instance)
        {
            Client.instance.SendMessageToServer(message, secure);
        }
    }
    #endregion

}
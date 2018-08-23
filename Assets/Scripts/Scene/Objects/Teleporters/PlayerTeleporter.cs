using System.Collections;
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
    public string groveStreet;
    public string placeToGo;
    public bool finalScene;

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
            if (mustDoSomething)
            {
                DoYourTeleportedThing(id, other.gameObject, true);
            }

            other.GetComponent<PlayerController>().respawnPosition = teleportPosition;
            levelManager.Respawn();
        }
        else
        {
            if (mustDoSomething)
            {
                DoYourTeleportedThing(id, other.gameObject, false);
            }
            other.GetComponent<PlayerController>().respawnPosition = teleportPosition;
            levelManager.Respawn(other.GetComponent<PlayerController>());
        }
    }

    public virtual void ActivateTeleporterFromServer(int playerIncoming)
    {
        PlayerController player = FindMyPlayer(playerIncoming);

        if (mustDoSomething)
        {
            DoYourTeleportedThing(id, player.gameObject, false);
        }

        levelManager.Respawn(player);
    }

    public virtual void DoYourTeleportedThing(int id, GameObject player, bool fromLocal)
    {
        switch (id)
        {
            case 1:
                HandleCase1(player, fromLocal);
                break;
            case 2:
                HandleCase2(player, fromLocal);
                break;
            case 3:
                HandleCase3(player, fromLocal);
                break;
            case 4:
                HandleCase4(player, fromLocal);
                break;
            case 5:
                HandleCase5(player, fromLocal);
                break;
            case 6:
                HandleCase6(player, fromLocal);
                break;
            case 7:
                HandleCase7(player, fromLocal);
                break;
            case 8:
                HandleCase8(player, fromLocal);
                break;
            case 9:
                HandleCase9(player, fromLocal);
                break;
            case 10:
                HandleCase10(player, fromLocal);
                break;
            case 11:
                HandleCase11(player, fromLocal);
                break;
            case 12:
                HandleCase12(player, fromLocal);
                break;
            case 13:
                HandleCase13(player, fromLocal);
                break;
            case 14:
                HandleCase14(player, fromLocal);
                break;
            case 15:
                HandleCase15(player, fromLocal);
                break;
            case 16:
                HandleCase16(player, fromLocal);
                break;
            case 17:
                HandleCase17(player, fromLocal);
                break;
            case 18:
                HandleCase18(player, fromLocal);
                break;
            case 19:
                HandleCase19(player, fromLocal);
                break;
            case 20:
                HandleCase20(player, fromLocal);
                break;
            case 21:
                HandleCase21(player, fromLocal);
                break;
            case 22:
                HandleCase22(player, fromLocal);
                break;
            case 23:
                HandleCase23(player, fromLocal);
                break;
            case 24:
                HandleCase24(player, fromLocal);
                break;
            case 25:
                HandleCase25(player, fromLocal);
                break;
            case 26:
                HandleCase26(player, fromLocal);
                break;
            case 27:
                HandleCase27(player, fromLocal);
                break;
            case 28:
                HandleCase28(player, fromLocal);
                break;
            case 29:
                HandleCase29(player, fromLocal);
                break;
            default:
                return;
        }
    }


    protected PlayerController FindMyPlayer(int playerId)
    {
        switch (playerId)
        {
            case 0:
                return levelManager.GetMage();

            case 1:
                return levelManager.GetWarrior();

            case 2:
                return levelManager.GetEngineer();

            default:
                return null;
        }
    }

    protected virtual void CheckTeleportPositionAndId()
    {
        if (teleportPosition.Equals(default(Vector2)))
        {
            Debug.Log("you need a teleport position");
        }

        if (placeToGo.Equals(default(string)))
        {
            Debug.LogError("The Teleporter named: " + gameObject.name + "doesnt have a Destiny Assigned");
        }

        else
        {
            if (finalScene)
            {
                StartCoroutine(WaitTillKnow());
            }
        }

        if (mustDoSomething)
        {
            if (id.Equals(default(int)))
            {
                Debug.LogError("teleport: " + gameObject.name + "needs an Id");
            }
        }
    }

    public IEnumerator WaitTillKnow()
    {
        yield return new WaitForSeconds(2f);
        if (FindObjectOfType<VectorTeleportAssigner>())
        {
            VectorTeleportAssigner vAssigner = FindObjectOfType<VectorTeleportAssigner>();
            teleportPosition = vAssigner.WhereAmIGoing(placeToGo);
        }
    }

    private void HandleCase1(GameObject player, bool fromLocal) //ChangeFilters in Warrior Zone -- Scene4
    {
        if (fromLocal)
        {
            int playerId = player.GetComponent<PlayerController>().playerId;
            SendMessageToServer("ActivateThisTeleporter" + "/" + id.ToString() + "/" + playerId.ToString(), true);
        }

        GameObject pFilter = GameObject.Find("ChangableMageFilter1");
        if (pFilter)
        {
            PlayerFilter playerFilter = pFilter.GetComponent<PlayerFilter>();
            playerFilter.allowedPlayers[0] = levelManager.GetMage();


            ParticleSystem particles = pFilter.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule module = particles.main;
            module.startColor = new Color(0, 255, 213, 255);
        }

        levelManager.InstatiateSprite("Arrows/mageArrowDown", new Vector2(-22f, 0.9f));
        GameObject switchObject = GameObject.Find("DestroyableSwitch");
        Destroy(switchObject);
        Destroy(gameObject);

    }

    private void HandleCase2(GameObject player, bool fromLocal)
    {
        if (fromLocal)
        {
            int playerId = player.GetComponent<PlayerController>().playerId;
            SendMessageToServer("ActivateThisTeleporter" + "/" + id.ToString() + "/" + playerId.ToString(), true);
        }

        GameObject magePathBlocker = GameObject.Find("ParedMetalZonaSecretMage");
        if (magePathBlocker)
        {
            Destroy(magePathBlocker);
        }

        ColliderDeactivator cDeactivatorZone1 = GameObject.Find("Zone1Intro").GetComponent<ColliderDeactivator>();
        cDeactivatorZone1.OnExitPlayer(player);
        Debug.LogError("Must Implement ExitPlayer Zone 1");
    }

    private void HandleCase3(GameObject player, bool fromLocal)
    {
        if (fromLocal)
        {
            int playerId = player.GetComponent<PlayerController>().playerId;
            SendMessageToServer("ActivateThisTeleporter" + "/" + id.ToString() + "/" + playerId.ToString(), true);
        }

        GameObject wSecretBlocker = GameObject.Find("ParedMetalZonaSecretWarrior");
        if (wSecretBlocker)
        {
            Destroy(wSecretBlocker);
        }

        ColliderDeactivator cDeactivatorZone1 = GameObject.Find("Zone1Intro").GetComponent<ColliderDeactivator>();
        cDeactivatorZone1.OnExitPlayer(player);
    }

    private void HandleCase4(GameObject player, bool fromLocal)
    {
        if (fromLocal)
        {
            int playerId = player.GetComponent<PlayerController>().playerId;
            SendMessageToServer("ActivateThisTeleporter" + "/" + id.ToString() + "/" + playerId.ToString(), true);
        }

        GameObject eSecretBlocker = GameObject.Find("ParedMetalZonaSecretEngin");
        if (eSecretBlocker)
        {
            Destroy(eSecretBlocker);
        }
        ColliderDeactivator cDeactivatorZone1 = GameObject.Find("Zone1Intro").GetComponent<ColliderDeactivator>();
        cDeactivatorZone1.OnExitPlayer(player);
    }

    private void HandleCase5(GameObject player, bool fromLocal)
    {

        GameObject firstDeactivator = GameObject.Find("mFirstDeactivator");
        ColliderDeactivator cDeactivator1 = firstDeactivator.GetComponent<ColliderDeactivator>();
        cDeactivator1.OnExitPlayer(player);

        GameObject secondDeactivator = GameObject.Find("mSecondDeactivator");
        ColliderDeactivator cDeactivator2 = secondDeactivator.GetComponent<ColliderDeactivator>();
        cDeactivator1.OnExitPlayer(player);

    }

    // The ones from scene 2

    private void HandleCase6(GameObject player, bool fromLocal)
    {
        if (fromLocal)
        {
            int playerId = player.GetComponent<PlayerController>().playerId;
            SendMessageToServer("ActivateThisTeleporter" + "/" + id.ToString() + "/" + playerId.ToString(), true);
        }

        GameObject engineerArrow = GameObject.Find("engineerArrowUp (Clone)");
        Destroy(engineerArrow);
        levelManager.InstatiateSprite("Arrows/engineerArrowRight", new Vector2(36.83f, -5.58f));
    }

    private void HandleCase7(GameObject player, bool fromLocal)
    {
        if (fromLocal)
        {
            int playerId = player.GetComponent<PlayerController>().playerId;
            SendMessageToServer("ActivateThisTeleporter" + "/" + id.ToString() + "/" + playerId.ToString(), true);
        }

        GameObject enginArrow = GameObject.Find("warriorArrowDown (Clone)");
        if (enginArrow)
        {
            Destroy(enginArrow);
            levelManager.InstatiateSprite("Arrows/warriorArrowRight", new Vector2(35.95f, -6.3f));
        }
    }

    // Scene 5 Deactivators 


    private void HandleCase8(GameObject player, bool fromLocal)
    {
        ColliderDeactivator cDeactivatorZone2 = GameObject.Find("Zone1Intro").GetComponent<ColliderDeactivator>();
        cDeactivatorZone2.OnEnterPlayer(player);
        //Activate Zone 1 Colliders
    }

    private void HandleCase9(GameObject player, bool fromLocal)
    {
        //Activate Zone 2 Colliders
        ColliderDeactivator cDeactivatorZone2 = GameObject.Find("Zone2").GetComponent<ColliderDeactivator>();
        cDeactivatorZone2.OnEnterPlayer(player);
    }

    private void HandleCase10(GameObject player, bool fromLocal)
    {
        //Activate Zone 3 Colliders
        ColliderDeactivator cDeactivatorZone3 = GameObject.Find("Zone3").GetComponent<ColliderDeactivator>();
        cDeactivatorZone3.OnEnterPlayer(player);
    }

    private void HandleCase11(GameObject player, bool fromLocal)
    {
        //Activate Zone 4 Colliders
        ColliderDeactivator cDeactivator4 = GameObject.Find("Zone4").GetComponent<ColliderDeactivator>();
        cDeactivator4.OnEnterPlayer(player);

    }

    private void HandleCase12(GameObject player, bool fromLocal)
    {
        //Activate Zone 5 Colliders
        ColliderDeactivator cDeactivator5 = GameObject.Find("Zone5").GetComponent<ColliderDeactivator>();
        cDeactivator5.OnEnterPlayer(player);

    }

    private void HandleCase13(GameObject player, bool fromLocal)
    {
        //Activate Zone 6 Colliders
        ColliderDeactivator cDeactivator6 = GameObject.Find("Zone6").GetComponent<ColliderDeactivator>();
        cDeactivator6.OnEnterPlayer(player);
    }

    private void HandleCase14(GameObject player, bool fromLocal)
    {
        //Activate Secret Mage Zone Colliders
        ColliderDeactivator cDeactivatorMageZone = GameObject.Find("MageWeirdZone").GetComponent<ColliderDeactivator>();
        cDeactivatorMageZone.OnEnterPlayer(player);
    }

    private void HandleCase15(GameObject player, bool fromLocal)
    {
        // Activate Secret Warrior Zone Colliders
        ColliderDeactivator cDeactivatorWarriorZone = GameObject.Find("WarriorWeirdZone").GetComponent<ColliderDeactivator>();
        cDeactivatorWarriorZone.OnEnterPlayer(player);
    }

    private void HandleCase16(GameObject player, bool fromLocal)
    {
        // Activate Secret Engineer Zone Colliders
        ColliderDeactivator cDeactivatorEnginZone = GameObject.Find("EnginWeirdZone").GetComponent<ColliderDeactivator>();
        cDeactivatorEnginZone.OnEnterPlayer(player);
    }

    private void HandleCase17(GameObject player, bool fromLocal) //Return Zone Colliders 
    {
        //ReturnFrom1
        ColliderDeactivator cDeactivatorZone1 = GameObject.Find("Zone1Intro").GetComponent<ColliderDeactivator>();
        cDeactivatorZone1.OnExitPlayer(player);
    }

    private void HandleCase18(GameObject player, bool fromLocal)
    {
        //ReturnFrom2
        ColliderDeactivator cDeactivator2 = GameObject.Find("Zone2").GetComponent<ColliderDeactivator>();
        cDeactivator2.OnExitPlayer(player);
    }

    private void HandleCase19(GameObject player, bool fromLocal)
    {
        //ReturnFrom3
        ColliderDeactivator cDeactivator3 = GameObject.Find("Zone3").GetComponent<ColliderDeactivator>();
        cDeactivator3.OnExitPlayer(player);
    }

    private void HandleCase20(GameObject player, bool fromLocal)
    {
        //Return From 4
        ColliderDeactivator cDeactivator4 = GameObject.Find("Zone4").GetComponent<ColliderDeactivator>();
        cDeactivator4.OnExitPlayer(player);
    }

    private void HandleCase21(GameObject player, bool fromLocal)
    {
        //Return From 5
        ColliderDeactivator cDeactivator5 = GameObject.Find("Zone5").GetComponent<ColliderDeactivator>();
        cDeactivator5.OnExitPlayer(player);
    }

    private void HandleCase22(GameObject player, bool fromLocal)
    {
        //Return From 6
        ColliderDeactivator cDeactivator6 = GameObject.Find("Zone6").GetComponent<ColliderDeactivator>();
        cDeactivator6.OnExitPlayer(player);
    }

    private void HandleCase23(GameObject player, bool fromLocal)
    {
        // Entering GearZone Scene4
        ColliderDeactivator[] cDeactivators23 = GameObject.Find("WarriorGearDeactivator").GetComponents<ColliderDeactivator>();
        foreach (ColliderDeactivator cDeactivator in cDeactivators23)
        {
            cDeactivator.OnEnterPlayer(player);
        }
    }

    private void HandleCase24(GameObject player, bool fromLocal)
    {
        //Left Gear Zone Scene 4
        ColliderDeactivator[] cDeactivators23 = GameObject.Find("WarriorGearDeactivator").GetComponents<ColliderDeactivator>();
        foreach (ColliderDeactivator cDeactivator in cDeactivators23)
        {
            cDeactivator.OnExitPlayer(player);
        }
    }

    private void HandleCase25(GameObject player, bool fromLocal)    //  Stage 6
    {
        ActivateDestinyColliders(player);
        DeactivateOriginColliders(player);
        SendMessageToServer("PlayerIsInZone" + "/" + placeToGo, true);
    }

    private void HandleCase26(GameObject player, bool fromLocal)
    {
        GameObject firstDeactivator = GameObject.Find("eFirstZoneActivator");
        ColliderDeactivator cDeactivator1 = firstDeactivator.GetComponent<ColliderDeactivator>();
        cDeactivator1.OnExitPlayer(player);

        GameObject secondDeactivator = GameObject.Find("eSecondZoneActivator");
        ColliderDeactivator cDeactivator2 = secondDeactivator.GetComponent<ColliderDeactivator>();
        cDeactivator1.OnExitPlayer(player);
    }

    private void HandleCase27(GameObject player, bool fromLocal)
    {
        ColliderDeactivator[] cDeactivatorZone2 = GameObject.Find("Zone2").GetComponents<ColliderDeactivator>();
        foreach (ColliderDeactivator cDeactivator in cDeactivatorZone2)
        {
            cDeactivator.OnExitPlayer(player);
        }
    }

    private void HandleCase28(GameObject player, bool fromLocal)
    {
        //Leave Zone 6, Enter zone6 Final

        ColliderDeactivator[] cDeactivatorZone6 = GameObject.Find("Zone6").GetComponents<ColliderDeactivator>();
        foreach (ColliderDeactivator cDeactivator in cDeactivatorZone6)
        {
            cDeactivator.OnExitPlayer(player);
        }

        ColliderDeactivator[] cDeactivatorZone6Final = GameObject.Find("Zone6Final").GetComponents<ColliderDeactivator>();
        foreach (ColliderDeactivator cDeactivator in cDeactivatorZone6Final)
        {
            cDeactivator.OnEnterPlayer(player);
        }

    }

    private void HandleCase29(GameObject player, bool fromLocal)
    {
        //Leave Zone 6Final, Enter zone6 Previous

        ColliderDeactivator[] cDeactivatorZone6 = GameObject.Find("Zone6").GetComponents<ColliderDeactivator>();
        foreach (ColliderDeactivator cDeactivator in cDeactivatorZone6)
        {
            cDeactivator.OnEnterPlayer(player);
        }

        ColliderDeactivator[] cDeactivatorZone6Final = GameObject.Find("Zone6Final").GetComponents<ColliderDeactivator>();
        foreach (ColliderDeactivator cDeactivator in cDeactivatorZone6Final)
        {
            cDeactivator.OnExitPlayer(player);
        }

    }


    private void ActivateDestinyColliders(GameObject player)
    {
        GameObject destination = GameObject.Find(placeToGo);
        if (destination)
        {
            if (destination.GetComponent<ColliderDeactivator>())
            {
                ColliderDeactivator cDeactivator = destination.GetComponent<ColliderDeactivator>();
                cDeactivator.OnEnterPlayer(player);
            }
        }
    }

    private void DeactivateOriginColliders(GameObject player)
    {
        if (GameObject.Find(groveStreet))
        {
            GameObject origin = GameObject.Find(groveStreet);
            if (origin.GetComponent<ColliderDeactivator>())
            {
                ColliderDeactivator cDeactivator = origin.GetComponent<ColliderDeactivator>();
                cDeactivator.OnExitPlayer(player);
            }
        }
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
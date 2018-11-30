using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientMessageHandler : MonoBehaviour
{

    #region Attributes

    private int registeredEnemies;
    private static char[] separator = new char[1] { '/' };
    Client client;

    #endregion

    #region Constructor

    public ClientMessageHandler(Client instance)
    {
        registeredEnemies = 0;
        client = instance;
    }

    #endregion

    #region Common

    public void HandleMessage(string message)
    {
        string[] msg = message.Split(separator);

        switch (msg[0])
        {
            case "ReadyPoi":
                HandlePoiReady(msg);
                break;
            case "PoiReached":
                HandlePoiReached(msg);
                break;
            case "ChangeScene":
                HandleChangeScene(msg);
                break;
            case "TakeYourCheckpoint":
                HandleMyCheckpoint(msg);
                break;
            case "ObjectMoved":
                HandleObjectMoved(msg);
                break;
            case "ObjectDestroyed":
                HandleObjectDestroyed(msg);
                break;
            case "ChangeObjectPosition":
                HandleChangeObjectPosition(msg);
                break;
            case "ActivateTrigger":
                HandleActivateTrigger(msg);
                break;
            case "InstantiateObject":
                HandleInstantiateObject(msg);
                break;
            case "NewChatMessage":
                HandleNewChatMessage(msg);
                break;
            case "StartSpendingMana":
                HandleStartSpendingMana(msg);
                break;
            case "StopSpendingMana":
                HandlePlayerStopSpendingMana(msg);
                break;
            case "ChangeRegeneration":
                HandlePlayerChangedRegenerationState(msg);
                break;
            case "UpdateInventoryFromServer":
                HandleInventoryUpdateFromServer(msg);
                break;
            case "DisplayChangeHPToClient":
                HandleChangeHpHUDToClient(msg);
                break;
            case "DisplayChangeMPToClient":
                HandleChangeMpHUDToClient(msg);
                break;
            case "DisplayChangeExpToClient":
                HandleChangeExpHUDToClient(msg);
                break;
            case "EnemyDie":
                EnemyDie(msg);
                break;
            case "EnemyRegistered":
                EnemyRegistered(msg);
                break;
            case "EnemyStartPatrolling":
                EnemyStartPatrolling(msg);
                break;
            case "EnemyChangePosition":
                ChangeEnemyPosition(msg);
                break;
            case "EnemyPatrollingPoint":
                ChangeEnemyPatrollingPoint(msg);
                break;
            case "EnemiesSetControl":
                EnemiesSetControl(msg);
                break;
            case "PlayerSetId":
                HandlePlayerSetId(msg);
                break;
            case "PlayerDisconnected":
                HandlePlayerDisconnected(msg);
                break;
            case "PlayersAreDead":
                HandlePlayersAreDead(msg);
                break;
            case "PlayerVote":
                HandlePlayerVote(msg);
                break;
            case "ActivateThisTeleporter":
                HandleTeleporterActivated(msg);
                break;
            case "ColliderDeactivatorSet":
                HandleColliderCoordination(msg);
                break;
            case "PlayerPreVote":
                HandlePlayerPreVote(msg);
                break;
            case "PlayerChangePosition":
                HandleChangePlayerPosition(msg);
                break;
            case "PlayerAttack":
                HandleUpdatedAttackState(msg);
                break;
            case "PlayerTookDamage":
                HandlePlayerTookDamage(msg);
                break;
            case "PlayerPower":
                HandleUpdatedPowerState(msg);
                break;
            case "CreateGameObject":
                HandleCreateGameObject(msg);
                break;
            case "InstantiateThisObjects":
                HandleTruePrefabInstantiation(msg);
                break;
            case "DestroyObject":
                HandleDestroyObject(msg);
                break;
            case "ChangeSwitchStatus":
                HandleChangeSwitchStatus(msg);
                break;
            case "SwitchGroupReady":
                HandleSwitchGroupReady(msg);
                break;
            case "ActivateSystem":
                HandleActivateSystem(msg);
                break;
            case "ActivateNPCLog":
                HandleActivationNpcLog(msg);
                break;
            case "IgnoreCollisionBetweenObjects":
                HandleIgnoreCollisionBetweenObjects(msg);
                break;
            case "BubbleInstantiatorData":
                HandlerBubbleInstantiatorData(msg);
                break;
            case "PlayerHasReturned":
                HandlerPlayerReturned();
                break;
            case "CoordinateRotators":
                HandleRotatorCoordination(msg);
                break;
            case "CoordinateMovingObject":
                HandleMovingObjectCoordination(msg);
                break;
            case "CoordinateObjectInCircuit":
                HandlerCircuitObjectsMovementData(msg);
                break;
            case "ExpAnswer":
                HandlerExpAnswer(msg);
                break;
            case "SceneNameAnswerForMusic":
                HandleMusicForSceneCase(msg);
                break;
            case "CoordinatePlayerId":
                HandlePlayerIdCoordination(msg);
                break;
            case "ResetDamagingObjects":
                HandleResetCollisions(msg);
                break;
            case "SetHpMpFromServer":
                HandleHpMpFromServer(msg);
                break;
            default:
                break;
        }
    }

    #endregion

    #region Handlers

    #region NPC

    private void HandleActivationNpcLog(string[] msg)
    {
        if (NotInClientScene())
        {
            LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
            levelManager.ActivateNPCFeedback(msg[1]);
        }
    }

    #endregion

    #region Enemies

    public void EnemyRegistered(string[] msg)
    {
        int instanceId = int.Parse(msg[1]);
        int enemyId = int.Parse(msg[2]);
        int directionX = Int32.Parse(msg[3]);
        float posX = float.Parse(msg[4]);
        float posY = float.Parse(msg[5]);
        bool registered = false;

        EnemyController[] enemies = GameObject.FindObjectsOfType<EnemyController>();

        //Función tablet control over enemies
        if (LocalPlayerHasControlOverEnemies())
        {
            registered = true;

            if (++registeredEnemies == enemies.Length)
            {
                Debug.Log("Start enemy patrolling");
                EnemiesStartPatrolling();
            }
        }

        //Función tablets sin control over enemies
        else
        {
            foreach (EnemyController enemy in enemies)
            {
                if (enemy)
                {

                    if (enemy.gameObject.GetInstanceID() == instanceId)
                    {
                        enemy.Initialize(enemyId, directionX, posX, posY);
                        registered = true;
                    }
                }
                else
                {
                    Debug.Log("Enemy is null mdfk");
                }
            }
        }

        if (!registered)
        {
            Debug.Log("Enemy with iID " + instanceId + " id " + enemyId + " NOT REGISTERED");
        }

    }

    public void EnemiesStartPatrolling()
    {
        string message = "EnemiesStartPatrolling/true";
        Client.instance.SendMessageToServer(message, true);
    }

    public void EnemiesRegisterOnRoom()
    {
        int enemyId = 0;

        // Agregar al enemigo local al networking
        EnemyController[] enemies = GameObject.FindObjectsOfType<EnemyController>();

        Debug.Log("Activating " + enemies.Length + " enemies");

        foreach (EnemyController enemy in enemies)
        {
            enemy.Register(enemyId++);
        }
    }

    private void HandleHpMpFromServer(string[] msg)
    {
        HpMpManager hpMp = FindObjectOfType<HpMpManager>();
        int incHp = Int32.Parse(msg[1]);
        int incMp = Int32.Parse(msg[2]);

        if (hpMp.hpCurrentAmount != incHp)
        {
            Debug.Log("Hps are different. I'll correctThem");
            int hpDif = incHp - hpMp.hpCurrentAmount;
            Debug.Log("Im changing hp with: " + hpDif + " units. FromHandleHpMpFromServer");
            hpMp.ChangeHP(hpDif);
        }
        else
        {
            Debug.Log("Hps were no different");
        }

        if (hpMp.mpCurrentAmount != incMp)
        {
            Debug.Log("Mps are different. I'll correctThem");
            int mpDif = incMp - hpMp.mpCurrentAmount;
            Debug.Log("Im changing mp with: " + mpDif + " units. FromHandleHpMpFromServer");

            hpMp.ChangeMP(mpDif);
        }
        else
        {
            Debug.Log("Mps were no different");
        }

    }

    private void HandleResetCollisions(string[] msg)
    {
        if (NotInClientScene())
        {
            int playerId = Int32.Parse(msg[1]);
            LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
            switch (playerId)
            {
                case 0:
                    MageController mController = levelManager.GetMage();
                    mController.ResetDamagingObjects();
                    break;
                case 1:
                    WarriorController wController = levelManager.GetWarrior();
                    wController.ResetDamagingObjects();
                    break;
                case 2:
                    EngineerController eController = levelManager.GetEngineer();
                    eController.ResetDamagingObjects();
                    break;
                default:
                    return;
            }
        }
    }

    private void HandlePlayerIdCoordination(string[] msg)
    {
        if (NotInClientScene())
        {
            int incomingId = Int32.Parse(msg[1]);
            switch (incomingId)
            {
                case 0:
                    {
                        LevelManager lManager = GameObject.FindObjectOfType<LevelManager>();
                        MageController mage = lManager.GetMage();
                        mage.playerId = incomingId;
                    }
                    break;

                case 1:
                    {
                        LevelManager lManager = GameObject.FindObjectOfType<LevelManager>();
                        WarriorController warrior = lManager.GetWarrior();
                        warrior.playerId = incomingId;
                    }
                    break;

                case 2:
                    {
                        LevelManager lManager = GameObject.FindObjectOfType<LevelManager>();
                        EngineerController engin = lManager.GetEngineer();
                        engin.playerId = incomingId;
                    }
                    break;

                default:
                    return;
            }
        }
    }
    private void EnemiesSetControl(string[] msg)
    {
        bool control = bool.Parse(msg[1]);

        if (NotInClientScene())
        {
            PlayerController localPlayer = client.GetLocalPlayer();

            if (!localPlayer)
            {
                Debug.Log("No local player at this point");
                return;
            }

            if (localPlayer)
            {
                Debug.Log("Now I Have a Local Player!!!!!");
            }

            localPlayer.controlOverEnemies = control;

            if (control)
            {
                Debug.Log("ControlOverEnemies Assigned");
                client.StartFirstPlan();
                localPlayer.PlayMusic();
            }

        }
    }

    private void EnemyStartPatrolling(string[] msg)
    {
        int enemyId = Int32.Parse(msg[1]);
        int directionX = Int32.Parse(msg[2]);
        float posX = float.Parse(msg[3]);
        float posY = float.Parse(msg[4]);
        float patrolX = float.Parse(msg[5]);
        float patrolY = float.Parse(msg[6]);

        EnemyController enemy = client.GetEnemy(enemyId);

        if (enemy)
        {
            enemy.SetPatrollingPoint(directionX, posX, posY, patrolX, patrolY);
            enemy.StartPatrolling();
        }
    }

    private void ChangeEnemyPosition(string[] msg)
    {
        if (NotInClientScene())
        {
            int enemyId = Int32.Parse(msg[1]);
            int directionX = Int32.Parse(msg[2]);
            float posX = float.Parse(msg[3]);
            float posY = float.Parse(msg[4]);

            EnemyController enemy = client.GetEnemy(enemyId);

            if (enemy)
            {
                enemy.SetPosition(directionX, posX, posY);
            }
        }
    }

    private void ChangeEnemyPatrollingPoint(string[] msg)
    {
        if (NotInClientScene())
        {
            int enemyId = Int32.Parse(msg[1]);
            int directionX = Int32.Parse(msg[2]);
            float posX = float.Parse(msg[3]);
            float posY = float.Parse(msg[4]);
            float patrolX = float.Parse(msg[5]);
            float patrolY = float.Parse(msg[6]);

            EnemyController enemy = client.GetEnemy(enemyId);

            if (enemy)
            {
                enemy.SetPatrollingPoint(directionX, posX, posY, patrolX, patrolY);
            }
        }
    }

    private void HandlerExpAnswer(string[] msg)
    {
        if (NotInClientScene())
        {
            GameObject expConsultant = GameObject.Find(msg[2]);

            if (expConsultant.GetComponent<NPCtrigger>())
            {
                NPCtrigger _NpcTrigger = expConsultant.GetComponent<NPCtrigger>();
                _NpcTrigger.HandleExpQuestion(msg);
            }

            else if (expConsultant.GetComponent<EndOfScene>())
            {
                EndOfScene _EndofScene = expConsultant.GetComponent<EndOfScene>();
                _EndofScene.HandleExpQuestion(msg);
            }

        }
    }

    private void EnemyDie(string[] msg)
    {
        if (NotInClientScene())
        {
            int enemyId = Int32.Parse(msg[1]);

            EnemyController enemy = client.GetEnemy(enemyId);

            if (enemy)
            {
                enemy.Die();
            }
        }
    }

    private void HandlerMovableTriggerCoordinator(string[] msg)
    {
        if (NotInClientScene())
        {
            GameObject mTrigger = GameObject.Find(msg[1]);
            if (mTrigger)
            {
                GameObject movObject = mTrigger.GetComponent<MovableTriggerInstantiator>().objectNeeded;
                mTrigger.GetComponent<MovableTriggerInstantiator>().ActivateTrigger(movObject);
            }
        }
    }

    private void HandleTeleporterActivated(string[] msg)
    {
        if (NotInClientScene())
        {
            int teleporterId = Int32.Parse(msg[1]);
            int incomingPlayerId = Int32.Parse(msg[2]);

            GameObject lManagerObj = GameObject.Find("LevelManager");
            LevelManager lManager = lManagerObj.GetComponent<LevelManager>();
            GameObject player = lManager.GetPlayer(incomingPlayerId);
            lManager.ActivateTeleporter(player, teleporterId);
        }
    }


    private void HandleMusicForSceneCase(string[] msg)
    {
        if (NotInClientScene())
        {
            LevelManager lManager = GameObject.FindObjectOfType<LevelManager>();
            PlayerController pController = lManager.GetLocalPlayerController();
            if (pController.controlOverEnemies)
            {
                pController.HandleMusicAssignment(msg[1]);
            }
        }
    }

    #endregion

    #region HUD

    private void HandleStartSpendingMana(string[] msg)
    {
        if (NotInClientScene())
        {
            HpMpManager hpMpManager = FindObjectOfType<HpMpManager>();
            if (hpMpManager)
            {
                int rateDivider = Int32.Parse(msg[1]);
                int incomingMP = Int32.Parse(msg[2]);
                hpMpManager.ReceivePlayerStartSpendingMana(rateDivider, incomingMP);
            }
        }
    }
    private void HandleInventoryUpdateFromServer(string[] msg)
    {
        if (NotInClientScene())
        {
            PickUpItem[] items = FindObjectsOfType<PickUpItem>();
            foreach (PickUpItem item in items)
            {
                if (item.gameObject.GetComponent<SpriteRenderer>().sprite.name == msg[1])
                {
                    item.PickUpFromServer();
                }
            }
        }
    }

    private void HandlePlayerChangedRegenerationState(string[] msg)
    {
        if (NotInClientScene())
        {
            HpMpManager hpMpManager = FindObjectOfType<HpMpManager>();
            if (hpMpManager)
            {
                int rateDivider = Int32.Parse(msg[1]);
                int incomingMP = Int32.Parse(msg[2]);
                int incomingHP = Int32.Parse(msg[3]);
                hpMpManager.ReceivePlayerChangedRegeneration(rateDivider, incomingMP, incomingHP);
            }
        }
    }

    private void HandleChangeHpHUDToClient(string[] msg)
    {
        if (NotInClientScene())
        {
            int damage = Int32.Parse(msg[1]);
            HpMpManager hpAndMp = FindObjectOfType<HpMpManager>();
            Debug.Log("Im changing hp with: " + damage + " units. FromHandleHpHudtoClient");
            hpAndMp.ChangeHP(damage);
        }
    }

    private void HandleChangeMpHUDToClient(string[] msg)
    {
        if (NotInClientScene())
        {
            HUDDisplay hpAndMp = GameObject.FindObjectOfType<LevelManager>().hpAndMp;
            hpAndMp.CurrentMPPercentage(float.Parse(msg[1]));
        }
    }

    private void HandleChangeExpHUDToClient(string[] msg)
    {
        if (NotInClientScene())
        {
            HUDDisplay hpAndMp = GameObject.FindObjectOfType<LevelManager>().hpAndMp;
            hpAndMp.CurrentExpValue(msg[1]);
        }
    }

    #endregion

    #region Objects 

    #region Activables

    private void HandleActivateSystem(string[] msg)
    {
        if (NotInClientScene())
        {
            LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
            levelManager.ActivateSystem(msg[1]);
        }
    }

    #endregion

    #region Switches

    private void HandleSwitchGroupReady(string[] msg)
    {
        if (NotInClientScene())
        {
            int groupId = Int32.Parse(msg[1]);

            SwitchManager manager = GameObject.FindObjectOfType<SwitchManager>();
            manager.CallAction(groupId);
        }
    }

    private void HandleChangeSwitchStatus(string[] msg)
    {
        if (NotInClientScene())
        {
            int groupId = Int32.Parse(msg[1]);
            int individualId = Int32.Parse(msg[2]);
            bool on = bool.Parse(msg[3]);

            SwitchManager manager = GameObject.FindObjectOfType<SwitchManager>();
            Switch switchi = manager.GetSwitch(groupId, individualId);
            switchi.ReceiveDataFromServer(on);
        }
    }

    #endregion

    #region GameObjects

    private void HandleChangeObjectPosition(string[] msg)
    {
        if (NotInClientScene())
        {
            LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
            levelManager.MoveItemInGame(msg[1], msg[2], msg[3], msg[4]);
        }
    }

    private void HandleActivateTrigger(string[] msg)
    {
        string triggerName = msg[1];
        if (GameObject.Find(triggerName))
        {
            MovableTriggerInstantiator mInstantiator = GameObject.Find(triggerName).GetComponent<MovableTriggerInstantiator>();
            mInstantiator.HandleTriggerReachedByMovable();
        }
        else
        {
            Debug.Log("Couldn't find the TriggerGameObject for the MovableObject");
        }
    }

    private void HandleInstantiateObject(string[] msg)  //This method is Shit;
    {
        if (NotInClientScene())
        {
            LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
            levelManager.InstantiateObjectForPlanner(msg);
        }
    }

    private void HandleTruePrefabInstantiation(string[] msg)
    {
        if (NotInClientScene())
        {
            LevelManager lManager = FindObjectOfType<LevelManager>();
            lManager.TrulyInstantiateGameObject(msg);
        }
    }

    private void HandleCreateGameObject(string[] msg)
    {
        if (NotInClientScene())
        {
            string spriteName = msg[1];
            int playerId = int.Parse(msg[2]);

            LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
            levelManager.CreateGameObject(spriteName, playerId);
        }
    }

    private void HandleDestroyObject(string[] msg)
    {
        if (NotInClientScene())
        {
            LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
            GameObject objectToDestroy = GameObject.Find(msg[1]);

            if (objectToDestroy)
            {
                levelManager.DestroyObjectInGame(objectToDestroy);
            }
        }
    }

    private void HandleIgnoreCollisionBetweenObjects(string[] msg)
    {
        LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
        levelManager.IgnoreCollisionBetweenObjects(msg);
    }

    #endregion

    #region Movables

    private void HandleObjectMoved(string[] msg)
    {
        if (NotInClientScene())
        {
            string name = msg[1];
            float forceX = float.Parse(msg[2]);
            float forceY = float.Parse(msg[3]);

            Vector2 force = new Vector2(forceX, forceY);

            GameObject movableObject = GameObject.Find(name);

            if (!movableObject)
            {
                Debug.Log("Movable " + name + " does not exists");
                return;
            }

            MovableObject movableController = movableObject.GetComponent<MovableObject>();

            if (!movableController)
            {
                Debug.Log(name + " is not movable");
                return;
            }

            movableController.MoveMe(force, false);
        }
    }

    #endregion

    #region Destroyables

    private void HandleColliderCoordination(string[] msg)
    {
        if (NotInClientScene())
        {
            GameObject cDeactivatorHolder = GameObject.Find(msg[1]);
            if (cDeactivatorHolder)
            {
                if (cDeactivatorHolder.GetComponent<ColliderDeactivator>())
                {
                    ColliderDeactivator[] cDeactivators = cDeactivatorHolder.GetComponents<ColliderDeactivator>();

                    foreach (ColliderDeactivator deactivator in cDeactivators)
                    {
                        LevelManager lManager = FindObjectOfType<LevelManager>();
                        GameObject player;
                        int playerId = Int32.Parse(msg[2]);

                        switch (playerId)
                        {
                            case 0:
                                player = lManager.GetMage().gameObject;
                                break;

                            case 1:
                                player = lManager.GetWarrior().gameObject;
                                break;

                            case 2:
                                player = lManager.GetEngineer().gameObject;
                                break;

                            default:
                                return;
                        }

                        deactivator.OnEnterPlayer(player);
                    }
                }
            }
        }
    }

    private void HandleObjectDestroyed(string[] msg)
    {
        if (NotInClientScene())
        {
            string name = msg[1];
            GameObject destroyableObject = GameObject.Find(name);

            if (!destroyableObject)
            {
                Debug.Log("Destroyable " + name + " does not exists");
                return;
            }

            else
            {
                if (destroyableObject.GetComponent<DestroyableObject>())
                {
                    DestroyableObject destroyableController = destroyableObject.GetComponent<DestroyableObject>();
                    destroyableController.DestroyMe(false);
                    return;
                }

                else if (destroyableObject.GetComponent<BurnableObject>())
                {
                    BurnableObject bObject = destroyableObject.GetComponent<BurnableObject>();
                    bObject.BurnFromServer();
                    return;
                }

                else if (destroyableObject.GetComponent<PickUpExp>())
                {
                    PickUpExp pExp = destroyableObject.GetComponent<PickUpExp>();
                    pExp.DestroyMe();
                    return;
                }

                else if (destroyableObject.GetComponent<PickUpItem>())
                {
                    PickUpItem pItem = destroyableObject.GetComponent<PickUpItem>();
                    pItem.DestroyMe();
                    return;
                }

                else if (destroyableObject.GetComponent<MovableObject>())
                {
                    MovableObject mObject = destroyableObject.GetComponent<MovableObject>();
                    mObject.DestroyMe();
                    return;
                }

                else
                {
                    Debug.Log(name + " is neither Destroyable, Exp or Item");
                }
            }
        }
    }

    #endregion

    #endregion

    #region Scene

    private void HandlePoiReached(string[] msg)
    {
        LevelManager lManager = GameObject.FindObjectOfType<LevelManager>();
        string poiId = msg[1];
        string player = msg[2];
        lManager.HandleIncomingPoiReached(poiId, player);
    }

    private void HandlePoiReady(string[] msg)
    {
        LevelManager lManager = GameObject.FindObjectOfType<LevelManager>();
        string poiId = msg[1];
        lManager.HandlePoiReady(poiId);
    }

    private void HandleMyCheckpoint(string[] msg)
    {
        float x = float.Parse(msg[1]);
        float y = float.Parse(msg[2]);
        if(FindObjectOfType<LevelManager>().GetLocalPlayerController())
        {
            PlayerController localPlayer = FindObjectOfType<LevelManager>().GetLocalPlayerController();
            localPlayer.respawnPosition = new Vector3(x, y);
        }
    }

    private void HandleChangeScene(string[] msg)
    {
        string scene = msg[1];
        /*Scene currentScene = SceneManager.GetActiveScene();

        if (GetLocalPlayerData() != null)
        {
            string message = GetLocalPlayerData();
            client.SendMessageToServer("PlayerChangePosition" + "/" + message, true);
        }
        */
        SceneManager.LoadScene(scene);
    }

    #endregion

    #region Players

    private void HandlePlayerSetId(string[] msg)
    {
        if (NotInClientScene())
        {
            int playerId = Int32.Parse(msg[1]);
            bool controlOverEnemies = bool.Parse(msg[2]);

            LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
            levelManager.SetLocalPlayer(playerId);

            PlayerController playerController = client.GetLocalPlayer();
            playerController.controlOverEnemies = controlOverEnemies;

            if (controlOverEnemies)
            {
                //client.StartFirstPlan();
                EnemiesRegisterOnRoom();
                playerController.PlayMusic();
            }
        }
    }

    // Stop the remote player
    private void HandlePlayerDisconnected(string[] msg)
    {
        if (NotInClientScene())
        {
            int playerId = Int32.Parse(msg[1]);
            PlayerController player = client.GetPlayerController(playerId);

            if (player)
            {
                if (!player.localPlayer)
                {
                    player.remoteRight = false;
                    player.remoteLeft = false;
                }
            }

            LevelManager lManager = GameObject.FindObjectOfType<LevelManager>();

        }
    }

    private void HandleChangePlayerPosition(string[] data)
    {
        if (NotInClientScene())
        {
            int playerId = Int32.Parse(data[1]);
            float positionX = float.Parse(data[2]);
            float positionY = float.Parse(data[3]);
            int directionX = Int32.Parse(data[4]);
            int directionY = Int32.Parse(data[5]);
            float speedX = float.Parse(data[6]);
            bool isGrounded = bool.Parse(data[7]);
            bool pressingJump = bool.Parse(data[8]);
            bool pressingLeft = bool.Parse(data[9]);
            bool pressingRight = bool.Parse(data[10]);

            PlayerController playerController = client.GetPlayerController(playerId);

            if (playerController)
            {
                playerController.SetPlayerDataFromServer(positionX, positionY, directionX, directionY, speedX, isGrounded, pressingJump, pressingLeft, pressingRight);
            }
        }
    }

    private void HandleNewChatMessage(string[] msg)
    {
        if (NotInClientScene())
        {
            string chatMessage = msg[1];
            Chat.instance.UpdateChat(chatMessage);
        }
    }

    private void HandleUpdatedPowerState(string[] msg)
    {
        if (NotInClientScene())
        {
            int playerId = Int32.Parse(msg[1]);
            bool powerState = bool.Parse(msg[2]);

            PlayerController playerController = client.GetById(playerId);
            playerController.SetPowerState(powerState);
        }
    }

    private void HandleUpdatedAttackState(string[] msg)
    {
        if (NotInClientScene())
        {
            int playerId = Int32.Parse(msg[1]);

            PlayerController playerController = client.GetPlayerController(playerId);

            float x = float.Parse(msg[2]);
            float y = float.Parse(msg[3]);

            Vector2 startPosition = new Vector2(x, y);
            playerController.CastLocalAttack(startPosition);
        }
    }

    private void HandlePlayerVote(string[] msg)
    {
        if (NotInClientScene())
        {
            int playerId = Int32.Parse(msg[1]);
            DecisionSystem.Choice choice = (DecisionSystem.Choice)Enum.Parse(typeof(DecisionSystem.Choice), msg[2]);

            LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
            string decisionName = levelManager.localPlayer.decisionName;
            if (decisionName != null)
            {
                DecisionSystem currentDecision = GameObject.Find(decisionName).GetComponent<DecisionSystem>();
                currentDecision.ReceiveVote(playerId, choice);
            }
        }
    }

    private void HandlePlayerPreVote(string[] msg)
    {
        if (NotInClientScene())
        {
            int playerId = Int32.Parse(msg[1]);
            int preVote = Int32.Parse(msg[2]);


            LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
            string decisionName = levelManager.localPlayer.decisionName;
            if (decisionName != null)
            {
                DecisionSystem currentDecision = GameObject.Find(decisionName).GetComponent<DecisionSystem>();
                currentDecision.ReceivePreVote(playerId, preVote);
            }
        }
    }

    private void HandlePlayerTookDamage(string[] msg)
    {
        if (NotInClientScene())
        {
            int playerId = Int32.Parse(msg[1]);
            float forceX = float.Parse(msg[2]);
            float forceY = float.Parse(msg[3]);

            Vector2 force = new Vector2(forceX, forceY);

            PlayerController playerController = client.GetPlayerController(playerId);
            playerController.SetDamageFromServer(force);
        }
    }

    private void HandlePlayerStopSpendingMana(string[] msg)
    {
        int currentDivider = Int32.Parse(msg[1]);
        int currentMana = Int32.Parse(msg[2]);
        HpMpManager hpMpManager = FindObjectOfType<HpMpManager>();
        hpMpManager.ReceivePlayerStopSpendingMana(currentDivider, currentMana);
    }

    private void HandlePlayersAreDead(string[] array)
    {
        if (NotInClientScene())
        {
            LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
            levelManager.ReloadLevel(array[1]);
        }
    }

    private void HandlerPlayerReturned()
    {
        if (NotInClientScene())
        {
            LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
            levelManager.CoordinateReconnectionElements();
        }
    }

    private void HandlerBubbleInstantiatorData(string[] msg)
    {
        if (NotInClientScene())
        {
            string bubbleInstantiatorName = msg[1];
            GameObject bubbleSystem = GameObject.Find(bubbleInstantiatorName);
            if (bubbleSystem)
            {
                BubbleRotatingInstantiator bInstantiator = bubbleSystem.GetComponent<BubbleRotatingInstantiator>();
                bInstantiator.HandleBubbleInstantiatorData(msg);
            }
        }
    }


    private void HandlerCircuitObjectsMovementData(string[] msg)
    {
        if (NotInClientScene())
        {
            int objectId = int.Parse(msg[1]);

            ObjectInCircuitMovementController[] mControllers = GameObject.FindObjectsOfType<ObjectInCircuitMovementController>();
            foreach (ObjectInCircuitMovementController movingObjects in mControllers)
            {
                if (movingObjects.GetInstanceID() == objectId)
                {
                    movingObjects.HandleMovingInCircuitObjectData(msg);
                }
            }
        }
    }
    private void HandleRotatorCoordination(string[] msg)
    {
        if (NotInClientScene())
        {
            string rotatorName = msg[1];
            GameObject rotatingSystem = GameObject.Find(rotatorName);
            if (rotatingSystem)
            {
                Rotator rotator = rotatingSystem.GetComponent<Rotator>();
                rotator.HandleRotatingInstantiatorData(msg);
            }
        }
    }

    private void HandleMovingObjectCoordination(string[] msg)
    {
        if (NotInClientScene())
        {
            GameObject mObject = GameObject.Find(msg[1]);
            if (mObject)
            {
                MovingObject mO = mObject.GetComponent<MovingObject>();
                mO.HandlePlayerReturned(msg);
            }
        }
    }
    #endregion

    #endregion

    #region Utils

    public bool NotInClientScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        return currentScene.name != "ClientScene";
    }

    private bool LocalPlayerHasControlOverEnemies()
    {
        return client.GetLocalPlayer() && client.GetLocalPlayer().controlOverEnemies;
    }

    private string GetLocalPlayerData()
    {
        PlayerController pController = client.GetLocalPlayer();
        if (pController)
        {
            int playerId = pController.playerId;
            int directionX = pController.directionX;
            int directionY = pController.directionY;
            float speedX = pController.actualSpeed;
            bool isGrounded = pController.isGrounded;
            bool pressingJump = pController.jumpPressed;
            bool pressingLeft = pController.leftPressed;
            bool pressingRight = pController.rightPressed;

            string message = playerId + "/" +
                                  directionX + "/" +
                                  directionY + "/" +
                                  speedX + "/" +
                                  isGrounded + "/" +
                                  pressingJump + "/" +
                                  pressingLeft + "/" +
                                  pressingRight;
            return message;
        }
        else
        {
            return null;
        }
    }


    #endregion
}

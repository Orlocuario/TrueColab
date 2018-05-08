﻿using System.Collections;
using System; 
using UnityEngine;

public class SwitchActions : MonoBehaviour
{

    #region Attributes

    private bool activated;
    private int groupId;

    public GameObject exp;

    public float spacing = 2f;
    public float gridX = 5f;
    public float gridY = 5f;

	private LevelManager levelManager; 

    #endregion

    #region Constructor

    public SwitchActions(GroupOfSwitchs group)
    {
		levelManager = FindObjectOfType <LevelManager> ();
        groupId = group.groupId;
        Debug.Log("Activated Group Of Switches " + groupId);
        foreach (Switch switchi in group.GetSwitchs())
        {
            switchi.SetJobDone();
        }
    }

    #endregion

    #region Common

    public void DoSomething()
    {
        if (activated)
        {
            return;
        }

        activated = true;

        switch (groupId)
        {

            // Aquí comienzan acciones switch etapa 2

            case 0:
                HandlerGroup0();
                break;

            case 1:
                HandlerGroup1();
                break;

            case 2:
                HandlerGroup2();
                break;

            case 3:
                HandlerGroup3();
                break;
            case 4:
                HandlerGroup4();
                break;

            case 5:
                HandlerGroup5();
                break;

            case 6:
                HandlerGroup6();
                break;

            case 7:
                HandlerGroup7();
                break;

            case 8:
                HandlerGroup8();
                break;

            // Aquí comienzan acciones switch etapa 1

            case 9:     // Primeros peldaños
                HandlerGroup9();
                break;

            case 10:    // peldaño switch 2
                HandlerGroup10();
                break;

            case 11:    // peldaño 3rd Switch + Exp
                HandlerGroup11();
                break;

            case 12:    // OpenPaths
                HandlerGroup12();
                break;

            case 13:    // to the end of scene
                HandlerGroup13();
                break;

            // Cases Scene 3 

            case 14:    // Mueve máquina de engranajes para Yellow
                HandlerGroup14();
                break;
            case 15:    
                HandlerGroup15();
                break;
            case 16:    
                HandlerGroup16();
                break;
            case 17:                //DamagingNeutrals from Warrior
                HandlerGroup17();
                break;
            case 18:    
                HandlerGroup18();
                break;
            case 19:  
                HandlerGroup19();
                break;
            case 20:   
                HandlerGroup20();
                break;
            case 21:
                HandlerGroup21();
                break;
            case 22:
                HandlerGroup22();
                break;
            case 23:
                HandlerGroup23();
                break;
            case 24:
                HandlerGroup24();
                break;
            case 25:
                HandlerGroup25();
                break;
            case 26:
                HandlerGroup26();
                break;
            case 27:
                HandlerGroup27();
                break;
            case 28:
                HandlerGroup28();
                break;
            case 29:
                HandlerGroup29();
                break;
            case 30:
                HandlerGroup30();
                break;
            case 31:
                HandlerGroup31();
                break;
            case 32:
                HandlerGroup32();
                break;
            case 33:
                HandlerGroup33();
                break;
            case 34:
                HandlerGroup34();
                break;
            case 35:
                HandlerGroup35();
                break;
            case 36:
                HandlerGroup36();
                break;
            case 37:
                HandlerGroup37();
                break;
            case 38:
                HandlerGroup38();
                break;
            case 39:
                HandlerGroup39();
                break;
            case 40:
                HandlerGroup40();
                break;
            case 41:
                HandlerGroup41();
                break;
            case 42:
                HandlerGroup42();
                break;
            case 43:
                HandlerGroup43();
                break;
            case 44:
                HandlerGroup44();
                break;
            case 45:
                HandlerGroup45();
                break;
            case 46:
                HandlerGroup46();
                break;
            case 47:
                HandlerGroup47();
                break;
            case 48:
                HandlerGroup48();
                break;
            case 49:
                HandlerGroup49();
                break;
            case 50:
                HandlerGroup50();
                break;
            case 51:
                HandlerGroup51();
                break;
            case 52:
                HandlerGroup52();
                break;
            case 53:
                HandlerGroup53();
                break;
            case 54:
                HandlerGroup54();
                break;
            case 55:
                HandlerGroup55();
                break;
            case 56:
                HandlerGroup56();
                break;
            case 57:
                HandlerGroup57();
                break;
            case 58:
                HandlerGroup58();
                break;
            case 59:
                HandlerGroup59();
                break;
            case 60:
                HandlerGroup60();
                break;
            case 61:
                HandlerGroup61();
                break;
            case 62:
                HandlerGroup62();
                break;
            case 63:
                HandlerGroup63();
                break;
            case 64:
                HandlerGroup64();
                break;
            case 65:
                HandlerGroup65();
                break;
            case 66:
                HandlerGroup66();
                break;
            case 67:
                HandlerGroup67();
                break;
            case 68:
                HandlerGroup68();
                break;
            case 69:
                HandlerGroup69();
                break;
            case 70:
                HandlerGroup70();
                break;
            default:
                return;
        }
    }

    #endregion

    #region Handlers

    //Switches escena 2

    private void HandlerGroup0()
    {
        GameObject platEngineer = levelManager.InstantiatePrefab("MovPlatform", new Vector2(13.63f, -1.96f));
        levelManager.SetMovingObjectData(platEngineer, new Vector2(13.63f, -1.96f), new Vector2(13.63f, 0.5f), 1f, 1.5f, false);
        levelManager.ShowFeedbackParticles("FBMageButt", new Vector2(13.63f, -1.96f), 3f);
    }

    private void HandlerGroup1()
    {
        levelManager.ShowFeedbackParticles("FBMageButt", new Vector2(-25.83f, 16.9f), 4f);

        SendMessageToServer("ObstacleDestroyed/LavaPool", true);

        levelManager.DestroyObject("CajaSwitchFierro", .1f);
		levelManager.DestroyObject("RejaEng", .1f);
		levelManager.DestroyObject("SpikesDead", .1f);
        levelManager.DestroyObject("LavaPool", .1f);


        GameObject sueloMetal = GameObject.Find("SueloMetal");
        sueloMetal.AddComponent<PlatformEffector2D>();
    }

    private void HandlerGroup2()
    {
        CameraController mainCamera = GameObject.Find("MainCamera").GetComponent<CameraController>();
        TriggerCamera tCamera = GameObject.Find("TriggerCameraForSwitchGroup2").GetComponent<TriggerCamera>();

        mainCamera.ChangeState(CameraState.TargetZoom, tCamera.movements[0]);
          
		SlideRock rocaGigante = FindObjectOfType<SlideRock>();
		rocaGigante.Slide();
		Debug.Log ("Got Rock");

        levelManager.InstatiateSprite("Arrows/engineerArrowUp", new Vector2(45.3f, -6.42f));

        BendTree bendTree = FindObjectOfType<BendTree>();
		bendTree.Fall();
		Debug.Log ("Got Tree");

        levelManager.InstatiateSprite("Arrows/engineerArrowUp", new Vector2(33.07f, -6.3f));
        levelManager.InstatiateSprite("Arrows/mageArrowDown", new Vector2(35.16f, -6.3f));
        levelManager.InstatiateSprite("Arrows/warriorArrowDown", new Vector2(35.95f, -6.3f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(34.51f, -3.88f));

        GameObject sFeedback = GameObject.Find("ActivateNPCForAllSwitches");
        Destroy(sFeedback);
    }

    private void HandlerGroup3()
    {
        GameObject platLadder = levelManager.InstantiatePrefab("MovPlatform", new Vector2(43f, -16.46f));

        Vector2 startPos = platLadder.transform.position;
        Vector2 endPos = new Vector2(startPos.x, startPos.y + 4.2f);
        levelManager.SetMovingObjectData(platLadder, startPos, endPos, 0.8f, 1f, false);

        levelManager.ShowFeedbackParticles("FBMageButt", new Vector2(41.4f, -16.3f), 3f);
		levelManager.TogglePlayerFilter("FilterMage", true);
    }

    private void HandlerGroup4()
    {
		GameObject platparaMage = levelManager.InstantiatePrefab("MovPlatform", new Vector2(61f, -9.5f));

        Vector2 startPos = platparaMage.transform.position;
        Vector2 endPos = new Vector2(startPos.x, startPos.y + 1.3f);

		levelManager.SetMovingObjectData(platparaMage, startPos, endPos, 1f, 1.5f, false);

        /* Instantiate Arrow feedback y cambiar arrow de warrior*/
        GameObject sprite = GameObject.Find("CartelCambiante");
        Destroy(sprite);

		levelManager.ShowFeedbackParticles("warriorFeedbackSmall", new Vector2(70.7f, -20f), 4f);

		levelManager.InstantiatePortal("WarriorTeleporter", new Vector2(82.64f, -18.55f), new Vector2(37.23f, -5.58f), true, 7);
    }

    private void HandlerGroup5()
    {
		levelManager.InstantiatePrefab("PlataformaPastVoladora", new Vector2(39f, 7.5f));
		levelManager.InstantiatePrefab("PlataformaPastVoladora", new Vector2(35.5f, 7.5f));

    }

    private void HandlerGroup6()
    {
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(14.1f, -6.3f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(14.3f, -6.3f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(13.6f, -6.3f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(13.1f, -6.3f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(15f, -6.3f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(15f, -6.3f));
    }

    private void HandlerGroup7()
    {
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(62f, -14.23f));

		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(62.5f, -14.23f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(64f, -14.23f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(61.5f, -14.23f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(63f, -14.23f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(63.5f, -14.23f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(62f, -14.23f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(62.5f, -14.23f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(64f, -14.23f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(61.5f, -14.23f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(63f, -14.23f));
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(63.5f, -14.23f));
    }

    private void HandlerGroup8()
    {
		levelManager.InstantiatePrefab("Ambientales/Exp", new Vector2(80.45f, -18.52f));
    }

    // Switches Escena 1

    private void HandlerGroup9()
    {
		levelManager.ShowFeedbackParticles("FBMageButt", new Vector2(26.5f, -43.6f), 4f);

		levelManager.InstantiatePrefab("SueloMetalFlotante", new Vector2(24.78f, -42.31f));
		levelManager.InstantiatePrefab("SueloMetalFlotante", new Vector2(24.78f, -43.16f));
    }

    private void HandlerGroup10()
    {
		levelManager.ShowFeedbackParticles("FBMageButt", new Vector2(26.5f, -42.11f), 4f);
		levelManager.InstantiatePrefab("SueloMetalFlotante", new Vector2(24.86f, -41.2f));
    }

    private void HandlerGroup11()
    {
		levelManager.InstantiatePrefab("SueloMetalFlotante", new Vector2(26.11f, -40.40f));
    }

    private void HandlerGroup12()
    {
		levelManager.ShowFeedbackParticles("FBMageButt", new Vector2(32.11f, -39.31f), 4f);
		levelManager.InstantiatePrefab("TutorialPaths", new Vector2(35.6f, -38.95f));
		levelManager.DestroyObject("PathBlocker", .1f);
    }

    private void HandlerGroup13()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        levelManager.localPlayer.respawnPosition = new Vector3(136.15f, -26.33f, 1f);
        levelManager.Respawn();
    }


    // Switches ESCENA 3

    private void HandlerGroup14()
    {
        Debug.Log("Getting Game Objects");
		GameObject maquinaEngranajesYellow = levelManager.FindGameObject("DesplazarEngranajes");
		GameObject plataformaBajoEngranajes = levelManager.FindGameObject("PlataformaMovilYellow");
		GameObject firstSpikesYellow = levelManager.FindGameObject("FirstSpikes");
        GameObject spikesFeedBack = levelManager.FindGameObject("ActivateNPCSpikes");

        if (spikesFeedBack)
        {
            Destroy(spikesFeedBack);
        }


        Debug.Log("Getting Movement Controllers");
        OneTimeMovingObject moverEngranajes = maquinaEngranajesYellow.GetComponent<OneTimeMovingObject>();
        OneTimeMovingObject movePlataformaBajoEngranajes = plataformaBajoEngranajes.GetComponent<OneTimeMovingObject>();
        OneTimeMovingObject spikesMover = firstSpikesYellow.GetComponent<OneTimeMovingObject>();

        Debug.Log("Setting Bool in Movement Controllers");

        moverEngranajes.move = true;
        movePlataformaBajoEngranajes.move = true;
        spikesMover.move = true;

        Debug.Log("I performed my task with " + moverEngranajes.gameObject.name);
        Debug.Log("I performed my task with " + movePlataformaBajoEngranajes.gameObject.name);
        Debug.Log("I performed my task with " + spikesMover.gameObject.name);

        GameObject maquina1Engin = GameObject.Find("MaqEngranaje1");
        GameObject maquina2Engin = GameObject.Find("MaqEngranaje2");

		levelManager.StartAnimatorBool("StartMoving", true, maquina1Engin);
		levelManager.StartAnimatorBool("StartMoving", true, maquina2Engin);

    }
    private void HandlerGroup15()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();

        if (levelManager.GetEngineer().localPlayer)
        {
            Debug.Log("it's " + levelManager.GetEngineer().localPlayer + "que tengo un EnginLocal Player");
            CameraController camera = FindObjectOfType<CameraController>();
            TriggerCamera tCamera = GameObject.Find("TriggerCameraForEnginScene3").GetComponent<TriggerCamera>();
            camera.ChangeState(CameraState.TargetZoom, tCamera.movements[0]);
        }
        levelManager.DestroyObject("ActivateNPCForEnginDestroyabe", .1f);
        levelManager.DestroyObject("CajaSwitchFierro", 2.5f);
        levelManager.InstantiatePortal("EnginTeleporter", new Vector2(8.63f, 18.09f), new Vector2(14.1f, 15.7f));
    }
    private void HandlerGroup16()
    {
		levelManager.InstantiatePortal("MageTeleporter", new Vector2(12.07f, 20.25f), new Vector2(25.1f, -1.1f));
    }
    private void HandlerGroup17()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup18()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup19()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup20()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup21()
    {
		levelManager.InstantiatePrefab("Items/RunaV1", new Vector2(59.71f, 0.42f));
		levelManager.InstantiatePrefab("Items/RunaV1", new Vector2(49.21f, -7.73f));
		levelManager.InstantiatePrefab("Items/RunaV1", new Vector2(47.71f, 19.74f));
    }
    private void HandlerGroup22()
    {
		levelManager.InstantiatePrefab("Items/RunaR1", new Vector2(-19.81f, 15.54f)); // SeemsLike No Switch in scene 3 is Using This
    }

    private void HandlerGroup23()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup24()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup25()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup26()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup27()
    {
		levelManager.InstantiatePrefab("Items/RunaA1", new Vector2(59.4f, 0.42f));
		levelManager.InstantiatePrefab("Items/RunaA1", new Vector2(48.89f, -7.73f));
		levelManager.InstantiatePrefab("Items/RunaA1", new Vector2(47.39f, 19.74f));
    }
    private void HandlerGroup28()
    {
		levelManager.InstantiatePrefab("Items/RunaR1", new Vector2(60.03f, 0.42f));
		levelManager.InstantiatePrefab("Items/RunaR1", new Vector2(49.52f, -7.73f));
		levelManager.InstantiatePrefab("Items/RunaR1", new Vector2(48.02f, 19.74f));
    }
    private void HandlerGroup29()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup30()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup31()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup32()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup33()
    {
		levelManager.InstantiatePrefab("Items/RunaA1", new Vector2(39.91f, 13.61f));
		levelManager.InstantiatePrefab("Items/RunaV1", new Vector2(52.22f, -5.04f));
		levelManager.InstantiatePrefab("Items/RunaR1", new Vector2(43.32f, -12.53f));
    }

    private void HandlerGroup34()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup35()
    {
		levelManager.DamageAllPlayers(true);
    }

    private void HandlerGroup36()
    {
		levelManager.DamageAllPlayers(true);
    }

    private void HandlerGroup37()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup38()
    {
		levelManager.DamageAllPlayers(true);
    }

    private void HandlerGroup39()
    {
		levelManager.DamageAllPlayers(true);
    }

    private void HandlerGroup40()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup41()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup42()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup43()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup44()
    {
		levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup45()
    {
        levelManager.DamageAllPlayers(true);
    }
    private void HandlerGroup46()
    {
        GameObject cajaEngin = GameObject.Find("CajaSwitchFierroEngin");
        cajaEngin.SetActive(false);
    }

    private void HandlerGroup47()
    {
        GameObject cajaEngin = GameObject.Find("CajaSwitchFierroWarrior");
        cajaEngin.SetActive(false);
    }
    private void HandlerGroup48()
    {
        GameObject cajaEngin = GameObject.Find("CajaSwitchFierroMage");
        cajaEngin.SetActive(false);
    }
    private void HandlerGroup49()
    {
		levelManager.InstantiatePortal("MageTeleporter", new Vector2(75.27f, -5.48f), new Vector2(84f, -5.5f)); //Solucionar Vectores}
		levelManager.InstantiatePortal("WarriorTeleporter", new Vector2(66.47f, -12.66f), new Vector2(83f, -5.5f)); //Solucionar Vectores}
		levelManager.InstantiatePortal("EnginTeleporter", new Vector2(63.98f, 13.31f), new Vector2(82f, -5.5f)); //Solucionar Vectores}
		levelManager.PowerableToggleLavaIntoWater ("WaterFalling", true, 4);
    }

    // Switches Scene 4

    private void HandlerGroup50()
    {
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(3f, -2f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(4f, -2f));
    }

    private void HandlerGroup51()
    {
        levelManager.InstantiatePortal("WarriorTeleporter", new Vector2(-24.416f, -1.56f), new Vector2(-20.7f, 0.5f), true, 1);
        BurnableObject treeToBurn = GameObject.Find("TreeAltarHolder").GetComponent<BurnableObject>();
        treeToBurn.Burn();

        GameObject mageFeedback = GameObject.Find("ActivateNPCForGreen2");
        if (mageFeedback)
        {
            Destroy(mageFeedback);
        }
    }

    private void HandlerGroup52()
    {
        GameObject bInstantiator = GameObject.Find("BubbleExitUp");
        if (bInstantiator)
        {
            BubbleRotatingInstantiator bIController = bInstantiator.GetComponent<BubbleRotatingInstantiator>();
            bIController.GearActivation();
        }

        GameObject kZone = GameObject.Find("KillZoneForAll");
        if (kZone)
        {
            Destroy(kZone);
        }

        GameObject kZoneFB = GameObject.Find("ActivateNPCKillingObject");
        if (kZoneFB)
        {
            Destroy(kZoneFB);
        }

        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(-32.05f, 7.68f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(-32.05f, 7.05f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(-32.80f, 7.33f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(-31.30f, 7.33f));

        // This one is for the group of Switches in the warriorGearZone;
    }

    private void HandlerGroup53()
    {
        GameObject platEngineer = levelManager.InstantiatePrefab("MovPlatform", new Vector2(-31.76f, 35.69f));
        levelManager.SetMovingObjectData(platEngineer, new Vector2(-31.76f, 35.69f), new Vector2(-39.76f, 33.45f), 1f, 2f, false);

       // MustAlsoInstantiateAlotOfEXP!

        GameObject platEngineer2 = levelManager.InstantiatePrefab("MovPlatform", new Vector2(-30.14f, 29.64f));
        levelManager.SetMovingObjectData(platEngineer2, new Vector2(-30.14f, 29.64f), new Vector2(-39.76f, 32.7f), 1f, 2f, false);
    }

        // For Scene 5

    private void HandlerGroup54() // Switch For ForLoop Destroyer 1: Check // End Of Zone1 in Scene 6
    {
        levelManager.InstantiatePortal("WarriorTeleporter", new Vector2(-30.2f, -13.54f), new Vector2(.1f, .1f), true, 3);
        SwitchForLoopDestroyer swDestroyer = GameObject.Find("SwitchBoxDestroyer1").GetComponent<SwitchForLoopDestroyer>();
        swDestroyer.SwitchReady("Rojo");
    }

    private void HandlerGroup55() // Switch For ForLoop Destroyer 1 // End Of Zone1 in Scene 6
    {
        levelManager.InstantiatePortal("EnginTeleporter", new Vector2(1.76f, -23.48f), new Vector2(.1f, .1f), true, 4);
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(0f, -23.76f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(-7.8f, -23.76f));
        SwitchForLoopDestroyer swDestroyer = GameObject.Find("SwitchBoxDestroyer1").GetComponent<SwitchForLoopDestroyer>();
        swDestroyer.SwitchReady("Amarillo");

    }

    private void HandlerGroup64() //Switch For ForLoop Destroyer 1: Check  // End Of Zone1 in Scene 6
    {
        levelManager.InstantiatePortal("MageTeleporter", new Vector2(-41.78f, 1.64f), new Vector2(-2.2f, .1f), true, 2);
        SwitchForLoopDestroyer swDestroyer = GameObject.Find("SwitchBoxDestroyer1").GetComponent<SwitchForLoopDestroyer>();
        swDestroyer.SwitchReady("Verde");       
    }
        
    private void HandlerGroup56() ////Unlocks BubbleInstantiator in Zone 1 for Mage +Exp: Check
    {
        BubbleRotatingInstantiator bInstantiator = GameObject.Find("BubbleInstantiatorMageZone").GetComponent<BubbleRotatingInstantiator>();
        bInstantiator.GearActivation();

        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(-46.19f, -4.47f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(-44.69f, -4.47f));

    }


    private void HandlerGroup57() //MovesObjects and opens path to Altar Engin and instantiatesTeleport in Zone 5: Check
    {
        levelManager.InstantiatePortal("EnginTeleporter", new Vector2(-97.34f, 0.85f), new Vector2(-56.15f, 16.4f));
        GameObject mBground = GameObject.Find("DarkMovableBackground");
        if (mBground)
        {
            mBground.GetComponent<OneTimeMovingObject>().move = true;
        }

        GameObject mPlatformThick = GameObject.Find("MovablePlatformThick");
        if (mPlatformThick)
        {
            mPlatformThick.GetComponent<OneTimeMovingObject>().move = true;
        }

        GameObject mWall = GameObject.Find("MovableWallForEngin");
        if (mWall)
        {
            mWall.GetComponent<OneTimeMovingObject>().move = true;
        }
    }

    private void HandlerGroup58() // Destroys Box For Powerable In Zone 6 : Check
    {
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(57.034f, 15.6f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(55.774f, 15.6f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(54.69f, 15.6f));

        GameObject caja = GameObject.Find("CajaSwitchFierro");
        if (caja)
        {
            Destroy(caja);
        }

    }

    private void HandlerGroup59() // Teleporter to EndOfZone6: Check
    {
        levelManager.InstantiatePortal("AnyPlayerTeleporter", new Vector2(39.87f, 37f), new Vector2(80f, 41f));
    }

    private void HandlerGroup60() // Teleporter to EndOfZone6: Check
    {
        levelManager.InstantiatePortal("AnyPlayerTeleporter", new Vector2(69.68f, 39.41f), new Vector2(80f, 41f));

    }

    private void HandlerGroup61() // Teleporter to EndOfZone6: Check
    {
        levelManager.InstantiatePortal("AnyPlayerTeleporter", new Vector2(80.62f, 31.32f), new Vector2(80f, 41f));

    }

    private void HandlerGroup62() //End of Zone 6 in Scene 5
    {
        levelManager.InstantiatePortal("AnyPlayerTeleporter", new Vector2(113f, 42.8f), new Vector2(.1f, .1f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(111.53f, 42.8f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(110.69f, 42.8f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(109.82f, 42.8f));

        ForLoopDestroyer fDestroyer = GameObject.Find("BoxesHolder").GetComponent<ForLoopDestroyer>();
        fDestroyer.DestroyOneMoreObject();
    }   

    private void HandlerGroup63()       //ThisOneIsForScene3
    {
        GameObject burnableBoxes = GameObject.Find("CajasParaMage");
        burnableBoxes.GetComponent<ObjectsToDestroy>().BurnAllThisStuff();
    }

    // Handler64 está sobre el handler56// 

    private void HandlerGroup65() //  End Of Zone 4 in Scene 5
    {
        levelManager.InstantiatePortal("AnyPlayerTeleporter", new Vector2(34.03f, 6.26f), new Vector2(-2f, .1f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(33.73f, 6.26f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(33.43f, 6.26f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(33.13f, 6.26f));
            
        ForLoopDestroyer fDestroyer = GameObject.Find("BoxesHolder").GetComponent<ForLoopDestroyer>();
        fDestroyer.DestroyOneMoreObject();
    }

    private void HandlerGroup66() // Switches para zona 3 Mage
    {
        levelManager.InstantiatePortal("AnyPlayerTeleporter", new Vector2(-11.62f, 44.3f), new Vector2(-12f, 34.5f));

        SwitchForLoopDestroyer swDestroyer = GameObject.Find("SwitchBoxDestroyer3").GetComponent<SwitchForLoopDestroyer>();
        swDestroyer.SwitchReady("Verde");

        SwitchForLoopDestroyer swDestroyer2 = GameObject.Find("SwitchBoxDestroyerZone3").GetComponent<SwitchForLoopDestroyer>();
        swDestroyer2.SwitchReady("Verde");
    }

    private void HandlerGroup67()  // Switches para zona 3 Warrior
    {
        levelManager.InstantiatePortal("AnyPlayerTeleporter", new Vector2(10f, 45f), new Vector2(-12f, 34.5f));

        SwitchForLoopDestroyer swDestroyer = GameObject.Find("SwitchBoxDestroyer3").GetComponent<SwitchForLoopDestroyer>();
        swDestroyer.SwitchReady("Rojo");

        SwitchForLoopDestroyer swDestroyer2 = GameObject.Find("SwitchBoxDestroyerZone3").GetComponent<SwitchForLoopDestroyer>();
        swDestroyer2.SwitchReady("Rojo");


    }
    private void HandlerGroup68() // PAra Zona 3 Engin
    {
        levelManager.InstantiatePortal("AnyPlayerTeleporter", new Vector2(10.8f, 39.5f), new Vector2(-12f, 34.5f));
        SwitchForLoopDestroyer swDestroyer = GameObject.Find("SwitchBoxDestroyer3").GetComponent<SwitchForLoopDestroyer>();
        swDestroyer.SwitchReady("Amarillo");

        SwitchForLoopDestroyer swDestroyer2 = GameObject.Find("SwitchBoxDestroyerZone3").GetComponent<SwitchForLoopDestroyer>();
        swDestroyer2.SwitchReady("Amarillo");
    }

    private void HandlerGroup69()  // Para Zona 2
    {
        levelManager.InstantiatePortal("AnyPlayerTeleporter", new Vector2(86f, -25f), new Vector2(-2f, .1f));
        levelManager.InstantiatePortal("AnyPlayerTeleporter", new Vector2(86f, -28.5f), new Vector2(-2f, .1f));
        levelManager.InstantiatePortal("AnyPlayerTeleporter", new Vector2(83f, -19.2f), new Vector2(-2f, .1f));

        SwitchForLoopDestroyer swDestroyer = GameObject.Find("SwitchBoxDestroyer2").GetComponent<SwitchForLoopDestroyer>();
        swDestroyer.SwitchReady("Amarillo");
        swDestroyer.SwitchReady("Rojo");
        swDestroyer.SwitchReady("Verde");

    }

    private void HandlerGroup70()  //End Of Scene 5
    {
        CameraController mainCamera = GameObject.Find("MainCamera").GetComponent<CameraController>();
        TriggerCamera tCamera = GameObject.Find("TriggerCameraForSwitchGroup70").GetComponent<TriggerCamera>();
        mainCamera.ChangeState(CameraState.TargetZoom, tCamera.movements[0]);

        levelManager.InstantiatePrefab("PortalEndOfScene", new Vector2(7.74f, 7.84f));
        GameObject eOfScene = levelManager.InstantiatePrefab("EndOfScene", new Vector2(7.43f, 6.76f));
        EndOfScene theEnd = eOfScene.GetComponent<EndOfScene>();
        theEnd.playersToArrive = 3;

        GameObject oPlatform1 = levelManager.InstantiatePrefab("/Ambientales/OrnatedTreePlatform", new Vector2(10.02f, 8.38f));
        Transform oPlatTransform = oPlatform1.GetComponent<Transform>();
        oPlatTransform.localScale = new Vector3(-1, 1, 1);

        GameObject oPlatform2 = levelManager.InstantiatePrefab("/Ambientales/OrnatedTreePlatform", new Vector2(5.58f, 8.38f));

    }



    #endregion

    #region Utils


    #endregion

    #region Messaging

    private void SendMessageToServer(string message, bool secure)
    {
        if (Client.instance && Client.instance.GetLocalPlayer() && Client.instance.GetLocalPlayer().controlOverEnemies)
        {
            Client.instance.SendMessageToServer(message, secure);
        }
    }

    #endregion

}
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
        }
    }

    #endregion

    #region Handlers

    private void HandlerGroup0()
    {
        GameObject platEngineer = levelManager.InstantiatePrefab("MovPlatform", new Vector2(13.3f, -1f));
        levelManager.SetMovingObjectData(platEngineer, new Vector2(13.5f, -1.77f), new Vector2(13.5f, 0.36f), 1f, 1f, false);
        levelManager.ShowFeedbackParticles("FBMageButt", new Vector2(13.2f, -1.3f), 3f);
    }

    private void HandlerGroup1()
    {
        levelManager.ShowFeedbackParticles("FBMageButt", new Vector2(-25.83f, 16.9f), 4f);

        SendMessageToServer("ObstacleDestroyed/LavaPool", true);

        levelManager.DestroyObject("CajaSwitchFierro", .1f);
		levelManager.DestroyObject("RejaEng", .1f);
		levelManager.DestroyObject("SpikesDead", .1f);
		levelManager.DestroyObject("LavaPool", .1f);
    }

    private void HandlerGroup2()
    {
        CameraController mainCamera = GameObject.Find("MainCamera").GetComponent<CameraController>();
        mainCamera.ChangeState(CameraState.TargetZoom, 5, 34.9f, -3.06f, false, false, false, 100, 70);
          
		SlideRock rocaGigante = FindObjectOfType<SlideRock>();
		rocaGigante.Slide();
		Debug.Log ("Got Rock");

        BendTree bendTree = FindObjectOfType<BendTree>();
		bendTree.Fall();
		Debug.Log ("Got Tree");

    }

    private void HandlerGroup3()
    {
        GameObject platLadder = levelManager.InstantiatePrefab("MovPlatform", new Vector2(43f, -16.46f));
        MovingObject movingPlatform = platLadder.GetComponent<MovingObject>();

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
        ChangeSprite spriteChanger = GameObject.Find("CartelCambiante").GetComponent<ChangeSprite>();
        spriteChanger.SpriteChanger();

		levelManager.InstantiatePrefab("NPCForWarriorCave", new Vector2(71f, -19.4f));
		levelManager.InstantiatePrefab("Ambientales/InstantiateCheckPoints", new Vector2(60.94f, -19.9f));
		levelManager.InstatiateSprite("Arrows/warriorArrowLeft", new Vector2(70.7f, -20f));

		levelManager.ShowFeedbackParticles("FBMageButt", new Vector2(72.86f, -19.3f), 4f);
		levelManager.ShowFeedbackParticles("warriorFeedbackSmall", new Vector2(70.7f, -20f), 4f);

		levelManager.InstantiatePortal("WarriorTeleporter", new Vector2(82.64f, -18.55f), new Vector2(32.89f, -6.18f));
    }

    private void HandlerGroup5()
    {
		levelManager.InstantiatePrefab("PlataformaPastVoladora", new Vector2(39f, 7.5f));
		levelManager.InstantiatePrefab("PlataformaPastVoladora", new Vector2(35.5f, 7.5f));

		GameObject killzone = levelManager.InstantiatePrefab("KillZones/KillZoneEnginAir", new Vector2(34.6f, 6.15f));

        GameObject destroyer = GameObject.Find("EngKillzoneDestroyer");

        if (destroyer)
        {
            KillZoneDestroyer killZoneDestroyer = destroyer.GetComponent<KillZoneDestroyer>();

            if (killZoneDestroyer)
            {
                killZoneDestroyer.SetKillzone(GameObject.Find("Engineer"), killzone);
            }
        }
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
            camera.ChangeState(CameraState.TargetZoom, 2.7f, -41.96f, 44.1f, false, false, false, 80f, 80f);
        }

		levelManager.DestroyObject("CajaSwitchFierro", 2.5f);
		levelManager.InstantiatePortal("EnginTeleporter", new Vector2(-52.25f, -52.25f), new Vector2(-48.83f, 43.307f));
    }
    private void HandlerGroup16()
    {
		levelManager.InstantiatePortal("MageTeleporter", new Vector2(-50.43f, 47.8f), new Vector2(-37.81f, 26.37f));
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
		levelManager.InstantiatePrefab("Items/RunaV1", new Vector2(-13.85f, 19.74f));
		levelManager.InstantiatePrefab("Items/RunaV1", new Vector2(-2.072f, 27.9f));
		levelManager.InstantiatePrefab("Items/RunaV1", new Vector2(-15.365f, 47.26f));
    }
    private void HandlerGroup22()
    {
		levelManager.InstantiatePrefab("Items/RunaR1", new Vector2(-19.81f, 15.54f));
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
		levelManager.InstantiatePrefab("Items/RunaA1", new Vector2(-14.16f, 19.74f));
		levelManager.InstantiatePrefab("Items/RunaA1", new Vector2(-2.392f, 27.9f));
		levelManager.InstantiatePrefab("Items/RunaA1", new Vector2(-15.68f, 47.26f));
    }
    private void HandlerGroup28()
    {
		levelManager.InstantiatePrefab("Items/RunaR1", new Vector2(-13.54f, 19.74f));
		levelManager.InstantiatePrefab("Items/RunaR1", new Vector2(-1.767f, 27.9f));
		levelManager.InstantiatePrefab("Items/RunaR1", new Vector2(-15.054f, 47.26f));
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
		levelManager.InstantiatePrefab("Items/RunaA1", new Vector2(-23.14f, 41.59f));
		levelManager.InstantiatePrefab("Items/RunaV1", new Vector2(-10.75f, 22.567f));
		levelManager.InstantiatePrefab("Items/RunaR1", new Vector2(-19.81f, 15.54f));
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
		levelManager.InstantiatePortal("MageTeleporter", new Vector2(12.179f, 22f), new Vector2(23f, 22f)); //Solucionar Vectores}
		levelManager.InstantiatePortal("WarriorTeleporter", new Vector2(3.37f, 14.82f), new Vector2(24f, 22f)); //Solucionar Vectores}
		levelManager.InstantiatePortal("EnginTeleporter", new Vector2(0.9f, 41f), new Vector2(25f, 22f)); //Solucionar Vectores}
		levelManager.TogglePowerableAnimatorsWithTag ("waterFalling", true, "LavaCascade3Final");
    }

    // Switches Scene 4

    private void HandlerGroup50()
    {
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(3f, -2f));
        levelManager.InstantiatePrefab("Exp/ExpFeedback35", new Vector2(4f, -2f));
    }

    #endregion

    #region Utils


    #endregion

    #region Messaging

    private void AfterCameraHandler()
	{
		
	}

    private void SendMessageToServer(string message, bool secure)
    {
        if (Client.instance && Client.instance.GetLocalPlayer() && Client.instance.GetLocalPlayer().controlOverEnemies)
        {
            Client.instance.SendMessageToServer(message, secure);
        }
    }

    #endregion

}
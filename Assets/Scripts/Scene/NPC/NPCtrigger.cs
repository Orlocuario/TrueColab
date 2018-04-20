using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCtrigger : MonoBehaviour
{

    #region Attributes

    public bool teleport;
    public Vector3 teleportTarget;

    public bool musntDie;
    public float timeForReactivation;

    public bool freezesPlayer;
    public bool finishesScene;
    public bool waitBeforeSceneChange;
    public float timeBeforeSceneChange;
    public float feedbackTime;

    public bool forOnePlayerOnly;
    public PlayerController FeedBackPlayer;



    [System.Serializable]
    public struct NPCFeedback
    {
        public GameObject particles;
        public string message;
    };

    public NPCFeedback[] feedbacks;

    private NPCFeedback activeFeedback;
    private LevelManager levelManager;

    private int feedbackCount;
    private int playersArrived;
    public int playersNeeded;

    #endregion

    #region Start 

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        ToggleParticles(false);
        feedbackCount = 0;
        playersArrived = 0;

        CheckDecisionParameters();
        CheckPlayersNeededParameters();

    }

    #endregion

    #region Common

    public void ReadNextFeedback()
    {

        // Always shut the last particles
        if (activeFeedback.particles && activeFeedback.particles.activeInHierarchy)
        {
            activeFeedback.particles.GetComponent<ParticleSystem>();
            activeFeedback.particles.SetActive(false);
        }


        // Exit if every feedback was read
        if (feedbackCount >= feedbacks.Length)
        {
            EndFeedback();
            return;
        }

        activeFeedback = feedbacks[feedbackCount];


        // Activate particles
        if (activeFeedback.particles)
        {
            activeFeedback.particles.SetActive(true);
        }

        // Set feedback text
        if (activeFeedback.message != null)
        {
            levelManager.SetNPCText(activeFeedback.message);
        }

        feedbackCount += 1;
        StartCoroutine(WaitToReadNPCMessage());
    }

    #endregion

    #region Events

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            CheckIfEndsWithEndOfScene();
            if (forOnePlayerOnly)
            {
                CheckIfForOnePlayerOnly(other.gameObject);
                return;
            }

            if (levelManager.isPlayingFeedback)
            {
                return;
            }
            if (freezesPlayer)
            {
                levelManager.localPlayer.StopMoving();
                levelManager.localPlayer.SetPowerState(false);
                levelManager.localPlayer.SendPlayerPowerDataToServer();
            }

            DestroyMyCollider();
            ReadNextFeedback();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            if (finishesScene)
            {
                playersArrived--;
            }
        }
    }

    #endregion

    #region Utils

    protected void CheckPlayersNeededParameters()
    {
        if (forOnePlayerOnly)
        {
            if (FeedBackPlayer.Equals(default(PlayerController)))
            {
                Debug.LogError("The NPC Trigger named: " + name + " / Needs a pController Type to filter");
            }
        }
    }

    protected void CheckDecisionParameters()
    {
        if (finishesScene)
        {
            if (playersNeeded == 0)
            {
                Debug.LogError("the NPC system named: " + gameObject.name + "  finishes Scene but no number of players needed were set before start");
            }
        }
        if (musntDie)
        {
            if (timeForReactivation == 0)
            {
                Debug.LogError("the NPC system named: " + gameObject.name + "  needs a ReactivationTime");
            }
        }
    }

    protected void CheckIfEndsWithEndOfScene()
    {
        if (finishesScene)
        {
            playersArrived++;
            Debug.Log("playersArrived: " + playersArrived);
            if (playersArrived == playersNeeded)
            {
                DestroyMyCollider();
                ReadNextFeedback();
            }
            else
            {
                levelManager.ActivateNPCFeedback("¿Estás Solo? Así no podrás salir jamás...");
                return;
            }
        }
    }

    protected void CheckIfForOnePlayerOnly(GameObject player)
    {
        if (player.GetComponent<PlayerController>().GetType().Equals(FeedBackPlayer.GetType()))
        {
            DestroyMyCollider();
            ReadNextFeedback();
            return;
        }
        else
        {
            return;
        }
    }

    protected void DestroyMyCollider()
    {
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        if (collider.isTrigger)
        {
            collider.enabled = false;
        }
    }

    protected void UndestroyMyCollider()
    {
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        if (collider.isTrigger)
        {
            collider.enabled = true;
        }
    }


    protected void ToggleParticles(bool active)
    {
        foreach (NPCFeedback feedback in feedbacks)
        {
            if (feedback.particles)
            {
                feedback.particles.SetActive(active);
            }
        }

    }

    protected void EndFeedback()
    {
        levelManager.isPlayingFeedback = false;
        if (freezesPlayer)
        {
            levelManager.localPlayer.ResumeMoving();
            freezesPlayer = false;
        }

        if (teleport)
        {
            levelManager.localPlayer.respawnPosition = teleportTarget;
            levelManager.Respawn();
        }

        if (finishesScene)
        {
            if (waitBeforeSceneChange)
            {
                StartCoroutine(WaitBeforeChangeScene());
            }

            levelManager.GoToNextScene();
        }

        levelManager.ShutNPCFeedback(true);
        if (musntDie)
        {
            StartCoroutine(WaitToBeActiveAgain());
            return;
        }

        Destroy(gameObject);
    }

    protected bool GameObjectIsPlayer(GameObject other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        return playerController && playerController.localPlayer;
    }

    #endregion

    #region Coroutines

    private IEnumerator WaitToReadNPCMessage()
    {
        yield return new WaitForSeconds(feedbackTime);
        ReadNextFeedback();
    }

    private IEnumerator WaitToBeActiveAgain()
    {
        yield return new WaitForSeconds (timeForReactivation);
        UndestroyMyCollider();
        feedbackCount = 0;
    }

    private IEnumerator WaitBeforeChangeScene()
    {
        yield return new WaitForSeconds(timeBeforeSceneChange);
        levelManager.GoToNextScene();
    }

    #endregion

}

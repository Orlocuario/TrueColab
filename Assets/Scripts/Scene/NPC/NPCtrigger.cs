using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCtrigger : MonoBehaviour
{

    #region Attributes

    public bool teleport;
    public Vector3 whereToRespawn;

    public bool freezesPlayer;
    public bool hasDecision;
    public float feedbackTime;


    [System.Serializable]
    public struct NPCFeedback
    {
        public GameObject particles;
        public string message;
    };

    public NPCFeedback[] feedbacks;

    private NPCFeedback activeFeedback;
    private LevelManager levelManager;
    public DecisionSystem dSystem;

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
        if (hasDecision)
        {
            if (playersNeeded == 0)
            {
                Debug.LogError("this NPC has a decision system after the trigger but no number of players needed before starting");
            }
            if (!dSystem)
            {
                Debug.LogError("this NPC has needs a decision system to control");
            }
        }
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
            if (hasDecision)
            {
                playersArrived++;
                if (playersArrived == playersNeeded)
                {
                    ReadNextFeedback();
                }
                else
                {
                    levelManager.ActivateNPCFeedback("¿Estás Solo? Así no podrás salir jamás...");
                    return;
                }
            }

            if (freezesPlayer)
            {
                levelManager.localPlayer.StopMoving();
            }
            ReadNextFeedback();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            if (hasDecision)
            {
                playersArrived--;
            }
        }
    }

    #endregion

    #region Utils

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

        if (freezesPlayer)
        {
            levelManager.localPlayer.ResumeMoving();
        }

        if (teleport)
        {
            levelManager.localPlayer.respawnPosition = whereToRespawn;
            levelManager.Respawn();
        }

        if (hasDecision)
        {
            dSystem.StartThisVoting();
            
        }

        levelManager.ShutNPCFeedback(true);
        Destroy(this.gameObject);
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

    #endregion

}

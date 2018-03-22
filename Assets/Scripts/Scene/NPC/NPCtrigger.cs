﻿using System.Collections;
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

    public bool forOnePlayerOnly;
    public int requestedID; 


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
            CheckIfEndsWithDecision();
            PlayerFeedBackId(other.gameObject);

            if (freezesPlayer)
            {
                levelManager.localPlayer.StopMoving();
            }

            DestroyMyCollider();
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

    protected void CheckPlayersNeededParameters()
    {
        if (forOnePlayerOnly)
        {
            if (requestedID.Equals(default(int)))
            {
                Debug.LogError("The NPC Trigger named: " + name + " / Needs an entrance ID");
            }
        }
    }

    protected void CheckDecisionParameters()
    {
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

    protected void CheckIfEndsWithDecision()
    {
        if (hasDecision)
        {
            playersArrived++;
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

    protected void PlayerFeedBackId(GameObject player)
    {
        if (forOnePlayerOnly)
        {
            int incomingId = player.GetComponent<PlayerController>().playerId;
            if (incomingId == requestedID)
            {
                DestroyMyCollider();
                ReadNextFeedback();
            }
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

    #endregion

}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TriggerCamera : MonoBehaviour
{

    #region Attributes
    [Serializable]
    public struct CameraMovementData
    {
        public CameraState state;
        public GameObject target;
        public float ortographic_size;
        public bool hideChat;
        public bool playerCantMove;
        public bool hideCanvas;
        public float stepsToTarget;
        public float freezeTime;
        public float timeWaiting;
        public bool playerChangeState;

    }

    public CameraMovementData[] movements;
    public bool isCutScene;

    protected PlayerController[] playerControllers;

    #endregion

    #region Start

    private void Start()
    {
        playerControllers = new PlayerController[3];
    }

    #endregion

    #region Common 

    public void OnEnter()
    {
        GameObject camera = GameObject.Find("MainCamera");
        CameraController cameraController = camera.GetComponent<CameraController>();
        if (isCutScene)
        {
            cameraController.StartCoroutine(cameraController.StartCutscene(movements));
            return;
        }

        else if (movements[0].playerChangeState)
        {
            cameraController.ChangeState(movements[0].state);
        }

        else
        {
            cameraController.ChangeState(movements[0].state, movements[0]);
        }
    }

    private void OnExit()
    {

        GameObject camera = GameObject.Find("MainCamera");
        CameraController cameraController = camera.GetComponent<CameraController>();

        cameraController.SetDefaultValues();

    }

    #endregion

    #region Events

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            CheckIfPlayerEntered(other.gameObject);
            OnEnter();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            OnExit();
        }
    }

    #endregion

    #region Utils

    protected bool GameObjectIsPlayer(GameObject other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        return playerController && playerController.localPlayer;
    }

    protected void CheckIfPlayerEntered(GameObject playerObject)
    {
        PlayerController player = playerObject.GetComponent<PlayerController>();
        int i = player.playerId;
        if (playerControllers[i] == null)
        {
            playerControllers[i] = player;
        }
        else
        {
            return;
        }
    }

}
    #endregion

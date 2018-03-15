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

        public void NewMovement()
        {
            GameObject camera = GameObject.Find("MainCamera");
            CameraController cameraController = camera.GetComponent<CameraController>();
            cameraController.ChangeState(state, ortographic_size, target.transform.position.x, target.transform.position.y, hideChat, playerCantMove, hideCanvas, stepsToTarget, freezeTime);
        }
    }

    public CameraMovementData[] movements;
    public bool isCutScene;

    #endregion

    #region Common 

    public IEnumerator OnEnter()
    {
        for (int i = 0; i < movements.Length; i++)
        {
            movements[i].NewMovement();


            if (movements[i].state == CameraState.TargetZoomInCutscene)
            {
                yield return new WaitForSeconds(WaitForCamera(movements[i].stepsToTarget, movements[i].freezeTime));
                Debug.Log(WaitForCamera(movements[i].stepsToTarget, movements[i].freezeTime) * Time.deltaTime);
            }
            else if (movements[i].state == CameraState.TargetZoom)
            {
                yield return new WaitForSeconds(WaitForCamera(movements[i].stepsToTarget, movements[i].freezeTime));
                Debug.Log(WaitForCamera(movements[i].stepsToTarget*2, movements[i].freezeTime) * Time.deltaTime);
            }
            else
            {
                yield return new WaitForSeconds(movements[i].timeWaiting);
                Debug.Log("ImWaiting");
            }
        }

        if (isCutScene)
        {
            Debug.Log("Its a Cutscene, so left");
            OnExit();
            Destroy(gameObject, 1f);
        }
        Debug.Log("Im Not a Cutscene So i just left without restartint Left");
    }

    private void OnExit()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        PlayerController playerController = levelManager.GetLocalPlayerController();
        playerController.ResumeMoving();

        GameObject camera = GameObject.Find("MainCamera"); // TODO: Change this to obj name
        CameraController cameraController = camera.GetComponent<CameraController>();

        cameraController.SetDefaultValues();

    }

    #endregion

    #region Events

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            StartCoroutine(OnEnter());
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

    private float WaitForCamera(float stepsToTarget, float freezeTime)
    {
        return (stepsToTarget + freezeTime) / 30;
    }


    #endregion
}

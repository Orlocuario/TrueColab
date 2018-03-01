﻿using UnityEngine;

public enum CameraState
{
    Normal,
    Backwards,
    FixedX,
    FixedY,
    Zoomed,
    TargetZoom,
    TargetZoomInCutscene,
    NoFollowUp,
    NoFollowAhead,
};

public class CameraController : MonoBehaviour
{

    #region Attributes

    public CameraState currentState;

    public float smoothCamera;
    public float followAhead;
    public float startTime;
    public float followUp;
    public int cameraMovementsAsked;
    public int cameraMovementsDone;


    private LevelManager levelManager;
    private Vector3 currentStepPos;
    private Vector3 targetPosition;
    private GameObject target;
    private Camera thisCamera;

    public float stepsToTarget = 100;
    public float initialSize = 2.8f;
    public float globalFreezeTime = 70;

    private float cameraRate;
    private int zoomSteps;

    #endregion

    #region Start & Update

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        thisCamera = GetComponent<Camera>();

        ChangeState(CameraState.Normal, 10, 0, 0, false, false, false, 100f, 70f);
    }

    void Update()
    {
        if (!target)
        {
            return;
        }

        targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);

        switch (currentState)
        {

            case CameraState.Normal:
                MoveNormal();
                break;

            case CameraState.Backwards:
                MoveBackwards();
                break;

            case CameraState.Zoomed:    // Por qué aquí no hay nada???
                break;

            case CameraState.FixedX:
                MoveFixedX();
                break;

            case CameraState.FixedY:
                MoveFixedY();
                break;

            case CameraState.TargetZoom:
                MoveToZoom();
                break;

            case CameraState.TargetZoomInCutscene:
                MoveToZoomInCutScene();
                break;

            case CameraState.NoFollowUp:
                MoveNoFollowUp();
                break;

            case CameraState.NoFollowAhead:
                MoveNoFollowAhead();
                break;

        }
    }

    #endregion

    #region Common

    private void MoveNormal()
    {
        if (target.transform.localScale.x > 0f)
        {
            targetPosition = new Vector3(targetPosition.x + followAhead, targetPosition.y + followUp, targetPosition.z);
        }
        else
        {
            targetPosition = new Vector3(targetPosition.x - followAhead, targetPosition.y + followUp, targetPosition.z);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothCamera * Time.deltaTime);

    }

    private void MoveFixedX()
    {
        transform.position = new Vector3(transform.position.x, targetPosition.y, targetPosition.z);
    }

    private void MoveFixedY()
    {
        transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
    }

    private void MoveToZoom()
    {
        if (zoomSteps < stepsToTarget)
        {
            transform.position = new Vector3(transform.position.x + currentStepPos.x, transform.position.y + currentStepPos.y, transform.position.z);
            thisCamera.orthographicSize = thisCamera.orthographicSize + cameraRate;
            zoomSteps++;
        }
        else if (zoomSteps < stepsToTarget + globalFreezeTime)
        {
            zoomSteps++;
        }
        else if (zoomSteps < stepsToTarget + globalFreezeTime + stepsToTarget)
        {
            transform.position = new Vector3(transform.position.x - currentStepPos.x, transform.position.y - currentStepPos.y, transform.position.z);
            thisCamera.orthographicSize = thisCamera.orthographicSize - cameraRate;
            zoomSteps++;
        }
        else
        {
            levelManager.localPlayer.ResumeMoving();
            SetDefaultValues();
        }
    }
    private void MoveToZoomInCutScene()
    {
        if (zoomSteps < stepsToTarget)
        {
            transform.position = new Vector3(transform.position.x + currentStepPos.x, transform.position.y + currentStepPos.y, transform.position.z);
            thisCamera.orthographicSize = thisCamera.orthographicSize + cameraRate;
            zoomSteps++;
        }
        else if (zoomSteps < stepsToTarget + globalFreezeTime)
        {
            zoomSteps++;
        }
    }
    private void MoveNoFollowUp()
    {
        if (target.transform.localScale.x > 0f)
        {
            targetPosition = new Vector3(targetPosition.x + followAhead, targetPosition.y, targetPosition.z);
        }
        else
        {
            targetPosition = new Vector3(targetPosition.x - followAhead, targetPosition.y, targetPosition.z);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothCamera * Time.deltaTime);
    }

    private void MoveNoFollowAhead()
    {
        if (target.transform.localScale.y > 0f)
        {
            targetPosition = new Vector3(targetPosition.x, targetPosition.y + followUp, targetPosition.z);
        }
        else
        {
            targetPosition = new Vector3(targetPosition.x, targetPosition.y - followUp, targetPosition.z);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothCamera * Time.deltaTime);
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void ChangeState(CameraState state, float ortographicsize, float x, float y, bool sinChat,
                            bool playerCantMove, bool sinCanvas, float stepsToTarget, float freezeTime)
    {
        switch (state)
        {
            case CameraState.Normal:
                SetDefaultValues();
                break;
            case CameraState.Zoomed:
                SetZoomedValues(ortographicsize, x, y, sinChat, sinCanvas);
                break;
            case CameraState.FixedX:
                SetFixedX();
                break;
            case CameraState.FixedY:
                SetFixedY();
                break;
            case CameraState.TargetZoom:
                TargetedZoom(ortographicsize, x, y, playerCantMove, sinCanvas, stepsToTarget, freezeTime);
                break;
            case CameraState.NoFollowAhead:
                SetNoFollowAhead();
                break;
            case CameraState.NoFollowUp:
                SetNofollowUp();
                break;
        }
    }

    public void SetZoomedValues(float size, float x, float y, bool hideChat, bool sinCanvas)
    {
        currentState = CameraState.Zoomed;
        thisCamera.orthographicSize = size;
        transform.position = new Vector3(x, y, transform.position.z);

        ToggleChat(hideChat);
        ToggleCanvas(sinCanvas);
    }

    private void TargetedZoom(float size, float x, float y, bool playerCantMove, bool sinCanvas, float stepsToTarget, float freezeTime)
    {
        if (playerCantMove == true)
        {
            levelManager.localPlayer.StopMoving();
        }
        if (sinCanvas)
        {
            ToggleCanvas(false);
        }
        globalFreezeTime = freezeTime;
        Vector3 targetPosition = new Vector3(x, y, 0);
        currentState = CameraState.TargetZoom;

        currentStepPos = (targetPosition - transform.position) / stepsToTarget;
        cameraRate = (size - initialSize) / stepsToTarget;
        zoomSteps = 0;
    }

    #endregion

    #region Utils

    private void MoveBackwards()
    {
        currentState = CameraState.Backwards;
        followUp = -1f;
    }

    private void SetNofollowUp()
    {
        currentState = CameraState.NoFollowUp;
    }

    private void SetNoFollowAhead()
    {
        currentState = CameraState.NoFollowAhead;
        followAhead = .5f;
    }

    private void SetFixedX()
    {
        currentState = CameraState.FixedX;
    }

    private void SetFixedY()
    {
        currentState = CameraState.FixedY;
    }

    public void SetDefaultValues()
    {
        thisCamera.orthographicSize = initialSize;
        currentState = CameraState.Normal;

        smoothCamera = 3.9f;
        followAhead = .9f;
        followUp = 1f;

        stepsToTarget = 100;
        initialSize = 2.8f;
        globalFreezeTime = 70;

        ToggleCanvas(true);
        ToggleChat(true);
    }

    private void ToggleChat(bool active)
    {

        GameObject inputChat = GameObject.Find("PanelInput");
        GameObject panelChat = GameObject.Find("PanelChat");

        if (panelChat)
        {
            panelChat.SetActive(active);
        }

        if (inputChat)
        {
            inputChat.SetActive(active);
        }
    }

    private void ToggleCanvas(bool active)
    {

        GameObject canvas = GameObject.Find("Canvas");

        if (canvas)
        {
            canvas.SetActive(active);
        }
    }

    #endregion

}
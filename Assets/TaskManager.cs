using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    #region GlobalVariables
    public int taskId;
    [System.Serializable]
    public struct PlayersPoiInTask
    {
        public int playerId;
        public int[] poisInTask;
        public bool taskDone;
    }
    public PlayersPoiInTask[] playersInTask;

    #endregion

    #region UtilsAndEvents
    public void HandlePlayerUsedPoi(int incomingPoi, int playerId)
    {
        //Set Global Variable to Check if player Solved Task
        int poisSolved = 0;

        foreach (PlayersPoiInTask playerInTask in playersInTask)
        {
            //Check Incoming Player
            if (playerInTask.playerId == playerId)
            {
                SolvePoiInTaskForPlayer(playerInTask, incomingPoi, playerId, poisSolved);
            }
        }
    }

    private void SolvePoiInTaskForPlayer(PlayersPoiInTask playerInTask, int incomingPoi, int playerId, int poisSolved)
    {
        //If Player correct, Checks pois solved by player
        for (int i = 0; i < playerInTask.poisInTask.Length; i++)
        {
            if (playerInTask.poisInTask[i] == incomingPoi)
            {
                playerInTask.poisInTask[i] = -1;
                poisSolved++;
            }

            else if (playerInTask.poisInTask[i] == -1)
            {
                poisSolved++;
            }

            // If player Has solved all pois in Task, Check if player has control and send Message to Server
            if (poisSolved == playerInTask.poisInTask.Length)
            {
                playerInTask.taskDone = true;
                LevelManager lManager = FindObjectOfType<LevelManager>();
                if (lManager.GetLocalPlayerController().controlOverEnemies)
                {
                    SendTaskReadyByPlayer(taskId, playerId);
                    CheckIFAllPlayersSolvedTask();
                }
            }


        }
    }

    private void CheckIFAllPlayersSolvedTask()
    {
        int playersReady = 0; 
        foreach (PlayersPoiInTask player in playersInTask)
        {
            if (player.taskDone)
            {
                playersReady++;
            }
            if (playersReady == playersInTask.Length)
            {
                SendTaskReadyByGroup(taskId);
            }
        }
    }

    #endregion

    #region Messaging
    public void SendTaskReadyByPlayer(int taskSolved, int playerId)
    {
        Client.instance.SendMessageToServer("TaskReadyByPlayer" + "/" + playerId.ToString() + "/" +
                                                taskSolved.ToString(), true);
    }

    public void SendTaskReadyByGroup(int taskSolved)
    {
        Client.instance.SendMessageToServer("TaskReadyByGroup" + "/" +
                                                taskSolved.ToString(), true);
    }
    #endregion
}

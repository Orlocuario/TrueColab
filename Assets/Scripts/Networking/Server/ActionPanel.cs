﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour {

    #region Buttons

    public void ChatButton()
    {
        List<Room> rooms = Server.instance.rooms;
        foreach (Room room in rooms)
        {
            room.CreateTextChat();
        }
    }

    public void ServerButton()
    {
        Server.instance.InitializeBroadcast();
    }

    public void ResetServer()
    {
        Server.instance.Reset();
    }

    public void MaxPlayerRoomButton()
    {
        Text inputText = GameObject.Find("InputPlayerText").GetComponent<Text>();
        int number = Int32.Parse(inputText.text);
        Server.instance.maxPlayers = number;
    }

    public void SceneToLoadButton()
    {
        Text inputText = GameObject.Find("InputSceneText").GetComponent<Text>();
        Server.instance.sceneToLoad = "Escena" + inputText.text;
    }

    public void KillRoom(int boxId, int sceneToLoad)
    {
        RoomManager roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        Server.instance.DestroyRoom(roomManager.GetRoomFromRoomBox(boxId));
        roomManager.FreeSpace(boxId);
    }

    public void SendRoomToScene(int boxId)
    {

    }
    
    #endregion

}

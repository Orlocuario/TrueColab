﻿using UnityEngine;
using System.Collections;
using System;
using System.Globalization;

public class ServerMessageHandler
{
    Server server;

    public ServerMessageHandler(Server server)
    {
        this.server = server;
    }

    public void HandleMessage(string message, int connectionId)
    {
        char[] separator = new char[1] { '/' };
        string[] msg = message.Split(separator);

        switch (msg[0])
        {
            case "ChangeScene":
                HandleChangeScene(msg, connectionId);
                break;
            case "ObjectMoved":
                SendObjectMoved(message, connectionId);
                break;
            case "ObjectDestroyed":
                SendObjectDestroyed(message, connectionId);
                break;
            case "ObstacleDestroyed":
                HandleObstacleDestroyed(msg, connectionId);
                break;
            case "ChangeObjectPosition":
                SendUpdatedObjectPosition(message, connectionId);
                break;
            case "InstantiateObject":
                SendInstantiation(message, connectionId);
                break;
            case "NewChatMessage":
                SendNewChatMessage(message, connectionId);
                break;
            case "ChangeHpHUDToRoom":
                SendHpHUDToRoom(msg, connectionId);
                break;
            case "ChangeMpHUDToRoom":
                SendMpHUDToRoom(msg, connectionId);
                break;
            case "StopChangeHpAndMpHUDToRoom":
                StopChangeHPMpHUDToRoom(msg, connectionId);
                break;
            case "ChangeHpAndMpHUDToRoom": //Necessary coz' ChatZone changes both at the same rate
                SendHpHAndMpHUDToRoom(msg, connectionId);
                break;
            case "GainExp":
                SendExpToRoom(msg, connectionId);
                break;
            case "EnemyRegisterId":
                NewEnemy(msg, connectionId);
                break;
            case "EnemyDied":
                EnemyDied(message, msg, connectionId);
                break;
            case "EnemyChangePosition":
                EnemyChangePosition(message, msg, connectionId);
                break;
            case "EnemyPatrollingPoint":
                SendEnemyPatrollingPoint(message, msg, connectionId);
                break;
            case "EnemiesStartPatrolling":
                EnemiesStartPatrolling(connectionId);
                break;
            case "PlayerRequestId":
                SendAllData(connectionId,Server.instance.GetPlayer(connectionId).room); //Manda todo para manejar mejor reconexiones. Inclusive información de playerId.
                break;
            case "PlayerAttack":
                SendAttackState(message, connectionId, msg);
                break;
            case "PlayerPower":
                SendPowerState(message, connectionId, msg);
                break;
            case "PlayerChangePosition":
                SendUpdatedPosition(message, connectionId, msg);
                break;
            case "PlayerTookDamage":
                SendPlayerTookDamage(message, connectionId);
                break;
            case "PlayerVote":
                SendPlayerVoted(message, connectionId);
                break;
            case "PlayerPreVote":
                SendPlayerPreVoted(message, connectionId);
                break;
            case "CreateGameObject":
                SendNewGameObject(message, connectionId);
                break;
            case "DestroyObject":
                SendDestroyObject(message, connectionId);
                break;
            case "OthersDestroyObject":
                SendOthersDestroyObject(message, connectionId);
                break;
            case "InventoryUpdate":
                SendInventoryUpdate(message, connectionId);
                break;
            case "ChangeSwitchStatus":
                SendChangeSwitchStatus(message, msg, connectionId);
                break;
            case "SwitchGroupReady":
                SendSwitchGroupAction(message, msg, connectionId);
                break;
            case "ActivateSystem":
                SendActivateSystem(message, connectionId, msg);
                break;
            case "ActivateNPCLog": // No se si es necesario o no, ya que puedes llamar el metodo desde afuera (start o script)
                SendActivationNPC(msg, connectionId);
                break;
            case "IgnoreCollisionBetweenObjects":
                SendIgnoreCollisionBetweenObjects(message, connectionId);
                break;
            case "BubbleInstantiatorData":
                SendBubbleInstantiatorDaTa(message, connectionId);
                break;
            case "CoordinateRotators":
                SendRotatorsData(message, connectionId);
                break;
            case "CoordinateInstantiators":
                SyncPlatformInstantiators(message, connectionId);
                break;
            case "CoordinateMovingObject":
                SyncMovingObjects(message, connectionId);
                break;
            case "EnterPOI":
                HandleEnterPOI(msg, connectionId);
                break;
            case "ReadyPoi":
                HandleReadyPoi(msg, connectionId);
                break;
            default:
                break;
        }
    }

    private void HandleReadyPoi(string[] msg, int connectionId)
    {
        string poiID = msg[1].ToString();
        NetworkPlayer player = server.GetPlayer(connectionId);
        RoomLogger log = player.room.log;
        log.WritePoiIsReady(player.id, poiID);

        Debug.Log("POI " + poiID + " reached by all needed players in room " + player.room.id);

    }

    private void HandleEnterPOI(string[] msg, int connectionID)
    {
        string poiID = msg[1].ToString();
        NetworkPlayer player = server.GetPlayer(connectionID);
        RoomLogger log = player.room.log;
        log.WriteEnterPOI(player.id, poiID);
        Debug.Log("POI " + poiID + " reached by " + player.id + " in room " + player.room.id);
    }

    private void HandleChangeScene(string[] msg, int connectionId)
    {
        string scence = msg[1];

        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        int totalExp = room.hpManager.currentExp;
        RoomLogger log = room.log;
        log.WriteTotalExp(totalExp);

        SendChangeScene(scence, room);
    }


    private void EnemiesStartPatrolling(int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.EnemiesStartPatrolling();
    }

    //Usado para sincronizar estado del servidor con un cliente que se está reconectando
    public void SendAllData(int connectionId, Room room)
    {
        SendPlayerIdAndControl(connectionId);
        foreach (NetworkPlayer player in room.players)
        {
            room.SendMessageToPlayer(player.GetReconnectData(), connectionId, true);
        }

        foreach (RoomSwitch switchi in room.switchs)
        {
            room.SendMessageToPlayer(switchi.GetReconnectData(), connectionId, true);
        }

        foreach (string doorMessage in room.systemsManager.GetSystemsMessages())
        {
            room.SendMessageToPlayer(doorMessage, connectionId, true);
        }

        foreach (string obstacleMessage in room.obstacleManager.GetObstaclesMessages())
        {
            room.SendMessageToPlayer(obstacleMessage, connectionId, true);
        }

    }

    private void SendIgnoreCollisionBetweenObjects(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayers(message, true);
    }

    public void SendActivationNPC(string[] msg, int connectionId) // Manda un mensaje a un solo jugador
    {
        string message = msg[1];
        int playerId = int.Parse(msg[2]);
        int newConnectionId = 0;

        Room room = server.GetPlayer(connectionId).room;

        foreach (NetworkPlayer jugador in room.players)
        {
            if (playerId == jugador.id)
            {
                newConnectionId = jugador.connectionId;
                break;
            }
        }

        if (!message.Contains("ActivateNPCLog"))
        {
            message = "ActivateNPCLog/" + message;
        }

        server.NPCsLastMessage = message;
        room.SendMessageToPlayer(message, newConnectionId, true); // Message es el texto a mostrar en el NPC Log
        room.WriteFeedbackRecord(message + "/" + playerId);
    }

    private void SendActivateSystem(string message, int connectionId, string[] msg)
    {
        string systemName = msg[1];

        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;

        room.SendMessageToAllPlayersExceptOne(message, connectionId, true);
        room.systemsManager.AddSystem(systemName);
    }

    private void HandleObstacleDestroyed(string[] msg, int connectionId)
    {
        string obstacleName = msg[1];

        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;

        room.obstacleManager.AddObstacle(obstacleName);
    }

    private void SendSwitchGroupAction(string message, string[] msg, int connectionId)
    {
        // OBSOLETO <- por qué?
        int groupId = Int32.Parse(msg[1]);

        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;

        if (!room.activatedSwitchGroups.Contains(groupId))
        {
            room.activatedSwitchGroups.Add(groupId);
        }
    }

    private void SendChangeSwitchStatus(string message, string[] msg, int connectionId)
    {
        int groupId = Int32.Parse(msg[1]);
        int individualId = Int32.Parse(msg[2]);
        bool on = bool.Parse(msg[3]);

        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;

        room.SetSwitchOn(on, groupId, individualId);
        room.SendMessageToAllPlayersExceptOne(message, connectionId, true);
    }

    private void EnemyChangePosition(string message, string[] msg, int connectionId)
    {
        int enemyId = Int32.Parse(msg[1]);
        int directionX = Int32.Parse(msg[2]);
        float posX = float.Parse(msg[3]);
        float posY = float.Parse(msg[4]);

        NetworkPlayer player = server.GetPlayer(connectionId);
        NetworkEnemy enemy = player.room.GetEnemy(enemyId);

        if (enemy != null)
        {
            enemy.SetPosition(directionX, posX, posY);
            player.room.SendMessageToAllPlayersExceptOne(message, connectionId, false);
        }
    }

    private void SendEnemyPatrollingPoint(string message, string[] msg, int connectionId)
    {
        int enemyId = Int32.Parse(msg[1]);
        int directionX = Int32.Parse(msg[2]);
        float posX = float.Parse(msg[3]);
        float posY = float.Parse(msg[4]);
        float patrolX = float.Parse(msg[5]);
        float patrolY = float.Parse(msg[6]);

        NetworkPlayer player = server.GetPlayer(connectionId);
        NetworkEnemy enemy = player.room.GetEnemy(enemyId);

        if (enemy != null)
        {
            enemy.SetPatrollingPoint(directionX, posX, posY, patrolX, patrolY);
            player.room.SendMessageToAllPlayersExceptOne(message, connectionId, true);
        }
    }

    private void EnemyDied(string message, string[] msg, int connectionId)
    {
        int enemyId = Int32.Parse(msg[1]);

        NetworkPlayer player = server.GetPlayer(connectionId);
        NetworkEnemy enemy = player.room.GetEnemy(enemyId);

        if (enemy != null)
        {
            enemy.Die();
        }
    }

    private void NewEnemy(string[] msg, int connectionId)
    {
        int instanceId = Int32.Parse(msg[1]);
        int id = Int32.Parse(msg[2]);
        float hp = float.Parse(msg[3]);

        int directionX = Int32.Parse(msg[4]);
        float posX = float.Parse(msg[5]);
        float posY = float.Parse(msg[6]);

        string message = "EnemyRegistered/" + instanceId + "/" + id + "/" + directionX + "/" + posX + "/" + posY;

        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        NetworkEnemy enemy = room.AddEnemy(instanceId, id, hp); ;

        enemy.SetPosition(directionX, posX, posY);

        if (msg.Length >= 9)
        {
            float patrolX = float.Parse(msg[7]);
            float patrolY = float.Parse(msg[8]);

            message += ("/" + patrolX + "/" + patrolY);
            enemy.SetPatrollingPoint(directionX, posX, posY, patrolX, patrolY);
        }

        room.SendMessageToAllPlayers(message, true);
    }

    private void SendNewGameObject(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayers(message + "/" + player.id, true);
    }

    private void SendInventoryUpdate(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        player.InventoryUpdate(message);
        player.room.log.WriteInventory(player.id, message);
    }

    private void SendDestroyObject(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayers(message, true);
    }

    private void SendOthersDestroyObject(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayersExceptOne(message, connectionId, true);
    }

    private void SendPlayerVoted(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayersExceptOne(message, connectionId, true);
    }

    private void SendPlayerPreVoted(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayersExceptOne(message, connectionId, true);
    }

    private void SendHpHUDToRoom(string[] msg, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.hpManager.ChangeHP(msg[1], connectionId);
    }

    private void SendMpHUDToRoom(string[] msg, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.hpManager.ChangeMP(msg[1], connectionId);
    }


    private void StopChangeHPMpHUDToRoom(string[] msg, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.hpManager.StopChangeHpAndMpHUD(connectionId);
    }

    private void SendHpHAndMpHUDToRoom(string[] msg, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.hpManager.RecieveHpAndMpHUD(msg[1], connectionId);
    }

    private void SendExpToRoom(string[] msg, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.hpManager.ChangeExp(msg[1]);
    }

    private void SendNewFireball(string message, int connectionId, string[] data)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayersExceptOne(message, connectionId, false);
    }

    private void SendNewProjectile(string message, int connectionId, string[] data)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayersExceptOne(message, connectionId, false);
    }

    private void SendNewChatMessage(string chatMessage, int connectionID)
    {

        NetworkPlayer player = server.GetPlayer(connectionID);
        Room room = player.room;
        room.SendMessageToAllPlayers(chatMessage, false);
    }

    private void SendPlayerTookDamage(string message, int connectionID)
    {
        NetworkPlayer player = server.GetPlayer(connectionID);
        Room room = player.room;
        room.SendMessageToAllPlayersExceptOne(message, connectionID, false);
    }

    private void SendUpdatedPosition(string message, int connectionID, string[] data)
    {
        NetworkPlayer player = server.GetPlayer(connectionID);
        Room room = player.room;

        int charId = Int32.Parse(data[1]);
        float positionX = float.Parse(data[2]);
        float positionY = float.Parse(data[3]);
        int directionX = Int32.Parse(data[4]);
        int directionY = Int32.Parse(data[5]);
        float speedX = float.Parse(data[6]);
        bool isGrounded = bool.Parse(data[7]);
        bool pressingJump = bool.Parse(data[8]);
        bool pressingLeft = bool.Parse(data[9]);
        bool pressingRight = bool.Parse(data[10]);

        player.positionX = positionX;
        player.positionY = positionY;
        player.directionX = directionX;
        player.directionY = directionY;
        player.speedX = speedX;
        player.isGrounded = isGrounded;
        player.pressingJump = pressingJump;
        player.pressingLeft = pressingLeft;
        player.pressingRight = pressingRight;

        room.SendMessageToAllPlayersExceptOne(message, connectionID, false);
        room.log.WriteNewPosition(player.id, positionX, positionY, pressingJump, pressingLeft, pressingRight);
    }

    private void SendObjectMoved(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayersExceptOne(message, connectionId, true);
    }

    private void SendObjectDestroyed(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayersExceptOne(message, connectionId, true);
    }

    private void SendUpdatedObjectPosition(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayersExceptOne(message, connectionId, false);
    }

    private void SendInstantiation(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayers(message, true);
    }

    private void SendPlayerIdAndControl(int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);

        string message = "PlayerSetId/" + player.id + "/" + player.controlOverEnemies;

        server.SendMessageToClient(connectionId, message, true);
    }

    private void SendBubbleInstantiatorDaTa(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayers(message, true);
    }

    private void SendRotatorsData(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayers(message, true);
    }

    private void SyncPlatformInstantiators(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayers(message, true);
    }

    private void SyncMovingObjects(string message, int connectionId)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayers(message, true);
    }
    public void SendChangeScene(string sceneName, Room room)
    {
        string message = "ChangeScene/" + sceneName;
        room.sceneToLoad = sceneName;
        room.SendMessageToAllPlayers(message, true);
        room.Reset();
    }

    public void SendAttackState(string message, int connectionId, string[] data)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        room.SendMessageToAllPlayersExceptOne(message, connectionId, false);
        room.log.WriteAttack(player.id);
    }

    public void SendPowerState(string message, int connectionId, string[] data)
    {
        NetworkPlayer player = server.GetPlayer(connectionId);
        Room room = player.room;
        player.power = bool.Parse(data[2]);
        room.SendMessageToAllPlayersExceptOne(message, connectionId, false);
        room.log.WritePower(player.id, player.power);
    }
}

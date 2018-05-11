using System.Collections.Generic;
using UnityEngine;

public class RoomBox
{
    public enum PlayersID { None, Mage, Warrior, Engineer };
    public int roomId;
    public int boxId;
    public Room room;
    public Dictionary<string, PlayersID> currentPlayers;
    public RoomBox(int id, int boxId, Room room)
    {
        this.room = room;
        roomId = id;
        this.boxId = boxId;
        currentPlayers = new Dictionary<string, PlayersID>();
    }

    public bool AddPlayer(PlayersID player, string ip)
    {
        if (currentPlayers.ContainsValue(player))
        {
            Debug.LogError("CRITICAL ERROR ENCONTRAMOS EL BUG: Se intentó agregar más de una vez un " + player + " en el room " + roomId);
        }
        if (!currentPlayers.ContainsKey(ip))
        {
            currentPlayers.Add(ip, player);
            return true;
        }
        else
        {
            Debug.LogError("Se intentó agregar dos veces al jugador con ip " + ip +" al room" + roomId);
            return false;
        }
    }

    public void DeletePlayer(string ip)
    {
        currentPlayers.Remove(ip);
    }
}
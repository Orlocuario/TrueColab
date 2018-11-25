using System;

public class RoomHpMp
{

    #region Attributes

    public int currentExp;  //TODO: Variable Global EXP
    public int maxHP;
    public int maxMP;
    public int maxExp;
    public int currentHP;
    public int currentMP;
    public float percentageHP;
    public float percentageMP;
    public float percentageExp;
    private bool mpAtLimit;

    private int mpRateDivider;

    private NetworkPlayer[] regeneratingPlayers;
    private NetworkPlayer[] manaSpendingPlayers;

    Room room;

    #endregion

    #region Constructor

    public RoomHpMp(Room room)
    {
        this.room = room;
        maxHP = 250;
        maxMP = 250;
        //maxExp = 250;
        currentHP = maxHP;
        currentMP = maxMP;
        currentExp = 0;
        percentageHP = 1;
        percentageMP = 1;
        percentageExp = 0;

        regeneratingPlayers = new NetworkPlayer[3];
        manaSpendingPlayers = new NetworkPlayer[3];
    }

    #endregion

    #region RegenerationWork


    public void RecieveHpAndMpHUD(string ip)
    {
        if (IsPlayerSlotEmpty(ip))
        {
            GetPlayerRegenerating();
        }
    }

    private bool IsPlayerSlotEmpty(string ip)
    {
        int id = GetPlayerId(ip);
        if (regeneratingPlayers[id] != null)
        {
            return false;
        }
        else
        {
            regeneratingPlayers[id] = room.players[id];
            return true;
        }
    }

    private bool IsPlayerSlotOccupied(string ip)
    {
        int id = GetPlayerId(ip);
        if (regeneratingPlayers[id] == null)
        {
            return false;
        }
        else
        {
            regeneratingPlayers[id] = null;
            return true;
        }
    }


    private void GetPlayerRegenerating()
    {
        SetRegenerationParameters();
    }

    private void GetPlayerStopRegenerating()
    {
        SetRegenerationParameters();
    }

    public void StopChangeHpAndMpHUD(string ip)
    {
        if (IsPlayerSlotOccupied(ip))
        {
            GetPlayerStopRegenerating();
            //room.SendMessageToAllPlayers("DisplayStopChangeHPMPToClient/", false);
        }
    }

    private void SetRegenerationParameters()
    {
        int playersIn = 0;

        for (int i = 0; i < regeneratingPlayers.Length; i++)
        {
            if (regeneratingPlayers[i] != null)
            {
                playersIn++;
            }
        }

        int regenerationFrameRateDivider = playersIn;
        room.SendMessageToAllPlayers("ChangeRegeneration/" + regenerationFrameRateDivider.ToString(), true);

    }

    #endregion

    #region ManaSpending

    public void ReceivePowerStateChange(string ip, bool powerState, int incomingMP)
    {
        if (powerState == true)
        {
            if (IsPlayerMPSlotEmpty(ip))
            {
                room.hpMpManager.currentMP = incomingMP;
                int id = GetPlayerId(ip);
                GetPlayerSpendingMana(id);
            }
        }

        else
        {
            if (IsPlayerMPSlotOccupied(ip))
            {
                room.hpMpManager.currentMP = incomingMP;
                int id = GetPlayerId(ip);
                GetPlayerStopSpendingMana(id);
            }
        }
    }

    private bool IsPlayerMPSlotEmpty(string ip)
    {
        int id = GetPlayerId(ip);
        if (manaSpendingPlayers[id] != null)
        {
            return false;
        }
        else
        {
            manaSpendingPlayers[id] = room.players[id];
            return true;
        }
    }

    private bool IsPlayerMPSlotOccupied(string ip)
    {
        int id = GetPlayerId(ip);
        if (manaSpendingPlayers[id] == null)
        {
            return false;
        }
        else
        {
            manaSpendingPlayers[id] = null;
            return true;
        }
    }

    private void GetPlayerSpendingMana(int id)
    {
        SetMPParameters(id);
    }

    private void GetPlayerStopSpendingMana(int id)
    {
        SetMPParameters(id);
    }

    private void SetMPParameters(int id)
    {
        int playersIn = 0;
        for (int i = 0; i < manaSpendingPlayers.Length; i++)
        {
            if (manaSpendingPlayers[i] != null)
            {
                playersIn++;
            }
        }

        mpRateDivider = playersIn;

        if (mpRateDivider == 0)
        {
            room.SendMessageToAllPlayers("StopSpendingMana/" + mpRateDivider.ToString(), true);
        }
        else
        {
            room.SendMessageToAllPlayers("StartSpendingMana/" + mpRateDivider.ToString(), true);
        }
    }
    #endregion

    #region Utils
    private int GetPlayerId(string ip)
    {
        NetworkPlayer incomingPlayer = Server.instance.GetPlayer(ip);
        int id = incomingPlayer.id;
        return id;
    }

    public void ChangeHPFromDamage(int damage)
    {
        room.hpMpManager.currentHP -= damage;
        room.SendMessageToAllPlayers("DisplayChangeHPToClient/" + damage.ToString(), true);
    }

    public void ChangeMP(int deltaMP)
    {
        currentMP += deltaMP;

        if (currentMP > maxMP)
        {
            currentMP = maxMP;
        }
        else if (currentMP <= 0)
        {
            currentMP = 0;
        }

        percentageMP = currentMP / maxMP;
        currentMP = currentMP - deltaMP;
        if (percentageMP == 1 || percentageMP == 0)
        {
            if (mpAtLimit)
            {
                return;
            }
            mpAtLimit = true;
        }
        else
        {
            mpAtLimit = false;
        }

        room.SendMessageToAllPlayers("DisplayChangeMPToClient/" + percentageMP, false);
    }

    public void ChangeExp(string deltaExp)
    {
        int valueDeltaExp = Int32.Parse(deltaExp);
        currentExp += valueDeltaExp;

        room.SendMessageToAllPlayers("DisplayChangeExpToClient/" + currentExp, true);
    }

    public float GetExp()
    {
        return currentExp;
    }

    /* public void ChangeExp(string deltaExp)
    {
        float valueDeltaExp = float.Parse(deltaExp);
        currentExp += valueDeltaExp;

        if (currentExp >= maxExp)
        {
            currentExp = 0;
            // levelUp
        }

        percentageExp = currentExp / maxExp;
        room.SendMessageToAllPlayers("DisplayChangeExpToClient/" + percentageExp, false);
    }

    public void ChangeMaxExp(string NewMaxExp)
    {
        maxExp = float.Parse(NewMaxExp);
        ChangeExp(NewMaxExp);
    } */

    #endregion

}

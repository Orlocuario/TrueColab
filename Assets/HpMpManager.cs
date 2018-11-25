using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HpMpManager : MonoBehaviour {

    public float maxHP;
    public float maxMP;
    public float maxExp;
    public float currentHP;
    public float currentMP;
    public float percentageHP;
    public float percentageMP;
    public float percentageExp;
    private bool mpAtLimit;

    public int hpCurrentAmount;
    public int mpCurrentAmount;

    private int mpCost;
    private static int standardFrameRate;
    private float mpSpendingRate;
    private float currentMpRate;
    private bool isSpendingMana;

    private int regenerationUnits;
    private int standardRegFrameRate;
    private float regenerationFrameRate;
    private int currentRegRate;
    private bool isRegenerating;

    private LevelManager lManager;
    private HUDDisplay hudDisplay;


    // Use this for initialization
    void Start() {

        maxHP = 250;
        maxMP = 250;
        //maxExp = 250;
        currentHP = maxHP;
        currentMP = maxMP;
        percentageHP = 1;
        percentageMP = 1;
        percentageExp = 0;


        mpCost = -1;
        mpSpendingRate = 90;
        standardFrameRate = 90;
        currentMpRate = 0;
        isSpendingMana = false;

        regenerationUnits = 5;
        standardRegFrameRate = 60;
        regenerationFrameRate = 60;
        currentRegRate = 0;
        isRegenerating = false;

        lManager = FindObjectOfType<LevelManager>();
        hudDisplay = lManager.hpAndMp; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRegenerating)
        {
            currentRegRate++;
            if (currentRegRate >= regenerationFrameRate)
            {
                currentRegRate = 0;
                ChangeHP(regenerationUnits);
                ChangeMP(regenerationUnits);
            }
        }

        if (isSpendingMana)
        {
            currentMpRate++;

            if (currentMpRate >= mpSpendingRate)
            {
                currentMpRate = 0;
                ChangeMP(mpCost);
            }
        }
    } 

    public void ReceivePlayerStartSpendingMana(int rateDivider)
    {
        mpSpendingRate = standardFrameRate / rateDivider;
        currentMpRate = mpSpendingRate -= 2;
        isSpendingMana = true;
    }

    public void ReceivePlayerStopSpendingMana(int rateDivider)
    {
        if (rateDivider == 0)
        {
            currentRegRate = 0;
            mpSpendingRate = standardRegFrameRate;
            isSpendingMana = false;
        }
        else
        {
            mpSpendingRate = standardFrameRate /= rateDivider;
            isSpendingMana = true;
        }
    }


    public void ReceivePlayerChangedRegeneration(int rateDivider)
    {
        if (rateDivider <= 0)
        {
            rateDivider = 0;
            isRegenerating = false;
            regenerationFrameRate = 0;

            HUDDisplay hpAndMp = GameObject.FindObjectOfType<LevelManager>().hpAndMp;
            hpAndMp.StopLocalParticles(); // Only stop local particles
            return;
        }
        else
        {
            regenerationFrameRate = standardRegFrameRate / rateDivider;
            isRegenerating = true;
        }
    }

    public void ChangeHP(int deltaHP)
    {
        if (deltaHP == 0)
        {
            return;
        }

        currentHP += deltaHP;

        if (currentHP >= maxHP)
        {
            currentHP = maxHP;
        }

        else if (currentHP <= 0)
        {
            currentHP = 0;
            SendMessageToServer("PlayersAreDead/" + Server.instance.sceneToLoad);
            currentHP = maxHP;
            currentMP = maxMP;
        }

        percentageHP = currentHP / maxHP;
        hudDisplay.ChangeHP(deltaHP);
    }

    public void ChangeMP(int deltaMP)
    {
        currentMP += deltaMP;
        mpCurrentAmount =- deltaMP;

        if (currentMP > maxMP)
        {
            currentMP = maxMP;
        }
        else if (currentMP <= 0)
        {
            currentMP = 0;
        }

        percentageMP = currentMP / maxMP;

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

        if (hudDisplay != null)
        {
            hudDisplay.ChangeMP(deltaMP);
            mpCurrentAmount -= deltaMP;
        }
        else
        {
            hudDisplay = lManager.hpAndMp;
            if (hudDisplay)
            {
                hudDisplay.ChangeMP(deltaMP);
            }
            else
            {
                Debug.Log("Mefui a la mierda no sirvió de nada. No tengo huddisplay");
            }
        }
        //room.SendMessageToAllPlayers("DisplayChangeMPToClient/" + percentageMP, false);
    }


    protected void SendMessageToServer(string message)
    {
        if (Client.instance)
        {
            Client.instance.SendMessageToServer(message, true);
        }
    }

}

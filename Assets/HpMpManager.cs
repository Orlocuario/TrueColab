using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HpMpManager : MonoBehaviour
{
    public int maxHP;
    public int maxMP;
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
    void Start()
    {
        maxHP = 250;
        maxMP = 250;
        //maxExp = 250;
        currentHP = maxHP;
        currentMP = maxMP;
        mpCurrentAmount = maxMP;
        hpCurrentAmount = maxHP;
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
                Debug.Log("Im changing hp and mp with: " + regenerationUnits.ToString() + " units. From FixedUpdate");
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

    public void ReceivePlayerStartSpendingMana(int rateDivider, int incomingMP)
    {
        mpSpendingRate = standardFrameRate / rateDivider;
        currentMpRate = mpSpendingRate -= 2;
        isSpendingMana = true;
        if (incomingMP != mpCurrentAmount)
        {
            ChangeMP(incomingMP - mpCurrentAmount);
        }
    }

    public void ReceivePlayerStopSpendingMana(int rateDivider, int incomingMP)
    {
        if (rateDivider == 0)
        {
            currentRegRate = 0;
            mpSpendingRate = standardRegFrameRate;
            isSpendingMana = false;
            if (incomingMP != mpCurrentAmount)
            {
                ChangeMP(incomingMP - mpCurrentAmount);
            }
        }

        else
        {
            if (incomingMP != mpCurrentAmount)
            {
                ChangeMP(incomingMP - mpCurrentAmount);
            }
            mpSpendingRate = standardFrameRate /= rateDivider;
            isSpendingMana = true;
        }
    }


    public void ReceivePlayerChangedRegeneration(int rateDivider, int incomingMP, int incomingHP)
    {
        if (rateDivider <= 0)
        {
            rateDivider = 0;
            isRegenerating = false;
            regenerationFrameRate = 0;

            HUDDisplay hpAndMp = GameObject.FindObjectOfType<LevelManager>().hpAndMp;
            hpAndMp.StopLocalParticles(); // Only stop local particles
           /* if (incomingMP != mpCurrentAmount)
            {
                int howMuch = mpCurrentAmount - incomingMP;
                Debug.Log("Im changing  mp with: " + howMuch + " units. FromReceivePlayer. RateDiv = 0");
                ChangeMP(mpCurrentAmount - incomingMP);
            }
            if (incomingHP != hpCurrentAmount)
            {
                int howMuch = hpCurrentAmount - incomingHP;
                Debug.Log("Im changing hp with: " + howMuch + " units. FromReceivePlayer. RateDiv = 0");
                ChangeHP(hpCurrentAmount - incomingHP);
            }*/
            return;
        }
        else
        {
            if (incomingMP != mpCurrentAmount)
            {
                int howMuch = mpCurrentAmount - incomingMP;
                Debug.Log("Im changing  mp with: " + howMuch + " units. FromReceivePlayer. RateDiv != 0");
                ChangeMP(incomingMP - mpCurrentAmount);
            }
            if (incomingHP != hpCurrentAmount)
            {
                int howMuch = hpCurrentAmount - incomingHP;
                Debug.Log("Im changing hp with: " + howMuch + " units. FromReceivePlayer. RateDiv = 0");
                ChangeHP(incomingHP - hpCurrentAmount);
            }

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
        hpCurrentAmount += deltaHP;
        Debug.Log("hpCurrentAmount is: " + hpCurrentAmount);

        if (hpCurrentAmount > maxMP)
        {
            hpCurrentAmount = maxMP;
        }
        if (hpCurrentAmount <= 0)
        {
            hpCurrentAmount = 0;
        }

        if (currentHP >= maxHP)
        {
            currentHP = maxHP;
            if (LevelManager.lManager.GetLocalPlayerController().controlOverEnemies)
            {
                SendMessageToServer("MaxHPReached/");
            }
        }

        else if (currentHP <= 0)
        {
            currentHP = 0;

            if (FindObjectOfType<LevelManager>().GetLocalPlayerController().controlOverEnemies)
            {
                SendMessageToServer("PlayersAreDead/");
            }
            currentHP = maxHP;
            currentMP = maxMP;
        }

        percentageHP = currentHP / maxHP;
        if (!hudDisplay)
        {
            hudDisplay = FindObjectOfType<LevelManager>().hpAndMp;
            if (!hudDisplay)
            {
                Debug.LogError("HPManager doesnt have a HUDDISPLAY");
            }
        }
        if (hudDisplay)
        {
            hudDisplay.ChangeHP(deltaHP);
        }
    }

    public int GetCurrentHP()
    {
        return hpCurrentAmount;
    }

    public int GetCurrentMP()
    {
        return mpCurrentAmount;
    }

    public void ChangeMP(int deltaMP)
    {
        currentMP += deltaMP;
        mpCurrentAmount += deltaMP;         //int copy for data coordination

        Debug.Log("CurrentMP amount is: " + mpCurrentAmount);

        if (currentMP > maxMP)
        {
            currentMP = maxMP;
            if (LevelManager.lManager.GetLocalPlayerController().controlOverEnemies)
            {
                SendMessageToServer("MaxMPReached/");
            }
        }

        if (mpCurrentAmount > maxMP)
        {
            mpCurrentAmount = maxMP;
        }
        if (mpCurrentAmount <= 0)
        {
            mpCurrentAmount = 0;
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
    }

    public void SendUpdateExitToServer()
    {
        SendMessageToServer("StopChangeHpAndMpHUDToRoom/" + hpCurrentAmount.ToString() + "/" + mpCurrentAmount.ToString());
    }

    protected void SendMessageToServer(string message)
    {
        if (Client.instance)
        {
            Client.instance.SendMessageToServer(message, true);
        }
    }
}

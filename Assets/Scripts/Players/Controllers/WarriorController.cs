using System.Collections;
using UnityEngine;


public class WarriorController : PlayerController
{

    #region Attributes

    protected int attacks = 0;

    #endregion

    #region Utils

    protected override AttackController GetAttack()
    {
        string attackName = (isPowerOn) ? "SuperPunch" : "Punch";
        var attackType = new PunchController().GetType();

        GameObject attackObject = (GameObject)Instantiate(Resources.Load(attackPrefabName + attackName));
        PunchController attackController = (PunchController)attackObject.GetComponent(attackType);

        return attackController;
    }

    public bool IsWarriored (GameObject player)
    {
        if (isPowerOn)
        {
            PowerableObject[] powerables = FindObjectsOfType<PowerableObject>();

            foreach (PowerableObject powerable in powerables)
            {
                if (powerable.IsPowered())
                {
                    PowerableObject.Power power = powerable.GetActivatedPower();
                    if (power.caster.Equals(this) || power.attack.GetType().Equals(new PunchController().GetType()) || power.expectedParticle.GetType().Equals(new WarriorPoweredParticles().GetType()))
                    {
						if (power.InPowerArea(player, true))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    #endregion

}

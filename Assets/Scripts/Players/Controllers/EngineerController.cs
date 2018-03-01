using CnControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerController : PlayerController
{

    #region Attributes

    private bool jumpedInAir;

    #endregion

    #region Utils

    protected override AttackController GetAttack()
    {
        if (levelManager.GetWarrior().IsWarriored(this.gameObject))
        {
            var attackType = new PunchController().GetType();
            string attackName = (isPowerOn) ? "SuperPunch" : "Punch";
            GameObject attackObject = (GameObject)Instantiate(Resources.Load(attackPrefabName + attackName));
            PunchController attackController = (PunchController)attackObject.GetComponent(attackType);
            return attackController;
        }
        else
        {
            var attackType = new ProjectileController().GetType();
            string attackName = (isPowerOn) ? "SuperProjectile" : "Projectile";

            GameObject attackObject = (GameObject)Instantiate(Resources.Load(attackPrefabName + attackName));
            ProjectileController attackController = (ProjectileController)attackObject.GetComponent(attackType);
            return attackController;
        }

    }

    protected PunchController GetPunch()
    {
        var attackType = new PunchController().GetType();
        string attackName = (isPowerOn) ? "SuperPunchController" : "PunchController";
        GameObject attackObject = (GameObject)Instantiate(Resources.Load(attackPrefabName + attackName));
        PunchController attackController = (PunchController)attackObject.GetComponent(attackType);
        return attackController;
    }

    protected override bool IsJumping(bool isGrounded)
    {
        if (localPlayer)
        {

            if (!isPowerOn)
            {
                return base.IsJumping(isGrounded);
            }

            if (isGrounded)
            {
                jumpedInAir = false;
            }

            bool pressedJump = CnInputManager.GetButtonDown("Jump Button");

            if (pressedJump && isGrounded && !remoteJumping)
            {
                remoteJumping = true;
                SendPlayerDataToServer();
                return true;
            }

            if (pressedJump && !isGrounded && !jumpedInAir && !remoteJumping)
            {
                remoteJumping = true;
                jumpedInAir = true;
                SendPlayerDataToServer();
                return true;
            }

            if (remoteJumping)
            {
                remoteJumping = false;
                SendPlayerDataToServer();
            }

            return false;
        }

        return remoteJumping;
    }

    public bool IsElectrified(GameObject playerOrMovable)
    {
        PowerableObject[] powerables = FindObjectsOfType<PowerableObject>();

        foreach (PowerableObject powerable in powerables)
        {
            if (powerable.IsPowered())
            {
                PowerableObject.Power power = powerable.GetActivatedPower();
                if (power.caster.Equals(this))
                {
                    if (power.InPowerArea(playerOrMovable, true))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    #endregion



}


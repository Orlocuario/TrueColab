using UnityEngine;

public class MageController : PlayerController
{
    #region Attributes

    protected static float shieldArea;

    #endregion

    #region Start & Update

    protected override void Start()
    {
        base.Start();
        shieldArea = 0;
        LoadShieldArea();
    }

    protected override void Update()
    {
        base.Update();
        //levelManager._.DrawCircle(transform.position, shieldArea, null, Color.blue, this);
    }

    #endregion

    #region Common

    public bool ProtectedByShield(GameObject player)
    {
        // Check for player proximity to mage with activated shield
        if (isPowerOn)
        {
            if (Vector2.Distance(player.transform.position, transform.position) <= shieldArea)
            {
                return true;
            }
        }

        PowerableObject[] powerables = FindObjectsOfType<PowerableObject>();

        foreach (PowerableObject powerable in powerables)
        {
            if (powerable.IsPowered())
            {
                PowerableObject.Power power = powerable.GetActivatedPower();
                if (power.caster.Equals(this) || power.attack.Equals(new FireballController()) || power.expectedParticle.Equals(new MagePoweredParticles()))
                {
                    if (power.InPowerArea(player, false))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    #endregion

    #region Utils

    protected override AttackController GetAttack()
    {
        if (levelManager.GetWarrior().IsWarriored(gameObject))
        {
            var attackType = new PunchController().GetType();
            string attackName = (isPowerOn) ? "SuperPunch" : "Punch";
            GameObject attackObject = (GameObject)Instantiate(Resources.Load(attackPrefabName + attackName));
            PunchController attackController = (PunchController)attackObject.GetComponent(attackType);
            return attackController;
        }
        else
        {
            var attackType = new FireballController().GetType();
            string attackName = (isPowerOn) ? "SuperFireball" : "Fireball";
            GameObject attackObject = (GameObject)Instantiate(Resources.Load(attackPrefabName + attackName));
            FireballController attackController = (FireballController)attackObject.GetComponent(attackType);
            return attackController;
        }
    }

    protected void LoadShieldArea()
    {
        foreach (GameObject particle in particles)
        {
            float radius = particle.GetComponent<ParticleSystem>().shape.radius;
            if (shieldArea < radius)
            {
                shieldArea = radius;
            }
        }
    }

    #endregion

}

using System;
using UnityEngine;

public class PowerableObject : MonoBehaviour
{

    #region Attributes

    #region Enum

    public enum ActivationType
    {
        Attack,
        Power,
        ByParticle
    }

    #endregion

    #region Struct

    [Serializable]
    public struct Power
    {
        [Tooltip("How this object will be empowered")]
        public ActivationType activationType;

        [Tooltip("If ActivationType==Attack only")]
        public AttackController attack;

        [Tooltip("If ActivationType==Power only")]
        public PlayerController caster;

        [Tooltip("If ActivationType==ByParticle only")]
        public PoweredParticles expectedParticle;

        [Tooltip("Which particles activate with this power, they should be child objects of the PowerableObject")]
        public GameObject[] particles;

        public bool InPowerArea(GameObject player, bool includeSleepingParticles)
        {
            if (particles != null && particles.Length > 0)
            {
                foreach (GameObject particle in particles)
                {
                    if (particle.activeInHierarchy || includeSleepingParticles)
                    {
                        Collider2D collider = particle.GetComponent<Collider2D>();

                        if (collider && collider.isTrigger)
                        {
                            ParticleSystem particleWithTrigger = particle.GetComponent<ParticleSystem>();

                            if (particleWithTrigger)
                            {
                                if (ParticleIsCircle(particleWithTrigger))
                                {
                                    return Vector2.Distance(particleWithTrigger.transform.position, player.transform.position) <= particleWithTrigger.shape.radius;
                                }

                                else if (ParticleIsBox(particleWithTrigger))
                                {
                                    return PlayerInsideBox(player, particleWithTrigger);
                                }

                                return false;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public bool ParticleIsCircle(ParticleSystem ps)
        {
            return ps && ps.shape.shapeType.Equals(ParticleSystemShapeType.Circle);
        }

        public bool ParticleIsBox(ParticleSystem ps)
        {
            return ps && ps.shape.shapeType.Equals(ParticleSystemShapeType.Box);
        }

        public bool PlayerInsideBox(GameObject player, ParticleSystem particle)
        {
            Vector2 playerPosition = player.transform.position;
            Vector2 particlePosition = particle.transform.position;

            float baseDistance = particle.shape.box.x / 2;
            float verticalDistance = particle.shape.box.y / 2;

            //Debug.Log("Es " + (playerPosition.x <= particlePosition.x + baseDistance) + " que el player está en mi rango derecho");
            //Debug.Log("Es " + (playerPosition.x >= particlePosition.x - baseDistance) + " que el player está en mi rango izquierdo");
            //Debug.Log("Es " + (playerPosition.y <= particlePosition.y + verticalDistance) + " que el player está en mi rango cenital");
            //Debug.Log("Es " + (playerPosition.y >= particlePosition.y - verticalDistance) + " que el player está en mi rango subterráneo");

            bool inside = (
                playerPosition.x <= particlePosition.x + baseDistance &&
                playerPosition.x >= particlePosition.x - baseDistance &&
                playerPosition.y <= particlePosition.y + verticalDistance &&
                playerPosition.y >= particlePosition.y - verticalDistance);

            return inside;
        }
    }

    #endregion

    public Power[] powers;
    [Tooltip("How many frames to wait before shutdown? 30frames ~ 1 sec")]
    public int shutdownFrames;
    [Tooltip("If true the powerable must extend the 'DoYourPowerableThing' and 'DoYourPowerableThing' methods ")]
    public bool mustDoSomething;

    protected LevelManager levelManager;
    protected Power? activatedPower;

    protected int poweredFrameCount; // How many frames it has been powered.
    protected bool shutdown;
    protected bool powered;

    #endregion

    #region Start & Update

    protected void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        InitializeParticles();
        poweredFrameCount = 0;
    }

    protected void Update()
    {
        //DebugPowerAreas();
        //DebugPowerableTrigger();

        if (shutdown)
        {
            if (poweredFrameCount++ == shutdownFrames)
            {
                DeactivatePower();
            }
        }

    }

    #endregion

    #region Common

    public virtual void ActivatePower(Power power)
    {
        powered = true;
        poweredFrameCount = 0;
        activatedPower = power;
        ToggleParticles(power.particles, true);

        if (power.caster != null)
        {
            if (CasterIsEngineer(power))
            {
                TurnPlayersIfNeeded(power);
                TurnObjectsIfNeeded(power);
            }
        }

        if (mustDoSomething)
        {
            DoYourPowerableThing(); //This should be an override
        }

    }

    public virtual void DeactivatePower()
    {
        ToggleParticles(activatedPower.Value.particles, false);

        if (activatedPower.Value.caster != null)
        {
            if (CasterIsEngineer(activatedPower.Value))
            {
                GravityBackToNormal();
                TurnObjectsIfNeeded(activatedPower.Value);
            }
        }

        if (mustDoSomething)
        {
            UndoYourPowerableThing(); //this should be an override
        }

        powered = false;
        shutdown = false;
        activatedPower = null;
    }

    protected void TurnPlayersIfNeeded(Power power)
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        GameObject[] players = levelManager.players;

        foreach (GameObject player in players)
        {
            if (power.InPowerArea(player, true))
            {
                player.GetComponent<PlayerController>().SetPositiveGravity(false);
            }
        }
    }

    protected void TurnObjectsIfNeeded(Power power)
    {
        MovableObject[] objects = FindObjectsOfType<MovableObject>();

        foreach (MovableObject movable in objects)
        {
            if (power.InPowerArea(movable.gameObject, true))
            {
                Rigidbody2D movableRb2d = movable.GetComponent<Rigidbody2D>();
                movableRb2d.gravityScale *= -1;
            }
        }
    }


    protected void GravityBackToNormal()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        GameObject[] players = levelManager.players;

        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().SetPositiveGravity(true);
        }

    }

    protected virtual bool ActivatePower(Power power, GameObject gameObject)
    {
        switch (power.activationType)
        {
            case ActivationType.Attack:
                return ActivateWithAttack(power, gameObject);
            case ActivationType.Power:
                return ActivateWithPower(power, gameObject);
            case ActivationType.ByParticle:
                return ActivateWithParticle(power, gameObject);
            default:
                return false;
        }
    }


    protected bool ActivateWithParticle(Power power, GameObject possibleParticle)
    {
        if (power.expectedParticle != null)
        {
            if (ParticleActivatesPower(power.expectedParticle, possibleParticle))
            {
                ActivatePower(power);
                return true;
            }

            else
            {
                return false;
            }
        }
        Debug.LogError("The powerableObject named: " + gameObject.name + " has a ByParticle power set but no ParticleType set");
        return false; 
    }

    protected bool ActivateWithAttack(Power power, GameObject attack)
    {
        if (power.attack)
        {
            if (AttackActivatesPower(power.attack, attack))
            {
                ActivatePower(power);
                return true;
            }
        }
        else
        {
            Debug.LogError("This PowerableObject.ActivationType is 'Attack' but has no attack set.");
        }

        return false;
    }

    protected bool ActivateWithPower(Power power, GameObject playerGO) //CheckThis
    {
        if (power.caster)
        {
            if (PlayerActivatesPower(power.caster, playerGO))
            {
                PlayerController player = playerGO.GetComponent<PlayerController>();
                player.availablePowerable = gameObject;

                if (player.isPowerOn)
                {
                    ActivatePower(power);
                    return true;
                }

            }
        }
        else
        {
            Debug.LogError("This PowerableObject.ActivationType is 'Power' but has no caster set.");
        }

        return false;

    }

    public Power GetActivatedPower()
    {
        return activatedPower.Value;
    }

    public bool IsPowered()
    {
        return powered;
    }

    #endregion

    #region Events

    protected void OnTriggerEnter2D(Collider2D collision)
    {

        for (int i = 0; i < powers.Length; i++)
        {
            bool activated = ActivatePower(powers[i], collision.gameObject);
            if (activated)
            {
                // Start shutting down immediatelly if is attack activated
                if (powers[i].activationType.Equals(ActivationType.Attack))
                {
                    shutdown = true;
                }
                break;
            }
        }
    }


    protected void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player && player.availablePowerable == gameObject)
        {
            player.availablePowerable = null;
            if (powered)
            {
                shutdown = true;
            }
        }
    }


    #endregion

    #region Utils

    #region Debugs

    protected void DebugPowerAreas()
    {
        foreach (Power power in powers)
        {
            if (power.particles != null && power.particles.Length > 0)
            {
                levelManager._.DrawColliders(power.particles, true, this);
            }
        }

        /*
        if (power.caster)
        {

            Vector2 playerPosition = power.caster.transform.position;
            Vector2 particlePosition = ps.transform.position;
            Vector2 border = particlePosition;

            float baseDistance = boxCollider.size.x / 2;
            float verticalDistance = boxCollider.size.y / 2;

            if (playerPosition.x >= particlePosition.x + baseDistance)
            {
                border.x = particlePosition.x + baseDistance;
            }

            else if (playerPosition.x <= particlePosition.x - baseDistance)
            {
                border.x = particlePosition.x - baseDistance;
            }
            else
            {
                border.x = playerPosition.x;
            }

            if (playerPosition.y >= particlePosition.y + verticalDistance)
            {
                border.y = particlePosition.y + verticalDistance;
            }
            else if (playerPosition.y <= particlePosition.y - verticalDistance)
            {
                border.y = particlePosition.y - verticalDistance;
            }
            else
            {
                border.y = playerPosition.y;
            }

            levelManager._.DrawDistance(playerPosition, border, Color.green, this);
        }*/


    }

    protected void DebugPowerableTrigger()
    {
        levelManager._.DrawColliders(GetComponents<Collider2D>(), true, this);
    }

    #endregion

    #region Validators

    public bool CasterIsWarrior(Power power)
    {
        return power.caster.GetType().Equals(new WarriorController().GetType());
    }

    public bool CasterIsMage(Power power)
    {
        return power.caster.GetType().Equals(new MageController().GetType());
    }

    public bool CasterIsEngineer(Power power)
    {
        return power.caster.GetType().Equals(new EngineerController().GetType());
    }

    protected bool GameObjectIsPlayer(GameObject other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        return playerController && playerController.localPlayer;
    }

    public bool PlayerActivatesPower(PlayerController expectedCaster, GameObject playerGO)
    {
        PlayerController player = playerGO.GetComponent<PlayerController>();
        return player && player.GetType().Equals(expectedCaster.GetType());
    }

    public bool ActivatesWithPunch(Power power)
    {
        return power.attack && power.attack.GetType().Equals(new PunchController().GetType());
    }

    public bool ParticleActivatesPower(PoweredParticles expectedPArticle, GameObject posibleParticle)
    {
        if (posibleParticle.GetComponent<PoweredParticles>())
        {
            PoweredParticles poweredParticle = posibleParticle.GetComponent<PoweredParticles>();
            return poweredParticle.GetType().Equals(expectedPArticle.GetType());
        }
        else
        {
            return false;
        }
    }

    public bool AttackActivatesPower(AttackController expectedAttack, GameObject attackGO)
    {
        AttackController attack = attackGO.GetComponent<AttackController>();
        if (attack)
        {

            if (attack.IsPowered())
            {
                return attack.GetType().Equals(expectedAttack.GetType());
            }

        }

        return false;
    }

    #endregion

    #region Particles

    protected void ToggleParticles(GameObject[] particles, bool activate)
    {
        if (particles != null && particles.Length > 0)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].SetActive(activate);
            }
        }
    }

    protected void InitializeParticles()
    {
        if (powers != null && powers.Length > 0)
        {
            for (int i = 0; i < powers.Length; i++)
            {
                if (powers[i].particles != null && powers[i].particles.Length > 0)
                {
                    ToggleParticles(powers[i].particles, false);
                }
            }

        }

    }

    #endregion

    #region Animations


    #endregion

    #region Not Implemented

    protected virtual void UndoYourPowerableThing()
    {
        throw new NotImplementedException("Every powerable must implement an UndoPowerble method");
    }

    protected virtual void DoYourPowerableThing()
    {
        throw new NotImplementedException("Every powerable must implement a PowerableDo method");
    }

    #endregion

    #endregion

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{

    #region Attributes

    public enum MoveType
    {
        Target,
        Targets,
        Direction
    }
    protected MoveType moveType;
    protected Vector2 target;
    protected Vector2[] targets;
    protected SceneAnimator sceneAnim;

    protected float currentDistance;
    protected float maxDistance;
    protected bool initialized;
    protected float direction;
    protected bool isMoving;
    protected bool enhanced;
    protected float speed;
    public int targetsReached;
    protected bool yetNeeded;
    private bool imNeutral;
    private float timetoKillBubble;
    private LevelManager levelManager;

    private PlayerController[] playerControllers;
    private BubbleParticleController parasiteParticle;

    #endregion

    // Use this for initialization
    protected virtual void Start()
    {
        yetNeeded = true;
        currentDistance = 0;
        maxDistance = 6f;
        targetsReached = 0;
        sceneAnim = FindObjectOfType<SceneAnimator>();
        levelManager = FindObjectOfType<LevelManager>();
        playerControllers = new PlayerController[3];
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (isMoving)
        {
            Move();
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            int i = player.playerId;
            playerControllers[i] = player;

            playerControllers[i].parent = gameObject;
            other.transform.parent = transform;
        }
    }
    protected virtual void OnCollisionExit2D(Collision2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            int i = player.playerId;
            player.transform.parent = null;
            playerControllers[i] = null;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<DamagingObject>())
        {
            if (!levelManager.GetMage().ProtectedByShield(gameObject))
            {
                if (CheckifThereAreAnyPlayers())
                {
                    ReleasePlayers();
                    Destroy(parasiteParticle.gameObject);
                    Destroy(gameObject);
                }
            }
        }
        if (other.GetComponent<EnemyController>())
        {
            if (levelManager.GetWarrior())
            {
                if (levelManager.GetWarrior().IsWarriored(gameObject))
                {
                    EnemyController destroyable = other.GetComponent<EnemyController>();
                    destroyable.TakeDamage(100);
                }
                else
                {
                    if (CheckifThereAreAnyPlayers())
                    {
                        ReleasePlayers();
                        Destroy(parasiteParticle.gameObject);
                        Destroy(gameObject);
                    }

                    Destroy(parasiteParticle.gameObject);
                    Destroy(gameObject);
                }
            }
        }

        if (other.GetComponent<DestroyableObject>())
        {
            if (levelManager.GetWarrior())
            {
                if (levelManager.GetWarrior().IsWarriored(gameObject))
                {
                    DestroyableObject destroyable = other.GetComponent<DestroyableObject>();
                    destroyable.DestroyMe(true);
                }
                else
                {
                    for (int i = 0; i < playerControllers.Length; i++)
                    {
                        if (playerControllers[i] != null)
                        {
                            PlayerController playerToRelease = playerControllers[i];
                            playerToRelease.ResetTransform();
                            playerToRelease.TakeDamage(10, new Vector2(150f, 15f));
                            playerToRelease.parent = null;
                        }
                    }

                    Destroy(parasiteParticle.gameObject);
                    Destroy(gameObject);
                }
            }
        }

        if (other.GetComponent<KillingObject>() && other.GetComponent<KillingObject>().activated)
        {
            if (!levelManager.GetMage().ProtectedByShield(gameObject))
            {
                for (int i = 0; i < playerControllers.Length; i++)
                {
                    if (playerControllers[i] != null)
                    {
                        PlayerController playerToRelease = playerControllers[i];
                        playerToRelease.ResetTransform();
                        playerToRelease.parent = null;
                    }
                }
                if (CheckifThereAreAnyPlayers())
                {
                    Destroy(parasiteParticle.gameObject);
                    Destroy(gameObject);
                }
            }
            else if (levelManager.GetMage().ProtectedByShield(gameObject))
            {
                for (int i = 0; i < playerControllers.Length; i++)
                {
                    if (playerControllers[i] != null)
                    {
                        PlayerController playerSaved = playerControllers[i];
                        Physics2D.IgnoreCollision(playerSaved.GetComponent<BoxCollider2D>(), other.GetComponent<Collider2D>(), true);
                    }
                }

            }
        }
    }

    protected bool CheckifThereAreAnyPlayers()
    {
        for (int i = 0; i < playerControllers.Length; i++)
        {
            if (playerControllers[i] != null)
            {
                return true;
            }
        }
        return false;
    }

    protected void ReleasePlayers()
    {
        for (int i = 0; i < playerControllers.Length; i++)
        {
            if (playerControllers[i] != null)
            {
                PlayerController playerToRelease = playerControllers[i];
                playerToRelease.ResetTransform();
                playerToRelease.TakeDamage(10, new Vector2(150f, 15f));
                playerToRelease.parent = null;
            }
        }
    }

    protected bool MageActivatedBubblePower()
    {
        PowerableObject powerableObject = gameObject.GetComponent<PowerableObject>();
        if (powerableObject)
        {
            if (powerableObject.IsPowered())
            {
                PowerableObject.Power? activatedPower = powerableObject.GetActivatedPower();
                if (activatedPower.Value.caster != null)
                {
                    if (activatedPower.Value.caster.Equals(new MageController().GetType()))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    protected bool EngineerActivatedbubblePower()
    {
        PowerableObject powerableObject = gameObject.GetComponent<PowerableObject>();
        if (powerableObject)
        {
            if (powerableObject.IsPowered())
            {
                PowerableObject.Power? activatedPower = powerableObject.GetActivatedPower();
                if (activatedPower.Value.caster != null)
                {
                    if (activatedPower.Value.caster.Equals(new EngineerController().GetType()))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public void InitializeColouredBubbles(MoveType _moveType, PlayerController caster, GameObject bubbleParticle)
    {
        SetPowerableBubbleCaster(caster);
        SetPowerParticle(bubbleParticle);
        SetExpectedParticleType(bubbleParticle);
        moveType = _moveType;
        initialized = true;
    }

    public void InitializeNeutralBubble(MoveType _moveType, GameObject[] casters)
    {
        SetNeutralBubblePowerable(casters);
        moveType = _moveType;
        initialized = true;
    }
    private void SetExpectedParticleType(GameObject bubbleParticleForType)
    {
        PowerableObject.Power[] bubblePowers = gameObject.GetComponent<PowerableObject>().powers;
        if (bubblePowers.Length >= 2)
        {
            PoweredParticles typeOfParticle = bubbleParticleForType.GetComponent<PoweredParticles>();
            bubblePowers[2].expectedParticles = typeOfParticle;
        }
    }

    private void SetPowerParticle(GameObject bubbleParticle)
    {
        parasiteParticle = bubbleParticle.GetComponent<BubbleParticleController>();

        PowerableObject.Power[] bubblePowers = gameObject.GetComponent<PowerableObject>().powers;
        if (bubblePowers.Length >= 1)
        {
            bubblePowers[0].particles[0] = bubbleParticle;
        }
    }

    public void SetMovement(Vector2 startPosition, Vector2[] _targets, float _speed, float _timeToWait, float _timeToKillBubble)
    {
        if (!initialized || moveType.Equals(MoveType.Direction) || moveType.Equals(MoveType.Target))
        {
            Debug.LogError("Attack was not initialized correctly");
            return;
        }

        timetoKillBubble = _timeToKillBubble;
        targets = _targets;
        speed = _speed;
        transform.position = startPosition;

        StartCoroutine(WaitToMove(_timeToWait));

    }
    protected IEnumerator WaitToMove(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        isMoving = true;
    }

    protected bool GameObjectIsPlayer(GameObject other)
    {
        return other.GetComponent<PlayerController>();
    }

    protected void Move()
    {

        switch (moveType)
        {
            case MoveType.Direction:
                MoveInDirection();
                break;
            case MoveType.Targets:
                MoveToTarget();
                break;
        }

    }

    protected void MoveInDirection()
    {
        float distance = GetSpeedInDirection();

        transform.position += Vector3.right * distance;

        currentDistance += System.Math.Abs(distance);

        if (maxDistance <= currentDistance)
        {
            Destroy(gameObject);
        }
    }

    protected void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targets[targetsReached], GetSpeedToTarget());
        parasiteParticle.transform.position = gameObject.transform.position;

        if (transform.position.x == targets[targetsReached].x && transform.position.y == targets[targetsReached].y)
        {
            if (yetNeeded)
            {
                targetsReached++;

                if (targetsReached == targets.Length)
                {
                    Destroy(gameObject, timetoKillBubble);
                    Destroy(parasiteParticle.gameObject, timetoKillBubble);


                    for (int i = 0; i < playerControllers.Length; i++)
                    {
                        if (playerControllers[i] != null)
                        {
                            playerControllers[i].ResetTransform();
                            if (imNeutral)
                            {
                                playerControllers[i].TakeDamage(20, new Vector2(150, 15));
                            }
                        }
                    }
                    enabled = false;
                }

            }
        }
    }


    protected float GetSpeedToTarget()
    {
        return GetSpeed() * Time.deltaTime * 1.3f;
    }

    protected float GetSpeedInDirection()
    {
        return GetSpeed() * direction * Time.deltaTime;
    }

    private void SetPowerableBubbleCaster(PlayerController caster)
    {
        PowerableObject.Power[] bubblePowers = gameObject.GetComponent<PowerableObject>().powers;
        if (bubblePowers.Length >= 1)
        {
            bubblePowers[0].caster = caster;
        }
    }

    private void SetNeutralBubblePowerable(GameObject[] casters)
    {
        imNeutral = true;
        PowerableObject.Power[] bubblePowers = gameObject.GetComponent<PowerableObject>().powers;
        for (int i = 0; i < bubblePowers.Length; i++)
        {
            bubblePowers[i].caster = casters[i].GetComponent<PlayerController>();
        }
    }
    protected virtual float GetSpeed()
    {
        return speed; //Every Player Checks His Speed n' Shit
    }

}
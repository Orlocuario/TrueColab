﻿using CnControls;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    #region Attributes

    public PlannerPlayer playerObj;
    public Vector3 respawnPosition;
    public LayerMask whatIsGround;
    public GameObject[] particles;
    public Transform groundCheck;
    public GameObject parent;

    // Remote data
    public bool remoteAttacking;
    public bool remoteJumping;
    public bool remoteRight;
    public bool remoteLeft;
    public bool remoteUp;

    // Local data
    public bool rightPressed;
    public bool leftPressed;
    public bool jumpPressed;
    public bool localPlayer;
    public bool isGrounded;
    public bool upPressed;

	public GameObject availablePowerable;
    public GameObject availableChatZone;
    public GameObject availableInstantiatorTrigger; 
    public string decisionName;
    public bool controlOverEnemies;
    public float groundCheckRadius;
    public bool canAccelerate;
    public float acceleration;
    public float actualSpeed;
    public int mpUpdateFrame;
    public int sortingOrder;
    public int playerId;
    public bool mpDepleted;
    public bool isPowerOn;
    public int directionY;
    // 1 = de pie, -1 = de cabeza
    public int directionX;
    // 1 = derecha, -1 = izquierda
    public float gravityPower;

    protected SceneAnimator sceneAnimator;
    protected LevelManager levelManager;
    protected SpriteRenderer sprite;
    protected Vector3 lastPosition;
    protected Rigidbody2D rb2d;

    // Statics
    protected static float maxAcceleration = 1f;
    protected static float takeDamageRate = 1f;
    protected static float attackRate = .25f;
    protected static float maxXSpeed = 3f;
    protected static float maxYSpeed = 8f;

    protected static string attackPrefabName = "Prefabs/Attacks/";

    protected static int mpUpdateFrameRate = 30;
    protected static int mpSpendRate = -1;
    protected static int attackSpeed = 4;

    protected string attackAnimName;
    protected bool isTakingDamage;
    protected bool isAttacking;
    protected bool connected;
    protected bool canMove;
    protected float speedX;
    protected float speedY;


    protected int debuger;
    private CameraState cameraState;

    #endregion

    #region Start

    protected virtual void Start()
    {

        sceneAnimator = FindObjectOfType<SceneAnimator>();

        if (!sceneAnimator)
        {
            Debug.Log(name + " did not found the SceneAnimator");
        }

        levelManager = FindObjectOfType<LevelManager>();
        rb2d = GetComponent<Rigidbody2D>();

        respawnPosition = transform.position;

        attackAnimName = "Attacking";

        controlOverEnemies = false;
        canAccelerate = false;
        isAttacking = false;
        localPlayer = false;
        isGrounded = false;
        mpDepleted = false;
        isPowerOn = false;
        connected = true;
        canMove = true;
        availableChatZone = null;
		decisionName = null;
		availablePowerable = null; 
        gravityPower = 2.3f;

        remoteAttacking = false;
        remoteJumping = false;
        remoteRight = false;
        remoteLeft = false;
        remoteUp = false;

        mpUpdateFrame = 0;
        acceleration = 0f;
        sortingOrder = 0;
        directionY = 1;
        directionX = 1;
        debuger = 0;

        SetPositiveGravity(true);
        InitializeParticles();
        IgnoreCollisionWithObjectsWhoHateMe();
        IgnoreCollisionBetweenPlayers();


        // TODO: Remove this.
        FindObjectOfType<SoundManager>().PlaySound(gameObject, GameSounds.PlayerAttack, false);
    }

    #endregion

    #region Update

    protected virtual void Update()
    {
        if (!connected || !canMove)
        {
            return;
        }

        if (transform.parent != null)
        {
            parent = transform.parent.gameObject;
        }

        Move();
        Attack();
        UsePower();
        UseItem();
    }

    #endregion

    #region Common

    #region Connection

    public void Connect(bool _connected)
    {
        connected = _connected;

        remoteJumping = false;
        remoteRight = false;
        remoteLeft = false;

        SendPlayerDataToServer();
    }

    public void Activate(int _playerId)
    {
        localPlayer = true;
        playerId = _playerId;
        sprite = GetComponent<SpriteRenderer>();

        if (sprite)
        {
            sprite.sortingOrder = sortingOrder + 1;
        }

        if (Chat.instance)
        {
            Chat.instance.EnterFunction(name + ": Ha Aparecido!");
        }

    }

    #endregion

    #region Loop

    #region Attack

    protected void Attack()
    {

        if (!localPlayer || isAttacking)
        {
            return;
        }

        bool attackButtonPressed = CnInputManager.GetButtonDown("Attack Button");

        if (attackButtonPressed)
        {
            CastAttack();
        }

    }

    protected virtual void CastAttack()
    {
        CastLocalAttack(transform.position);
        SendAttackDataToServer();
    }

    public virtual void CastLocalAttack(Vector2 startPosition, Vector2 targetPosition)
    {
        AttackController attack = GetAttack();
        attack.Initialize(this, AttackController.MoveType.Target);
        attack.SetMovement(startPosition, targetPosition, attackSpeed);
    }

    public virtual void CastLocalAttack(Vector2 startPosition)
    {
        isAttacking = true;

        AttackController attack = GetAttack();
        attack.Initialize(this, AttackController.MoveType.Direction);
        attack.SetMovement(startPosition, directionX, attackSpeed);

        StartCoroutine(WaitAttacking());
        AnimateAttack();
    }

    #endregion

    #region Move

    protected void Move()
    {

        isGrounded = IsItGrounded();
        if (IsJumping(isGrounded))
        {
            speedY = maxYSpeed * directionY;
        }
        else
        {
            speedY = rb2d.velocity.y;
        }

        if (IsGoingRight())
        {
            // Si estaba yendo a la izquierda resetea la aceleración
            if (directionX == -1)
            {
                ResetDirectionX(1);
            }

            // sino acelera
            else if (acceleration < maxAcceleration)
            {
                Accelerate();
            }

            actualSpeed = maxXSpeed * acceleration;
            speedX = actualSpeed;
        }
        else if (IsGoingLeft())
        {

            // Si estaba yendo a la derecha resetea la aceleración
            if (directionX == 1)
            {
                ResetDirectionX(-1);
            }

            // sino acelera
            else if (acceleration < maxAcceleration)
            {
                Accelerate();
            }

            actualSpeed = maxXSpeed * acceleration;
            speedX = -actualSpeed;
        }
        else
        {
            speedX = 0f;
            acceleration = 0;
        }

        if (lastPosition != transform.position)
        {
            if (sceneAnimator)
            {
                sceneAnimator.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x), this.gameObject);
                sceneAnimator.SetBool("IsGrounded", isGrounded, this.gameObject);
            }
        }

        rb2d.velocity = new Vector2(speedX, speedY);
        lastPosition = transform.position;

    }

    #endregion

    #region Power

    public void UsePower()
    {

        if (localPlayer)
        {

            if (!levelManager.hpAndMp)
            {
                Debug.Log("Levelmanager HpAndMp is not set");
                return;
            }

            bool powerButtonPressed = CnInputManager.GetButtonDown("Power Button");
            float mpCurrentPercentage = levelManager.hpAndMp.mpCurrentPercentage;

            // Se acabó el maná
            if (mpCurrentPercentage <= 0f)
            {
                // Si no he avisado que se acabó el maná, aviso
                if (!mpDepleted)
                {
                    mpUpdateFrame = 0;
                    mpDepleted = true;

                    SetPowerState(false);
                    SendPowerDataToServer();
                }
            }

            // Hay maná
            else
            {
                // Reseteo la variable para avisar que el maná se acabó (volvió?)
                if (mpDepleted)
                {
                    mpDepleted = false;
                }

                // Toggle power button
                if (powerButtonPressed)
                {
                    SetPowerState(!isPowerOn);
                    SendPowerDataToServer();
                }

                if (isPowerOn)
                {

                    if (mpUpdateFrame == mpUpdateFrameRate)
                    {
                        levelManager.hpAndMp.ChangeMP(mpSpendRate); // Change local
                        SendMPDataToServer(); // Change remote
                        mpUpdateFrame = 0;
                    }

                    mpUpdateFrame++;

                }

            }
        }

    }

    #endregion

    #region Item

    public void UseItem()
    {
        if (localPlayer)
        {
            bool itemButtonPressed = CnInputManager.GetButtonDown("Bag Button");

            if (itemButtonPressed)
            {
                Inventory.instance.UseItem(this);
            }
        }
    }

    #endregion

    #endregion

    #region Callable

    public void HardReset()
    {
        StopMoving();
        ResetTransform();
        SetPowerState(false);
        ResetDamagingObjects();
        ResetChatZones();
        ResetDamagingTriggers();
        ResetDecisions();
        availablePowerable = null;
        gameObject.SetActive(false);
    }

    public void ResetChatZones()
    {
        if (availableChatZone != null)
        {
            ChatZone chatZoneOff = availableChatZone.GetComponent<ChatZone>();
            chatZoneOff.TurnChatZoneOff();
            availableChatZone = null; 
        }
    }

    public void ResetDamagingTriggers()
    {
        if (availableInstantiatorTrigger != null)
        {
            DamagingInstantiatorTrigger availableTrigger = availableInstantiatorTrigger.GetComponent<DamagingInstantiatorTrigger>();
            availableTrigger.ExitTrigger();
            availableChatZone = null;
        }
    }

    public void ResetDecisions()
    {
        if (decisionName != null)
        {
            DecisionSystem decisionOff = GameObject.Find(decisionName).GetComponent<DecisionSystem>();
            decisionOff.ResetDecision();
            decisionOff = null;
        }
    }

    public void ResetTransform()
    {
        transform.parent = null;
        SetPositiveGravity(true);
        IgnoreCollisionBetweenPlayers();
    }

    public void TakeDamage(int damage, Vector2 force)
    {
        if (isTakingDamage)
        {
            return;
        }

        isTakingDamage = true;

        if (force.x != 0 || force.y != 0)
        {
            rb2d.AddForce(force); // Take force local
            SendMessageToServer("PlayerTookDamage/" + playerId + "/" + force.x + "/" + force.y); // Take force remote
        }

        if (damage != 0)
        {

            // Always send negative values tu HPHUD
            if (damage > 0)
            {
                damage *= -1;
            }

            levelManager.hpAndMp.ChangeHP(damage); // Change local HP
            SendMessageToServer("ChangeHpHUDToRoom/" + damage); // Change remote HP

        }

        StartCoroutine(WaitTakingDamage());
        AnimateTakingDamage();

    }

    #endregion

    #endregion

    #region Utils

    // Set variables in their default state

    #region Initializers

    protected void IgnoreCollisionWithObjectsWhoHateMe()
    {
        IgnoreCollisionWithPlayers[] objectsWhoHateMe = FindObjectsOfType<IgnoreCollisionWithPlayers>();

        if (objectsWhoHateMe != null)
        {
            foreach (IgnoreCollisionWithPlayers objectWhoHatesMe in objectsWhoHateMe)
            {
                Debug.Log("the " + objectWhoHatesMe.gameObject.name + " will no longer strike me");
                BoxCollider2D colliderWhoHatesMe = objectWhoHatesMe.GetComponent<BoxCollider2D>();
                Physics2D.IgnoreCollision(colliderWhoHatesMe, gameObject.GetComponent<BoxCollider2D>(), true);

                Debug.Log("the " + objectWhoHatesMe.gameObject.name + " will no longer strike me for sure?");
            }

        }
    }
    public void IgnoreCollisionBetweenPlayers()
    {
        Collider2D collider = GetComponent<Collider2D>();

        GameObject player1 = GameObject.Find("Mage");
        GameObject player2 = GameObject.Find("Warrior");
        GameObject player3 = GameObject.Find("Engineer");
        Physics2D.IgnoreCollision(collider, player1.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(collider, player2.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(collider, player3.GetComponent<Collider2D>());
    }

    #endregion

    // Validate for player conditions

    #region Validations

    protected bool GameObjectIsPOI(GameObject other)
    {
        return other.GetComponent<PlannerPoi>();
    }

    #endregion

    // Set player data from other classes

    #region Remote Setters

    public virtual void StopMoving()
    {
        canMove = false;

        isTakingDamage = false;
        isAttacking = false;

        remoteJumping = false;
        remoteRight = false;
        remoteLeft = false;

        SendPlayerDataToServer();

        if (sceneAnimator)
        {
            sceneAnimator.SetFloat("Speed", 0, gameObject);
            sceneAnimator.SetBool("IsGrounded", true, gameObject);
            sceneAnimator.SetBool("Attacking", false, gameObject);
        }
    }

    public virtual void ResumeMoving()
    {
        canMove = true;
    }

    public void SetPositiveGravity(bool hasPositiveGravity)
    {
        if (hasPositiveGravity)
        {
            directionY = 1;
            rb2d.gravityScale = 2.5f;
            
        }
        else
        {
            directionY = -1;
            rb2d.gravityScale = -1.5f;
			cameraState = CameraState.Backwards;
			SetCamera();
        }

        transform.localScale = new Vector3(directionX, directionY, 1f);
    }

    private void SetCamera()
    {
        GameObject camera = GameObject.Find("MainCamera");
		if (camera != null) 
		{
			CameraController cameraController = camera.GetComponent<CameraController>();
			float cameraSize = cameraController.initialSize;
			cameraController.ChangeState(cameraState, cameraSize, transform.position.x, transform.position.y, true, false, false, 100, 70);
		}
    }

    protected void SetRespawn(Vector3 placeToGo)
    {
        if (!localPlayer)
        {
            return;
        }

        respawnPosition = placeToGo;

    }

    public void SetPowerState(bool active)
    {
        ToggleParticles(active);
        isPowerOn = active;
        if (availablePowerable != null)
        {
            TogglePowerable(active);
        }
    }

    protected void ResetDamagingObjects()
    {
        DamagingObject[] damagingObjects = FindObjectsOfType<DamagingObject>();
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        foreach (DamagingObject damaging in damagingObjects)
        {
            damaging.UpdateCollisionsWithPlayer(gameObject, false);
        }
        foreach (EnemyController enemy in enemies)
        {
            enemy.UpdateCollisionsWithPlayer(gameObject, false);
        }
    }

    protected void TogglePowerable(bool activate)
    {
        GameObject powerableGo = (availablePowerable);
        if (powerableGo)
        {
            PowerableObject powerable = powerableGo.GetComponent<PowerableObject>();

            for (int i = 0; i < powerable.powers.Length; i++)
            {
                if (powerable.PlayerActivatesPower(powerable.powers[i].caster, gameObject))
                {
                    if (powerable.IsPowered() && !activate)
                    {
                        powerable.DeactivatePower();
                    }
                    else if (!powerable.IsPowered() && activate)
                    {
                        powerable.ActivatePower(powerable.powers[i]);
                    }
                    break;
                }

            }
        }

    }

    public void SetDamageFromServer(Vector2 force)
    {
        rb2d.AddForce(force);
    }

    public void SetPlayerDataFromServer(float positionX, float positionY, int directionX, int directionY, float speedX, bool isGrounded, bool remoteJumping, bool remoteLeft, bool remoteRight)
    {

        this.remoteJumping = remoteJumping;
        this.remoteRight = remoteRight;
        this.remoteLeft = remoteLeft;
        this.isGrounded = isGrounded;
        this.directionX = directionX;
        this.directionY = directionY;
        this.speedX = speedX;

        if (sceneAnimator)
        {
            sceneAnimator.SetFloat("Speed", Mathf.Abs(speedX), this.gameObject);
            sceneAnimator.SetBool("IsGrounded", isGrounded, this.gameObject);
        }

        transform.position = new Vector3(positionX, positionY, transform.position.z);
        transform.localScale = new Vector3(directionX, directionY, 1f);
    }

    #endregion

    // Manage particles

    #region Particles

    protected void InitializeParticles()
    {
        ParticleSystem[] _particles = GetComponentsInChildren<ParticleSystem>();

        if (_particles == null || _particles.Length == 0)
        {
            return;
        }

        particles = new GameObject[_particles.Length];

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i] = _particles[i].gameObject;
        }

        ToggleParticles(false);

    }

    protected virtual void ToggleParticles(bool active)
    {

        if (particles != null && particles.Length > 0)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].SetActive(active);
            }
        }
    }

    #endregion

    // Manage animations

    #region Animations

    protected void AnimateAttack()
    {
        if (sceneAnimator && attackAnimName != null)
        {
            sceneAnimator.StartAnimation(attackAnimName, gameObject);
        }
    }

    protected void AnimateTakingDamage()
    {
        if (sceneAnimator)
        {
            sceneAnimator.StartAnimation("TakingDamage", this.gameObject);
        }
    }

    #endregion

    // Doh...

    #region Attacks

    protected virtual AttackController GetAttack()
    {
        throw new NotImplementedException("Every player must implement a GetAttack method");
    }

    #endregion

    #region Movement

    protected bool IsGoingRight()
    {
        if (localPlayer)
        {

            bool buttonRightPressed = CnInputManager.GetAxisRaw("Horizontal") == 1;

            // si el wn esta apuntando hacia arriba/abajo con menor inclinacion que hacia la derecha, start moving
            if (buttonRightPressed && !remoteRight)
            {
                remoteRight = true;
                remoteLeft = false;
                SendPlayerDataToServer();
            }

            // si no se esta apretando el joystick
            else if (!buttonRightPressed && remoteRight)
            {
                remoteRight = false;
                SendPlayerDataToServer();
            }

        }

        return remoteRight;

    }

    protected bool IsGoingLeft()
    {
        if (localPlayer)
        {

            bool buttonLeftPressed = CnInputManager.GetAxisRaw("Horizontal") == -1f;

            // si el wn esta apuntando hacia arriba/abajo con menor inclinacion que hacia la derecha, start moving
            if (buttonLeftPressed && !remoteLeft)
            {
                remoteLeft = true;
                remoteRight = false;
                SendPlayerDataToServer();
            }

            // si no se esta apretando el joystick
            else if (!buttonLeftPressed && remoteLeft)
            {
                remoteLeft = false;
                SendPlayerDataToServer();
            }

        }

        return remoteLeft;
    }

    public bool IsGoingUp()
    {
        return false;
    }

    protected bool IsItGrounded()
    {
        // El radio del groundChecker debe ser menor a la medida del collider del player/2 para que no haga contactos laterales.
        groundCheckRadius = GetComponent<Collider2D>().bounds.extents.x;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    protected virtual bool IsJumping(bool isGrounded)
    {
        if (localPlayer)
        {
            bool pressedJump = CnInputManager.GetButtonDown("Jump Button");
            bool isJumping = pressedJump && isGrounded;

            if (isJumping && !remoteJumping)
            {
                remoteJumping = true;
                SendPlayerDataToServer();
            }
            else if (!isJumping && remoteJumping)
            {
                remoteJumping = false;
                SendPlayerDataToServer();
            }

        }

        return remoteJumping;

    }

    protected void ResetDirectionX(int newDirectionX)
    {
        transform.localScale = new Vector3(newDirectionX, directionY, 1f);
        directionX = newDirectionX;
        acceleration = .1f;
    }

    protected void Accelerate()
    {
        if (canAccelerate)
        {
            acceleration += .1f;
            canAccelerate = false;
        }
        else
        {
            canAccelerate = true;
        }

    }

    #endregion

    #endregion

    #region Events

    protected void OnTriggerStay2D(Collider2D other)
    {
        if (GameObjectIsPOI(other.gameObject))
        {
            PlannerPoi newPoi = other.GetComponent<PlannerPoi>();
            if (!playerObj.playerAt.name.Equals(newPoi.name))
            {
                Debug.Log("Change OK: " + newPoi.name);
                playerObj.playerAt = newPoi;
                playerObj.luring = false;
                if (newPoi.araña != null && this.playerId == 0)
                {
                    playerObj.luring = true;
                    newPoi.araña.blocked = false;
                    newPoi.araña.open = true;
                }
                Planner planner = FindObjectOfType<Planner>();
                planner.Monitor();
            }
        }
    }

    #endregion

    #region Messaging

    public void SendPlayerDataToServer()
    {
        if (!localPlayer)
        {
            return;
        }

        string message = "PlayerChangePosition/" +
                               playerId + "/" +
                               transform.position.x + "/" +
                               transform.position.y + "/" +
                               directionX + "/" +
                               directionY + "/" +
                               Mathf.Abs(rb2d.velocity.x) + "/" +
                               isGrounded + "/" +
                               remoteJumping + "/" +
                               remoteLeft + "/" +
                               remoteRight;

        SendMessageToServer(message);
    }

    protected virtual void SendAttackDataToServer()
    {
        string message = "PlayerAttack/" + playerId + "/" + transform.position.x + "/" + transform.position.y;
        SendMessageToServer(message);
    }

    protected void SendPowerDataToServer()
    {
        string message = "PlayerPower/" + playerId + "/" + isPowerOn;
        SendMessageToServer(message);
    }

    public void SendMPDataToServer()
    {
        SendMessageToServer("ChangeMpHUDToRoom/" + mpSpendRate);
    }

    protected void SendMessageToServer(string message)
    {
        if (Client.instance)
        {
            Client.instance.SendMessageToServer(message, false);
        }
    }

    #endregion

    #region Coroutines


    public IEnumerator WaitTakingDamage()
    {
        yield return new WaitForSeconds(takeDamageRate);
        isTakingDamage = false;
    }

    #endregion

}
using UnityEngine;
using System.Collections.Generic;

/** 
 *  This class is for static damaging objects such as a lava
 *  in order to work this objects must have a trigger collider
 */
public class DamagingObject : MonoBehaviour
{
    #region Attributes

    protected Dictionary<string, bool> ignoresCollisions;

    public Vector2 force;
    public int damage;


    #endregion

    #region Start & Update

    protected virtual void Start()
    {
        ignoresCollisions = new Dictionary<string, bool> { { "Mage", false }, { "Warrior", false }, { "Engineer", false } };
    }

    // Update is called once per frame
    void Update()
    {
    }

    #endregion

    #region Common

    protected virtual void DealDamage(GameObject player)
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        PlayerController playerController = player.GetComponent<PlayerController>();
        MageController mage = levelManager.GetMage();

        Vector2 playerPosition = player.transform.position;
        Vector2 attackForce = force;

        // Only hit local players
        if (!playerController.localPlayer)
        {
            return;
        }

        // Don't hit protected players
        if (mage.ProtectedByShield(player))
        {
            if (!ignoresCollisions[player.name])
            {
                UpdateCollisionsWithPlayer(player, true);
            }
            return;
        }
        else
        {
            if (ignoresCollisions[player.name])
            {
                UpdateCollisionsWithPlayer(player, false);
            }
        }

        // If player is at the left side of the enemy push it to the left
        if (playerPosition.x < transform.position.x)
        {
            attackForce.x *= -1;
        }

        playerController.TakeDamage(damage, attackForce);

    }

    #endregion

    #region Messaging

    private void SendIgnoreCollisionDataToServer(GameObject player, bool collision)
    {
        SendMessageToServer("IgnoreCollisionBetweenObjects/" + collision + "/" + player.name + "/" + gameObject.name, true);
    }

    protected virtual void SendMessageToServer(string message, bool secure)
    {
        if (Client.instance)
        {
            Client.instance.SendMessageToServer(message, secure);
        }
    }

    #endregion

    #region Events

    // Attack those who collide with me
    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            DealDamage(other.gameObject);
        }

        if (GameObjectIsEnemy(other.gameObject))
        {
            if (CheckIfImWarriored(gameObject))
            {
                KillEnemy(other.gameObject);
            }

            if (CheckIfImMaged())
            {
                GetThisEnemyMaged(other.gameObject);
            }
        }

        if (GameObjectIsDestroyable(other.gameObject))
        {
            if (CheckIfImWarriored(gameObject))
            {
                DestroyableObject destroyable = other.gameObject.GetComponent<DestroyableObject>();
                destroyable.DestroyMe(true);
            }
        }
    }

    protected void OnTriggerStay2D(Collider2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            DealDamage(other.gameObject);
        }
    }

    // Attack those who enter the alert zone
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            DealDamage(other.gameObject);
        }
    }

    #endregion

    #region Utils
    protected void GetThisEnemyMaged(GameObject enemy)
    {
        EnemyController eC = enemy.gameObject.GetComponent<EnemyController>();
        eC.GetThisEnemyMaged();
    }

    protected void KillEnemy(GameObject enemy)
    {
        EnemyController eC = enemy.gameObject.GetComponent<EnemyController>();
        eC.TakeDamage(150);
        Destroy(gameObject, .2f);
    }


    protected bool CheckIfImMaged()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager.GetMage())
        {
            if (levelManager.GetMage().ProtectedByShield(gameObject))
            {
                return true;
            }
        }
        else
        {
            Debug.LogError("It Seems there is no Mage");
        }
        return false;
    }


    protected bool CheckIfImWarriored(GameObject enemy)
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager.GetWarrior())
        {
            if (levelManager.GetWarrior().IsWarriored(gameObject))
            {
                return true;
            }
        }
        else
        {
            Debug.LogError("It Seems there is no warrior");
        }
        return false;
    }

    protected bool GameObjectIsDestroyable(GameObject other)
    {
        if (other.GetComponent<DestroyableObject>())
        {
            return true;
        }
        return false;
    }

    protected bool GameObjectIsEnemy(GameObject other)
    {
        if (other.GetComponent<EnemyController>())
        {
            return true;
        }
        return false;
    }

    protected bool GameObjectIsPlayer(GameObject other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        return playerController && playerController.localPlayer;
    }

    public virtual void UpdateCollisionsWithPlayer(GameObject player, bool ignores)
    {
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            if (!collider.isTrigger)
            {
                Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), ignores);
            }
        }

        ignoresCollisions[player.name] = ignores;
        SendIgnoreCollisionDataToServer(player, ignores);

    }

    #endregion

}

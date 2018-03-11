using UnityEngine;
using System.Collections;

public class MovableObject : MonoBehaviour
{

    #region Attributes

    public PlannerObstacle obstacleObj = null;

    public GameObject openningTrigger; // The trigger that makes dissapear the object
    public string openedPrefab; // How it looks when its opened
    public GameObject[] particles;

    protected bool imMoving;
    protected int poweredFrameCount;
    protected int shutdownFrames;
    protected SceneAnimator sceneAnimator;
    protected Rigidbody2D rgbd;

    #endregion

    #region Start & Update


    // Use this for initialization
    protected virtual void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        sceneAnimator = FindObjectOfType<SceneAnimator>();
        InitializeParticles();
    }

    protected virtual void Update()
    {
        if (imMoving)
        {
            if (poweredFrameCount++ == shutdownFrames)
            {
                rgbd.constraints = RigidbodyConstraints2D.FreezePositionX;
                sceneAnimator.SetBool("Moving", false, gameObject);
                imMoving = false; 
            }
        }
    }

    #endregion

    #region Common

    public virtual void MoveMe(Vector2 force, bool movedFromLocal)
    {
        if (rgbd)
        {
            rgbd.constraints = RigidbodyConstraints2D.None;
            imMoving = true;
            rgbd.AddForce(force);

            if (movedFromLocal)
            {
                SendMovableDataToServer(force);
            }

            if (!sceneAnimator)
            {
                Debug.Log("AnimatorControl not found in " + name);
                return;
            }
            sceneAnimator.SetBool("Moving", true, gameObject);
        }
    }


    protected void TransitionToOpened(GameObject trigger)
    {
        if (obstacleObj != null)
        {
            obstacleObj.blocked = false;
            obstacleObj.open = true;
        }

        if (openedPrefab != null)
        {
            SendMessageToServer("InstantiateObject/Prefabs/" + openedPrefab, false);
        }

        SendMessageToServer("DestroyObject/" + name, false);
    }

    #endregion

    #region Events

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger)
        {
            if (openningTrigger != null)
            {
                if (TriggerIsOpener(other.gameObject))
                {
                    TransitionToOpened(other.gameObject);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Prevents weird collisions with other game objects.
        if (!collision.gameObject || !collision.gameObject.GetComponent<Rigidbody2D>())
        {
            return;
        }

        // Counter the force of every other game object.
        if (!GameObjectIsPunch(collision.gameObject))
        {
            Rigidbody2D otherRb2d = collision.gameObject.GetComponent<Rigidbody2D>();
            rgbd.AddForce(-otherRb2d.velocity);
        }

    }

    #endregion

    #region Utils

    protected bool TriggerIsOpener(GameObject trigger)
    {
        return trigger && trigger.Equals(openningTrigger);
    }

    protected bool GameObjectIsPunch(GameObject other)
    {
        return other.GetComponent<PunchController>();
    }
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

        ToggleParticles(particles, false);

    }

    public void ToggleParticles(GameObject[] particles, bool activate)
    {
        if (particles != null && particles.Length > 0)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].SetActive(activate);
            }
        }
    }

    #endregion

    #region Messaging

    protected void SendMovableDataToServer(Vector2 force)
    {

        SendMessageToServer("ObjectMoved/" +
                name + "/" +
                force.x + "/" +
                force.y,
                false);

    }

    private void SendMessageToServer(string message, bool secure)
    {
        if (Client.instance)
        {
            Client.instance.SendMessageToServer(message, secure);
        }
    }

    #endregion

}
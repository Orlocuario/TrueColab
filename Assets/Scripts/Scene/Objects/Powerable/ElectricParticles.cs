using UnityEngine;

public class ElectricParticles : MonoBehaviour
{

    #region Events

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.SetPositiveGravity(false);
        }

        if (other.GetComponent<MovableObject>())
        {
            other.GetComponent<Rigidbody2D>().gravityScale *= -1;
            MovableObject movable = other.GetComponent<MovableObject>();
            GameObject[] particles = movable.particles;
            movable.ToggleParticles(particles, true);
        }

    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.SetPositiveGravity(true);
        }

        if (other.GetComponent<MovableObject>())
        {
            other.GetComponent<Rigidbody2D>().gravityScale *= -1;
            MovableObject movable = other.GetComponent<MovableObject>();
            GameObject[] particles = movable.particles;
            movable.ToggleParticles(particles, false);
        }
    }
    #endregion
}



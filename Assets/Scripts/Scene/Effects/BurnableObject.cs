using UnityEngine;

public class BurnableObject : PowerableObject
{
    #region Attributes

    private GameObject[] particles;

    #endregion

    #region Events

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (powered)
        {
            if (collision.gameObject.tag.Equals("BurnableBranches1"))
            {
                ParticleSystem flameParticles = collision.gameObject.GetComponentInChildren<ParticleSystem>();
                if (flameParticles)
                {
                    flameParticles.Play();
                    Destroy(collision.gameObject, 1f);
                }
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (powered)
        {
            if (collision.gameObject.tag.Equals("BurnableBranches1"))
            {
                ParticleSystem flameParticles = collision.gameObject.GetComponentInChildren<ParticleSystem>();
                if (flameParticles)
                {
                    flameParticles.Play();
                    Destroy(collision.gameObject, 1f);
                }
            }
        }
    }

    #endregion

    #region Utils

    protected override void DoYourPowerableThing()
    {
        //
    }

    protected override void UndoYourPowerableThing()
    {
        //   
    }
    #endregion

}

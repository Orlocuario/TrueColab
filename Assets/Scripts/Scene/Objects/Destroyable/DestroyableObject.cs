using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{

    #region Attributes

    public float destroyDelayTime;
    public bool reinforced;
    public bool mustReturn;
    public GameObject particle;

    #endregion

    #region Start

    protected virtual void Start()
    {
        destroyDelayTime = .4f;

        if (particle != null)
        {
            particle.SetActive(false);
        }
    }

    #endregion

    #region Common

    public virtual void DestroyMe(bool destroyedFromLocal)
    {
        if (particle != null)
        {
            particle.SetActive(true);
        }

        if (destroyedFromLocal)
        {
            SendDestroyDataToServer();
        }

        if (gameObject.GetComponent<ParticleSystem>())
        {
            gameObject.GetComponent<ParticleSystem>().Play();
        }

        if (mustReturn)
        {
            DoFalseDeactivation();
        }
        else
        {
            Destroy(gameObject, destroyDelayTime);
        }
    }

    private void DoFalseDeactivation()
    {
        DeactivateColliders();
        DeactivateSpriteRenderer();
    }
    private void DeactivateSpriteRenderer()
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.enabled = false;
    }

    private void DeactivateColliders()
    {
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }

    #endregion

    #region Messaging

    protected void SendDestroyDataToServer()
    {
        if (Client.instance)
        {
            Client.instance.SendMessageToServer("ObjectDestroyed/" + name + "/", true);
        }
    }

    #endregion

}
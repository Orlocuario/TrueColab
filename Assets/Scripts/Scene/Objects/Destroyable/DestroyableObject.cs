using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{

    #region Attributes

    public float destroyDelayTime;
    public bool reinforced;
    public bool mustReturn;
    public float timeToReactivate;
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
            StartCoroutine(DoFalseDeactivation());
            StartCoroutine(Reactivate());
        }
        else
        {
            Destroy(gameObject, destroyDelayTime);
        }
    }

    private IEnumerator DoFalseDeactivation()
    {
        yield return new WaitForSeconds(.1f);
        ToggleColliders(false);
        ToggleSpriteRenderer(false);
    }

    private void ToggleSpriteRenderer(bool active)
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.enabled = active;
    }

    private void ToggleColliders(bool active)
    {
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = active;
        }
    }

    private IEnumerator Reactivate()
    {
        yield return new WaitForSeconds(timeToReactivate);
        ToggleSpriteRenderer(true);
        ToggleColliders(true);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{

    #region Attributes

    public float destroyDelayTime;
    public bool reinforced;
    public bool mustReturn;
    private float timeToReactivate;
    public GameObject particle;

    #endregion

    #region Start

    protected virtual void Start()
    {
        timeToReactivate = 7f;
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
    
        if (gameObject.GetComponent<ParticleSystem>())
        {
            gameObject.GetComponent<ParticleSystem>().Play();
        }

        if (mustReturn)
        {
            StartCoroutine(DoFalseDeactivation());
            if (particle != null)
            {
                StartCoroutine(StopParticles());
            }
            StartCoroutine(ReactivateSprite());
            StartCoroutine(ReactivateColliders());  
        }

        else
        {
            if (destroyedFromLocal)
            {
                SendDestroyDataToServer();
            }

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
        for(int i = 0; i<gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).childCount > 0)
            {
                for(int j = 0; j< gameObject.transform.GetChild(i).childCount; j++)
                {
                    if (gameObject.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>())
                    {
                        SpriteRenderer renderer = gameObject.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>();
                        renderer.enabled = active;
                    }
                }
            }
            if (gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>())
            {
                SpriteRenderer secondLevelRenderer = gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                secondLevelRenderer.enabled = active;
            }
        }

    }

    private void ToggleColliders(bool active)
    {
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = active;
        }
    }
    private IEnumerator StopParticles()
    {
        yield return new WaitForSeconds(.6f);
        particle.SetActive(false);
    }

    private IEnumerator ReactivateSprite()
    {
        yield return new WaitForSeconds(timeToReactivate);
        ToggleSpriteRenderer(true);
    }

    private IEnumerator ReactivateColliders()
    {
        yield return new WaitForSeconds(.8f);
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
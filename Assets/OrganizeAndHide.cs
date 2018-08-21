using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganizeAndHide : MonoBehaviour {

    public Vector2[] vectorsToGo;
    public GameObject[] objectsToHold;
    public float timeToWait;


	// Use this for initialization
	void Start ()
    {
        CheckParameters();
        StartCoroutine(GiveSomeTime());
	}
	
    private void SendObjectsToItsPlace()
    {
        for (int i = 0; i<objectsToHold.Length; i++)
        {
            objectsToHold[i].transform.position = vectorsToGo[i];
        }
    }

    private void HideObjects()
    {
        for (int i = 0; i < objectsToHold.Length; i++)
        {
            objectsToHold[i].SetActive(false);
        }
    }

    public void ShowObjects()
    {
        for (int i = 0; i < objectsToHold.Length; i++)
        {
            objectsToHold[i].SetActive(true);
        }
    }

    private void CheckParameters()
    {
        if (vectorsToGo.Length == 0)
        {
            Debug.LogError("The Organizer and Hider named: " + gameObject.name + " doesnt have Vectors To Organize Objects with");
        }

        if (objectsToHold.Length == 0)
        {
            Debug.LogError("The Organizer and Hider named: " + gameObject.name + " doesnt have Objects to Hold");

        }

        if (timeToWait == 0)
        {
            Debug.LogError("The Organizer and Hider named: " + gameObject.name + " doesnt have Time to wait");

        }

    }

    private IEnumerator GiveSomeTime()
    {
        yield return new WaitForSeconds(timeToWait);
        SendObjectsToItsPlace();
        HideObjects();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MovableTriggerInstantiator : MonoBehaviour
{

    [Serializable]

    public struct ObjectToInstantiate
    {
        public Vector2 position;
        public string name;
    }

    public ObjectToInstantiate[] instantiateObjects;
    public GameObject objectNeeded;

    private LevelManager levelManager;

    void Start()
    {

        levelManager = FindObjectOfType<LevelManager>();
        if (instantiateObjects.Length == 0)
        {
            Debug.LogError(gameObject.name + " doesnt have instantiate values");
        }
    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == objectNeeded)
        {
            ActivateTrigger(other.gameObject);
        }
    }

    public void ActivateTrigger(GameObject other)
    {
        SendMessageToServer("ObjectDestroyed" + "/" + objectNeeded.gameObject.name, true);
        SendMessageToServer("ActivateTrigger" + "/" + gameObject.name, true);
        InstantiateStuff();
    }

    public void HandleTriggerReachedByMovable()
    {
        InstantiateStuff();
    }

    private void InstantiateStuff()
    {
        Destroy(objectNeeded);
        foreach (ObjectToInstantiate instObject in instantiateObjects)
        {
            levelManager.InstantiatePrefab(instObject.name, instObject.position);
        }
    }

    private void SendMessageToServer(string message, bool secure)
    {
        if (Client.instance)
        {
            Client.instance.SendMessageToServer(message, secure);
        }
    }
}

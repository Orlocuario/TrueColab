using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectNameGiver : MonoBehaviour {

    public int lastId;

    void Start()
    {
        GiveNames();
    }

    private void GiveNames()
    {
        lastId = 0;
        MovingObject[] movingObjects = FindObjectsOfType<MovingObject>();

        for (int i = 0; i < movingObjects.Length; i++)
        {
            GameObject movingObject = movingObjects[i].gameObject;
            string name = movingObject.name;
            name = name + i.ToString();
            movingObject.name = name;
            lastId = i;
        }
    }

    public void GiveNameFromInstantiation (GameObject incomingObj)
    {
        string objectName = incomingObj.name;
        int i = lastId; 
        objectName = objectName + i.ToString();
        incomingObj.name = name;
        lastId++;
    }
}

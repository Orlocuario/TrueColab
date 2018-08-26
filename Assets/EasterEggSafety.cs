using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggSafety : MonoBehaviour {

    private int timesHit;
    private int oblivionRate;
    private int currentOblivion;

    [System.Serializable]

    public struct ObjectAndVector
    {
        public string prefabName;
        public Vector2 position;
    }

    public ObjectAndVector[] objectsToInstantiate;

    // Use this for initialization
    void Start () {
        oblivionRate = 120;
        currentOblivion = 0;
	}
	
	// Update is called once per frame
	void Update () {
        currentOblivion++;
        if (currentOblivion == oblivionRate)
        {
            timesHit = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.GetComponent<MageController>().localPlayer)
        {
            PlayerController pController = collider.gameObject.GetComponent<PlayerController>();
            if(pController.GetType().Equals(new MageController().GetType()))
            {
                currentOblivion = 0;
                timesHit++;
                Debug.Log("TimesHit: " + timesHit);
                if (timesHit ==25)
                {
                    for (int i = 0; i < objectsToInstantiate.Length; i++) 
                    {
                        ObjectAndVector oVector = objectsToInstantiate[i];
                        float posX = oVector.position.x;
                        float posY = oVector.position.y;

                        SendMessageToServer("InstantiateThisObjects" + "/" + 
                                    objectsToInstantiate[i].prefabName + "/" + posX.ToString() + "/" + posY.ToString(), true);
                    }
                }
            }
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

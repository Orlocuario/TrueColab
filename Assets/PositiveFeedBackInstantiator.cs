using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositiveFeedBackInstantiator : MonoBehaviour
{

    private SoundManager soundManager;
    private int playersArrived;
    private bool beenUsed;

    public Vector2[] instantiationVectors;
    public string[] prefabNames;
    public string[] playersNeeded;


    private void Start()
    {
        playersArrived = 0;
    }

    public bool BeenUsed ()
    {
        return beenUsed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            for (int i = 0; i < playersNeeded.Length; i++)
            {
                if (playersNeeded[i] != null)
                {
                    if (playersNeeded[i] == collision.gameObject.name)
                    {
                        playersArrived++;
                        playersNeeded[i] = null;

                        if (playersArrived == playersNeeded.Length)
                        {
                            if (!beenUsed)
                            {
                                for (int j = 0; j < prefabNames.Length; j++)
                                {
                                    LevelManager levelManager = FindObjectOfType<LevelManager>();
                                    levelManager.InstantiatePrefab(prefabNames[j], instantiationVectors[j]);
                                }

                                beenUsed = true;
                            }
                        }
                    }
                }
            }
        }
    }
}

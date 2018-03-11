using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositiveFeedBackInstantiator : MonoBehaviour
{

    public Vector2[] instantiationVectors;
    public string[] prefabNames;
    private SoundManager soundManager;
    public string[] playersNeeded;
    private int playersArrived;


    private void Start()
    {
        playersArrived = 0;
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
                            for (int j = 0; j < prefabNames.Length; j++)
                            {
                                LevelManager levelManager = FindObjectOfType<LevelManager>();
                                levelManager.InstantiatePrefab(prefabNames[j], instantiationVectors[j]);
                            }
                        }

                    }
                }
            }
        }
    }
}

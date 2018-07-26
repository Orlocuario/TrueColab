using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorTeleportAssigner : MonoBehaviour {


    public struct VectorizedTeleporter
    {
        public Transform destinyObject;
        public PlayerTeleporter teleporter;
    }

    public VectorizedTeleporter[] teleporters;


	// Use this for initialization
	void Start () {

        for (int i = 0; i < teleporters.Length; i++)
        {
            
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

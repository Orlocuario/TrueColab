using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Igor : MonoBehaviour {

    public string mensajeAmiAmo;
	// Use this for initialization
	void Start () {
        if(mensajeAmiAmo == null)
        {
            Debug.LogError("Eres un estúpido igor");
        }
        StartCoroutine(WaitToNameMyMastar());
	}
	
    private IEnumerator WaitToNameMyMastar()
    {
        yield return new WaitForSeconds(2f);

        Debug.Log(mensajeAmiAmo);
    }
}

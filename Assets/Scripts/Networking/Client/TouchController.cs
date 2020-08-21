using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Hola Esto es un comentario

public class TouchController : MonoBehaviour {

    [SerializeField]
    string textObjectName;
    #region Events

    public void OnClick()
    {
        GameObject textObject = GameObject.Find(textObjectName);
        textObject.GetComponent<Text>().text = "Conectando";               //Change 

        GameObject client = GameObject.Find("ClientObject");
        ClientNetworkDiscovery listen = client.GetComponent<ClientNetworkDiscovery>();
        listen.InitializeListening();
    }

    #endregion
}

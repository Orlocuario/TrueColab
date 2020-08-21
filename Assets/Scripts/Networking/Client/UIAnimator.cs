using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    Image iComponent;
    int imageFrames;
    float frameRate;


    private void Awake()
    {
        iComponent = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}


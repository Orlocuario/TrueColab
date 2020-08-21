using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
    Image iComponent;

    [SerializeField]
    string animName;
    [SerializeField]
    int frameRate;
    [SerializeField]
    Sprite[] sprites;

    [SerializeField]
    float swing;

    int actualFrame = 0;
    int actualSprite = 0;
    bool canAnimate = false;

    private void Awake()
    {
        CorrectInitialCheck();
        iComponent = GetComponent<Image>();
    }

    private void CorrectInitialCheck()
    {
        if (animName == null)
        {
            Debug.LogError(gameObject.name + " has an animation with no name assigned");
        }
        if (frameRate == 0)
        {
            Debug.LogError(gameObject.name + " has no frame rate assigned");
        }
        if (sprites.Length == 0)
        {
            Debug.LogError(gameObject.name + " has no sprites assigned");
        }
        else
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i] == null)
                {
                    Debug.LogError("GameObject: " + gameObject.name + ". Sprite number " + i + " in the UI animation is emkpty");
                }
            }
        }
    }

    private void Start()
    {
        StartCoroutine(WaitForSwing());
    }
    private IEnumerator WaitForSwing()
    {
        yield return new WaitForSeconds(swing);
        canAnimate = true;
    }

    private void Update()
    {
        if (canAnimate)
        {
            Animate();
        }
    }

    void Animate()
    {
        actualFrame++;
        if (actualFrame == frameRate)
        {
            actualSprite++;
            if (actualSprite >= sprites.Length)
            {
                actualSprite = 0;
            }
            iComponent.sprite = sprites[actualSprite];
            actualFrame = 0;
        }
    }

    public UIAnimation(string _aName, int _frameRate, Sprite[] _sprites)
    {
        string aName = _aName;
        frameRate = _frameRate;
        sprites = new Sprite[_sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = _sprites[i];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    private SpriteRenderer sprite;


    bool setFade;
    bool transitionActive;
    float transitionTime;


    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (setFade)
        {
            if (transitionActive)
            {
                transitionTime += Time.deltaTime;

                sprite.color = sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.Lerp(sprite.color.a, 0.5f, transitionTime / 0.2f));

                if (transitionTime >= 0.2f)
                {
                    sprite.color = sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
                    transitionActive = false;
                    setFade = false;
                    transitionTime = 0;
                }
            }
        }

        else
        {
            if (transitionActive)
            {
                Debug.Log("Returning from fade");
                transitionTime += Time.deltaTime;

                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.Lerp(0.5f, 1, transitionTime / 0.2f));

                if (transitionTime >= 0.2f)
                {
                    sprite.color = sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
                    transitionActive = false;
                    transitionTime = 0;
                }
            }
        }
    }


    public void SetFadeValue(bool value)
    {
        setFade = value;
        transitionActive = true;
        transitionTime = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    public int x, y;

    public int animationStatus = 0;
    float animationElapsedTime, animationProgress, animationDuration, defaultAnimDuration = 0.1f;

    Color animColor = new Color(255, 0, 0, 127);

    Color startingAnimColor;

    void Start()
    {
        startingAnimColor = gameObject.GetComponent<MeshRenderer>().material.color;
    }

    public void UpdateName()
    {
        transform.name = x + ", " + y;
    }

    public void OnClick()
    {
        animationStatus = 1;
        animationElapsedTime = 0;
        animationProgress = 0;
        animationDuration = defaultAnimDuration;
    }

    void FixedUpdate()
    {
        if(animationStatus != 0)
        {
            animationElapsedTime += Time.deltaTime;
            animationProgress = animationElapsedTime / animationDuration;
            if(animationProgress >= 1)
            {
                if(animationStatus == 1)
                {
                    animationStatus = 0;
                    gameObject.GetComponent<MeshRenderer>().material.color = startingAnimColor;
                }
            }
            else if(animationStatus == 1) gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0.25f * Mathf.Sin(Mathf.PI * animationProgress));
        }
    }
}

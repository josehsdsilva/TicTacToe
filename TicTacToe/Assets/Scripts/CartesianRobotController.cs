using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartesianRobotController : MonoBehaviour
{
    public GameObject tip, top;

    float movingDistance = 1.05f;

    int x = 1, y = 1, movX, movY;
    Vector3 previousTipPosition, previousTopPosition;
    float speed = 1;

    int animationStatus = 0;
    float animationElapsedTime = 0.0f;
    float animationProgress = 0.0f;
    float animationDuration = 0.0f;

    public void Move(int _x, int _y)
    {
        previousTipPosition = tip.transform.position;
        previousTopPosition = top.transform.position;
        movX = _x - x;
        movY = _y - y;
        if(movX == 0) MoveY();
        else MoveX();
    }

    void MoveX()
    {
        animationStatus = 1;
        animationElapsedTime = 0;
        animationProgress = 0;
        animationDuration = speed * Mathf.Abs(movX);
    }

    void MoveY()
    {
        animationStatus = 2;
        animationElapsedTime = 0;
        animationProgress = 0;
        animationDuration = speed * Mathf.Abs(movY);
    }

    void FixedUpdate()
    {
        if(animationStatus != 0)
        {
            animationElapsedTime += Time.deltaTime;
            animationProgress = animationElapsedTime / animationDuration;
            if(animationProgress >= 1)
            {
                animationProgress = 1;
                if(animationStatus == 1)
                {
                    x += movX;
                    if(movY == 0)
                    {
                        EndOfTurn();
                    }
                    else
                    {
                        MoveY();
                    }
                }
                else if(animationStatus == 2)
                {
                    y += movY;
                    EndOfTurn();
                }
            }
            if(animationStatus == 1)
            {
                tip.transform.position = previousTipPosition + new Vector3(movX * movingDistance, 0, 0) * animationProgress;
            }
            else if(animationStatus == 2)
            {
                top.transform.position = previousTopPosition + new Vector3(0, 0, -movY * movingDistance) * animationProgress;
            }
        }
    }

    void EndOfTurn()
    {
        animationStatus = 0;
        Global.instance.EndOfTurn(x, y);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartesianRobotController : MonoBehaviour
{
    public GameObject arm, top, balls, ballDropPoint;

    int spaceX, spaceY;
    float x = 1, z = 1, movX, movZ;
    Vector3 previousArmPosition, previousTopPosition;

    // Animation Control
    [Range(0.1f, 1f)]
    [SerializeField] float defaultAnimDuration = 0.1f;

    [HideInInspector] public int animationStatus = 0;
    float animationElapsedTime = 0.0f;
    float animationProgress = 0.0f;
    float animationDuration = 0.0f;
    int movementType;
    float scaleFactor = 5.4f;

    // Balls Information
    Vector3[,] ballsStartPosition;
    int[] ballUsed;
    int selectedBall;

    void Start()
    {
        ballUsed = new int[2];
        x = transform.position.x;
        z = transform.position.z;
        ballsStartPosition = new Vector3[2, 5];
        for (int player = 0; player < 2; player++)
        {
            for (int ball = 0; ball < 5; ball++)
            {
                ballsStartPosition[player, ball] = balls.transform.GetChild(player * 5 + ball).transform.position;
            }
        }
        GetBall();
    }

    void GetBall()
    {
        movementType = 1;
        movX = ballsStartPosition[Global.instance.turn-1, ballUsed[Global.instance.turn-1]].x - x;
        movZ = ballsStartPosition[Global.instance.turn-1, ballUsed[Global.instance.turn-1]].z - z;
        MoveZ();
    }

    public void Move(int _x, int _y, Vector3 pos)
    {
        movementType = 2;
        spaceX = _x;
        spaceY = _y;
        movX = pos.x - x;
        movZ = pos.z - z;
        MoveZ();
    }

    void MoveZ()
    {
        previousTopPosition = top.transform.position;
        animationStatus = 2;
        animationElapsedTime = 0;
        animationProgress = 0;
        animationDuration = defaultAnimDuration * Mathf.Abs(movZ);
    }

    void MoveX()
    {
        previousArmPosition = arm.transform.position;
        animationStatus = 3;
        animationElapsedTime = 0;
        animationProgress = 0;
        animationDuration = defaultAnimDuration * Mathf.Abs(movX);
    }

    void ScaleUpArm()
    {
        animationStatus = 4;
        animationElapsedTime = 0;
        animationProgress = 0;
        animationDuration = defaultAnimDuration;
    }

    void ScaleDownArm()
    {
        animationStatus = 5;
        animationElapsedTime = 0;
        animationProgress = 0;
        animationDuration = defaultAnimDuration;
    }

    void CatchBall()
    {
        balls.transform.GetChild(selectedBall).gameObject.SetActive(false);
        ScaleDownArm();
    }

    void DropBall()
    {
        balls.transform.GetChild(selectedBall).transform.position = ballDropPoint.transform.position;
        balls.transform.GetChild(selectedBall).gameObject.SetActive(true);
        ballUsed[Global.instance.turn-1]++;
        ScaleDownArm();
    }

    void ResetNextBall()
    {
        for (int player = 0; player < 2; player++)
        {
            for (int ball = 0; ball < 4; ball++)
            {
                if(ballsStartPosition[player, ball] != balls.transform.GetChild(player * 5 + ball).transform.position)
                {
                    movementType = 3;
                    selectedBall = (Global.instance.turn-1) * 5 + ballUsed[Global.instance.turn-1];
                    movX = balls.transform.GetChild(player * 5 + ball).transform.position.x - x;
                    movZ = balls.transform.GetChild(player * 5 + ball).transform.position.z - z;
                    MoveZ();
                    return;
                }
            }
        }

        // Reset Game
        ballUsed = new int[2];
    }

    void PlaceOnDefaultPosition()
    {
        movementType = 4;
        movX = ballsStartPosition[Global.instance.turn-1, ballUsed[Global.instance.turn-1]].x - x;
        movZ = ballsStartPosition[Global.instance.turn-1, ballUsed[Global.instance.turn-1]].z - z;
        MoveZ();
    }

    void FixedUpdate()
    {
        if( Input.GetMouseButtonDown(1) )
        {
            ResetNextBall();
        }

        if(animationStatus != 0)
        {
            animationElapsedTime += Time.deltaTime;
            animationProgress = animationElapsedTime / animationDuration;
            if(animationProgress >= 1)
            {
                animationProgress = 1;
                if(animationStatus == 2)
                {
                    top.transform.position = previousTopPosition + new Vector3(0, 0, movZ);
                    z += movZ;
                    MoveX();
                }
                else if(animationStatus == 3)
                {
                    arm.transform.position = previousArmPosition + new Vector3(movX, 0, 0);
                    x += movX;
                    if(movementType != 0) ScaleUpArm();
                    else animationStatus = 0;
                }
                else if(animationStatus == 4)
                {
                    if(movementType == 1)
                    {
                        selectedBall = (Global.instance.turn-1) * 5 + ballUsed[Global.instance.turn-1];
                        CatchBall();
                    }
                    else if(movementType == 2 || movementType == 4)
                    {
                        DropBall();
                    }
                    else if(movementType == 3)
                    {
                        selectedBall = (Global.instance.turn-1) * 5 + ballUsed[Global.instance.turn-1];
                        CatchBall();
                    }
                }
                else if(animationStatus == 5)
                {
                    if(movementType == 1)
                    {
                        animationStatus = 0;
                    }
                    else if(movementType == 2)
                    {
                        EndOfTurn();
                        GetBall();
                    }
                    else if(movementType == 3)
                    {
                        PlaceOnDefaultPosition();
                    }
                    else if(movementType == 4)
                    {
                        ResetNextBall();
                    }
                }
            }
            else if(animationStatus == 2)
            {
                top.transform.position = previousTopPosition + new Vector3(0, 0, movZ) * animationProgress;
            }
            else if(animationStatus == 3)
            {
                arm.transform.position = previousArmPosition + new Vector3(movX, 0, 0) * animationProgress;
            }
            else if(animationStatus == 4)
            {
                arm.transform.localScale = new Vector3(1, 1 + scaleFactor * animationProgress, 1);
            }
            else if(animationStatus == 5)
            {
                arm.transform.localScale = new Vector3(1, 1 + scaleFactor * (1 - animationProgress), 1);
            }
        }
    }

    void EndOfTurn()
    {
        animationStatus = 0;
        Global.instance.EndOfTurn(spaceX, spaceY);
    }
}

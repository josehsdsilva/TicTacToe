using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartesianRobotController : MonoBehaviour
{
    public GameObject arm, top, balls, ballDropPoint;

    int spaceX, spaceY;
    float x = 1, z = 1, movX, movZ;
    Vector3 previousArmPosition, previousTopPosition;

    // Arm Movement
    private  float startX, startZ, currentX, currentZ;
    float spaceDistance = 1.1f;
    float scaleFactor = 5.4f;
    
    // Animation Control
    [Range(0.1f, 1f)]
    [SerializeField] float defaultAnimDuration = 0.1f;

    [HideInInspector] public int animationStatus = 0;
    float animationElapsedTime = 0.0f;
    float animationProgress = 0.0f;
    float animationDuration = 0.0f;
    int movementType;
    // 1 -  Get Ball to Play
    // 2 -  Move to Drop on board space
    // 3 -  Get Ball to reset
    // 4 -  Drop Ball to reset

    bool[,] resetedCircles = new bool[2, 5];


    // Balls Information
    Vector3[,] ballsStartPosition;
    int[] usedCircle;
    int selectedBall;
    string win;

    // Initialization
    void Awake()
    {
        usedCircle = new int[2];
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
    }

    // Orders
    public void GetCircle()
    {
        movementType = 1;
        movX = ballsStartPosition[GameController.instance.turn-1, usedCircle[GameController.instance.turn-1]].x - x;
        movZ = ballsStartPosition[GameController.instance.turn-1, usedCircle[GameController.instance.turn-1]].z - z;
        MoveZ();
    }

    public void Move(int _x, int _y)
    {
        Vector2 pos = GetSpacePosition(_x, _y);
        movementType = 2;
        spaceX = _x;
        spaceY = _y;
        movX = pos.x - x;
        movZ = pos.y - z;
        MoveZ();
    }

    Vector2 GetSpacePosition(int _x, int _y)
    {
        return new Vector2(-1.1f + spaceDistance * _x, 1.1f - spaceDistance * _y);
    }
        
    public void UpdateResetedCircles()
    {
        for (int player = 0; player < 2; player++)
        {
            for (int ball = 0; ball < 5; ball++)
            {
                if(HasSamePosition(ballsStartPosition[player, ball], balls.transform.GetChild(player * 5 + ball).transform.position)) resetedCircles[player, ball] = true;
                else resetedCircles[player, ball] = false;
            }
        }
    }

    public void ResetCircles()
    {
        animationStatus = 0;

        for (int player = 0; player < 2; player++)
        {
            for (int ball = 0; ball < 5; ball++)
            {
                if(!resetedCircles[player, ball])
                {
                    movementType = 3;
                    selectedBall = player * 5 + ball;
                    movX = balls.transform.GetChild(player * 5 + ball).transform.position.x - x;
                    movZ = balls.transform.GetChild(player * 5 + ball).transform.position.z - z;
                    resetedCircles[player, ball] = true;
                    MoveZ();
                    return;
                }
            }
        }

        // Game Reseted
        GameController.instance.ResetFinished();
    }

    // Helpers
    bool HasSamePosition(Vector3 startPos, Vector3 currentPos)
    {
        startX = RoundWith2Decimals(startPos.x);
        startZ = RoundWith2Decimals(startPos.z);
        currentX = RoundWith2Decimals(currentPos.x);
        currentZ = RoundWith2Decimals(currentPos.z);
        if( startX == currentX && startZ == currentZ) return true;
        return false;
    }

    float RoundWith2Decimals(float number)
    {
        return Mathf.Round(number * 100) * 0.01f;
    }

    public void ResetUsedCircles()
    {
        usedCircle[0] = 0;
        usedCircle[1] = 0;
    }

    // Arm Movement
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
        if(movementType == 2) usedCircle[GameController.instance.turn-1]++;
        ScaleDownArm();
    }

    void PlaceOnDefaultPosition()
    {
        int player = selectedBall / 5;
        int ball = selectedBall;
        if(selectedBall >= 5) ball = selectedBall - 5;
        
        movementType = 4;
        movX = ballsStartPosition[player, ball].x - x;
        movZ = ballsStartPosition[player, ball].z - z;
        MoveZ();
    }

    // Update
    void FixedUpdate()
    {
        if(animationStatus != 0)
        {
            animationElapsedTime += Time.deltaTime;
            animationProgress = animationElapsedTime / animationDuration;
            if(animationProgress >= 1)
            {
                animationProgress = 1;
                if(animationStatus == 2) // MoveZ
                {
                    top.transform.position = previousTopPosition + new Vector3(0, 0, movZ);
                    z += movZ;
                    MoveX();
                }
                else if(animationStatus == 3) // MoveX
                {
                    arm.transform.position = previousArmPosition + new Vector3(movX, 0, 0);
                    x += movX;
                    ScaleUpArm();
                }
                else if(animationStatus == 4) // ScaleUpArm
                {
                    if(movementType == 1)
                    {
                        selectedBall = (GameController.instance.turn-1) * 5 + usedCircle[GameController.instance.turn-1];
                        CatchBall();
                    }
                    else if(movementType == 2 || movementType == 4)
                    {
                        DropBall();
                    }
                    else if(movementType == 3)
                    {
                        CatchBall();
                    }
                }
                else if(animationStatus == 5) // ScaleDownArm
                {
                    if(movementType == 1)
                    {
                        animationStatus = 0;
                        GameController.instance.SetOnIdle();
                    }
                    else if(movementType == 2)
                    {
                        GameController.instance.EndOfTurn(spaceX, spaceY);
                        win = GameController.instance.CheckWin(GameController.instance.board);
                        if( win != null)
                        {
                            animationStatus = 0;
                            if(win == "0") GameController.instance.SetOnGameOver();
                            else GameController.instance.SetOnWin();
                        }
                        else
                        {
                            GameController.instance.SetOnGameSetup();
                        }
                    }
                    else if(movementType == 3)
                    {
                        PlaceOnDefaultPosition();
                    }
                    else if(movementType == 4)
                    {
                        ResetCircles();
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

}

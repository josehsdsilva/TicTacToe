using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public CartesianRobotController cartesianRobotController;

    public void Play(int _x, int _y)
    {
        if(Global.instance.board[_x, _y] == 0)
        {
            cartesianRobotController.Move(_x, _y);
        }
        else Debug.Log("Invalid Play");
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public CartesianRobotController cartesianRobotController;

    public void Play(int _x, int _y, Vector3 pos)
    {
        if(Global.instance.board[_x, _y] == 0 && cartesianRobotController.animationStatus == 0)
        {
            cartesianRobotController.Move(_x, _y, pos);
        }
        else Debug.Log("Invalid Play");
    }

    
}

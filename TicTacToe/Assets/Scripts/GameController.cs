using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    #region Gamestate Enum
        public enum Gamestate
        {
            gameSetup,
            idle,
            playerOnePlay,
            playerTwoPlay,
            Win,
            Reset
        }

    #endregion
    
    public CartesianRobotController cartesianRobotController;
    Gamestate gamestate;

    // Initialization
    void Start()
    {
        SetOnGameSetup();
        CatchCircle();
    }

    public void CatchCircle()
    {
        cartesianRobotController.GetCircle();
    }

    // Player 1 Play

    public void Play(int _x, int _y, Vector3 pos)
    {
        if(gamestate == Gamestate.idle && Global.instance.board[_x, _y] == 0 && cartesianRobotController.animationStatus == 0)
        {
            if(Global.instance.turn == 1) gamestate = Gamestate.playerOnePlay;
            else gamestate = Gamestate.playerTwoPlay;
            cartesianRobotController.Move(_x, _y, pos);
        }
        else Debug.Log("Invalid Play");
    }

    // Win
    public bool IsWin()
    {
        for (int aux = 0; aux < 3; aux++)
        {
            if(Global.instance.board[0, aux] != 0 && Global.instance.board[0, aux] == Global.instance.board[1, aux] && Global.instance.board[1, aux] == Global.instance.board[2, aux]) return true;
            if(Global.instance.board[aux, 0] != 0 && Global.instance.board[aux, 0] == Global.instance.board[aux, 1] && Global.instance.board[aux, 1] == Global.instance.board[aux, 2]) return true;
        }
        if(Global.instance.board[0, 0] != 0 && Global.instance.board[0, 0] == Global.instance.board[1, 1] && Global.instance.board[1, 1] == Global.instance.board[2, 2]) return true;
        if(Global.instance.board[0, 2] != 0 && Global.instance.board[0, 2] == Global.instance.board[1, 1] && Global.instance.board[1, 1] == Global.instance.board[2, 0]) return true;
        return false;
    }

    // Gamestates
    public void SetOnWin()
    {
        gamestate = Gamestate.Win;
        Debug.Log("Player " + Global.instance.turn + " Won!");
        SetOnResetGame();
    }

    public void SetOnIdle()
    {
        gamestate = Gamestate.idle;
    }

    public void SetOnGameSetup()
    {
        gamestate = Gamestate.gameSetup;
        CatchCircle();
    }

    public void SetOnResetGame()
    {
        gamestate = Gamestate.Reset;
        cartesianRobotController.ResetBallUsed();
        Global.instance.board = new int[3,3];
        Global.instance.turn = 1;
        cartesianRobotController.ResetBalls();
    }

    // Update

}

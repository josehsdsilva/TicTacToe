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
            humanPlay,
            AIPlay,
            Win,
            GameOver,
            Reset
        }

    #endregion
    
    public CartesianRobotController cartesianRobotController;
    Gamestate gamestate;

    public int[,] board = new int[3,3];

    public int turn = 1;

    int humanPlayerTurn = 1;

    // Initialization
    void Start()
    {
        RandomizeStartingPlayer();
        SetOnGameSetup();
        CatchCircle();
    }

    public void CatchCircle()
    {
        cartesianRobotController.GetCircle();
    }

    void RandomizeStartingPlayer()
    {
        humanPlayerTurn = Random.Range(1, 3);
    }

    // Human Play
    public void Play(int _x, int _y)
    {
        if(gamestate == Gamestate.idle && board[_x, _y] == 0 && cartesianRobotController.animationStatus == 0 && turn == humanPlayerTurn)
        {
            gamestate = Gamestate.humanPlay;
            cartesianRobotController.Move(_x, _y);
        }
        else Debug.Log("Invalid Play");
    }

    // AI Play
    public void AIPlay()
    {
        Vector2Int pos = GetRandomPlay();
        if(gamestate == Gamestate.idle && board[pos.x, pos.y] == 0 && cartesianRobotController.animationStatus == 0 && turn != humanPlayerTurn)
        {
            gamestate = Gamestate.AIPlay;
            cartesianRobotController.Move(pos.x, pos.y);
        }
        else Debug.Log("Invalid Play");
    }

    Vector2Int GetRandomPlay()
    {
        int auxX, auxY;
        for (int i = 0; i < 1; i++)
        {
            auxX = Random.Range(0, 3);
            auxY = Random.Range(0, 3);
            if(board[auxX, auxY] == 0) return new Vector2Int(auxX, auxY);
            i--;
        }
        return Vector2Int.zero;
    }

    // Turn

    public void EndOfTurn(int _x, int _y)
    {
        board[_x, _y] = turn;
        turn++;
        if(turn >= 3) turn = 1;
    }

    // Win
    public bool IsWin()
    {
        for (int aux = 0; aux < 3; aux++)
        {
            if(board[0, aux] != 0 && board[0, aux] == board[1, aux] && board[1, aux] == board[2, aux]) return true;
            if(board[aux, 0] != 0 && board[aux, 0] == board[aux, 1] && board[aux, 1] == board[aux, 2]) return true;
        }
        if(board[0, 0] != 0 && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) return true;
        if(board[0, 2] != 0 && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]) return true;
        return false;
    }

    // GameOver
    public bool IsGameOver()
    {
        int auxCounter = 0;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if(board[x, y] != 0) auxCounter++;
            }
        }
        return auxCounter >= 9;
    }

    // Gamestates
    public void SetOnWin()
    {
        gamestate = Gamestate.Win;
        Debug.Log("Player " + turn + " Won!");
        SetOnResetGame();
    }

    public void SetOnGameOver()
    {
        gamestate = Gamestate.GameOver;
        Debug.Log("Player " + turn + " Won!");
        SetOnResetGame();
    }

    public void SetOnIdle()
    {
        gamestate = Gamestate.idle;
        if(turn != humanPlayerTurn)
        {
            AIPlay();
        }
    }

    public void SetOnGameSetup()
    {
        gamestate = Gamestate.gameSetup;
        CatchCircle();
    }

    void SetOnResetGame()
    {
        gamestate = Gamestate.Reset;
        cartesianRobotController.ResetUsedCircles();
        board = new int[3,3];
        turn = 1;
        cartesianRobotController.UpdateResetedCircles();
        cartesianRobotController.ResetCircles();
    }

    public void ResetFinished()
    {
        RandomizeStartingPlayer();
        SetOnGameSetup();
    }

}

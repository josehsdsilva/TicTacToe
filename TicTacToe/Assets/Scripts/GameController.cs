using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    #region Singleton
    public static GameController instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }
    #endregion

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
    public AI AIPlayer;
    Gamestate gamestate;

    public int[,] board = new int[3,3];

    public int turn = 1;

    public int humanPlayerTurn = 1, aiPlayerTurn = 2;

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
        if(humanPlayerTurn == 1) aiPlayerTurn = 2;
        else aiPlayerTurn = 1;
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
        Vector2Int pos = AIPlayer.GetAIPlay();
        if(gamestate == Gamestate.idle && board[pos.x, pos.y] == 0 && cartesianRobotController.animationStatus == 0 && turn == aiPlayerTurn)
        {
            gamestate = Gamestate.AIPlay;
            cartesianRobotController.Move(pos.x, pos.y);
        }
        else Debug.Log("Invalid Play");
    }

    // Turn

    public void EndOfTurn(int _x, int _y)
    {
        board[_x, _y] = turn;
        turn++;
        if(turn >= 3) turn = 1;
    }

    // Win
    public string CheckWin(int [,] _board)
    {
        for (int aux = 0; aux < 3; aux++)
        {
            if(_board[0, aux] != 0 && _board[0, aux] == _board[1, aux] && _board[1, aux] == _board[2, aux])
            {
                if(_board[2, aux] == turn) return "1";
                return "-1";
            }
            if(_board[aux, 0] != 0 && _board[aux, 0] == _board[aux, 1] && _board[aux, 1] == _board[aux, 2])
            {
                if(_board[aux, 2] == turn) return "1";
                return "-1";
            }
        }
        if(_board[0, 0] != 0 && _board[0, 0] == _board[1, 1] && _board[1, 1] == _board[2, 2])
        {
            if(_board[2, 2] == turn) return "1";
            return "-1";
        }
        if(_board[0, 2] != 0 && _board[0, 2] == _board[1, 1] && _board[1, 1] == _board[2, 0])
        {
            if(_board[2, 0] == turn) return "1";
            return "-1";
        }
        if(IsGameOver(board)) return "0";
        return null;
    }

    // GameOver
    public bool IsGameOver(int[,] _board)
    {
        int auxCounter = 0;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if(_board[x, y] != 0) auxCounter++;
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
        Debug.Log("Tie");
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

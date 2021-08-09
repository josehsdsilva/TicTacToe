using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public Vector2Int GetAIPlay()
    {
        int score, bestScore = -1000;
        Vector2Int move = new Vector2Int();
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if(GameController.instance.board[x, y] == 0) 
                {
                    GameController.instance.board[x, y] = GameController.instance.aiPlayerTurn;
                    score = Minimax(GameController.instance.board, 0, -1000, 1000, false);
                    GameController.instance.board[x, y] = 0;
                    if(score > bestScore)
                    {
                        bestScore = score;
                        move = new Vector2Int(x, y);
                    }
                }
            }
        }
        return move;
    }

    int Minimax(int[,] board, int depth, int alpha, int beta, bool maximizingPlayer)
    {
        bool doubleBreak = false;
        int bestEval, eval;
        if(depth == 0)
        {
            return 0;
        }
        string result = GameController.instance.CheckWin(board);
        if(result != null) return int.Parse(result);
        
        if(maximizingPlayer)
        {
            bestEval = -1000;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if(board[x, y] == 0)
                    {
                        board[x, y] = GameController.instance.aiPlayerTurn;
                        eval = Minimax(board, depth + 1, alpha, beta, false);
                        board[x, y] = 0;
                        bestEval = Mathf.Max(bestEval, eval);
                        alpha = Mathf.Max(alpha, eval);
                        if(beta <= alpha)
                        {
                            doubleBreak = true;
                            break;
                        }
                    }
                }
                if(doubleBreak)
                {
                    doubleBreak = false;
                    break;
                }
            }
            return bestEval;
        }
        else
        {
            bestEval = 1000;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if(board[x, y] == 0)
                    {
                        board[x, y] = GameController.instance.humanPlayerTurn;
                        eval = Minimax(board, depth + 1, alpha, beta, true);
                        board[x, y] = 0;
                        bestEval = Mathf.Min(bestEval, eval);
                        beta = Mathf.Min(beta, eval);
                        if(beta <= alpha)
                        {
                            doubleBreak = true;
                            break;
                        }
                    }
                }
                if(doubleBreak)
                {
                    doubleBreak = false;
                    break;
                }
            }
            return bestEval;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    #region Singleton
    public static Global instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }
    #endregion

    public int[,] board = new int[3,3];
    public int turn = 1;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 1000;
    }


    public void EndOfTurn(int _x, int _y)
    {
        board[_x, _y] = turn;
        turn++;
        if(turn >= 3) turn = 1;
        Debug.Log("Player " + turn + " turn!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITest : MonoBehaviour
{
    //玩家
    private int player;
    private int winner;
    //棋盘格
    private int[,] chessBox = new int[3, 3];
    //局数
    private int count;


    void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        //初始化先手玩家
        player = 1;
        //赢家初始化为0
        winner = 0;
        //初始化局数
        count = 0;
        //初始化棋盘格
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                chessBox[i, j] = 0;
            }
        }
    }

    void OnGUI()
    {
        //初始化背景棋格,必须要有后面的“”
        GUI.Box(new Rect(250, 50, 300, 300), "");
        //初始化九个格子
        if (!EndGame())
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //button放在里面反应响应事件
                    if (chessBox[i, j] == 0 && GUI.Button(new Rect(295 + 70 * i, 95 + 70 * j, 70, 70), ""))
                    {
                        PutChess(i, j);
                    }
                    else if (chessBox[i, j] == 1) GUI.Button(new Rect(295 + 70 * i, 95 + 70 * j, 70, 70), "X");
                    else if (chessBox[i, j] == 2) GUI.Button(new Rect(295 + 70 * i, 95 + 70 * j, 70, 70), "O");
                }
            }
        }
        else
        {
            if (winner != 0)
            {
                GUI.Label(new Rect(320, 150, 160, 100), "            玩家 " + winner + " 获胜!");
            }
            else
            {
                GUI.Label(new Rect(320, 150, 160, 100), "                平局！");
            }
            //重新开始
            if (GUI.Button(new Rect(350, 200, 100, 50), "重新开始"))
            {
                StartGame();
            }
            ///退出游戏
            if (GUI.Button(new Rect(350, 250, 100, 50), "退出游戏"))
            {
                ExitGame();
            }
        }
    }
    private void PutChess(int i, int j)
    {
        chessBox[i, j] = player;
        player = 3 - player;
        count++;
    }

    private bool EndGame()
    {

        //判断横行
        for (int i = 0; i < 3; i++)
        {
            if (chessBox[i,0]!=0 && (chessBox[i, 0] == chessBox[i, 1]) && (chessBox[i, 0] == chessBox[i, 2])) winner = chessBox[i, 1];
        }
        //判断竖行
        for (int i = 0; i < 3; i++)
        {
            if (chessBox[0,i]!=0 && (chessBox[0, i] == chessBox[1, i]) && (chessBox[1, i] == chessBox[2, i])) winner = chessBox[1, i];
        }
        //判断斜方向
        if ((chessBox[1, 1] == chessBox[2, 2]) && (chessBox[0, 0] == chessBox[2, 2])) winner = chessBox[1, 1];
        if ((chessBox[0, 2] == chessBox[1, 1]) && (chessBox[2, 0] == chessBox[1, 1])) winner = chessBox[1, 1];

        if (count < 9 && winner == 0) return false;
        return true;
    }


    public void ExitGame()
    {
        //退出游戏
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}

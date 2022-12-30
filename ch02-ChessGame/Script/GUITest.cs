using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITest : MonoBehaviour
{
    //���
    private int player;
    private int winner;
    //���̸�
    private int[,] chessBox = new int[3, 3];
    //����
    private int count;


    void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        //��ʼ���������
        player = 1;
        //Ӯ�ҳ�ʼ��Ϊ0
        winner = 0;
        //��ʼ������
        count = 0;
        //��ʼ�����̸�
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
        //��ʼ���������,����Ҫ�к���ġ���
        GUI.Box(new Rect(250, 50, 300, 300), "");
        //��ʼ���Ÿ�����
        if (!EndGame())
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //button�������淴Ӧ��Ӧ�¼�
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
                GUI.Label(new Rect(320, 150, 160, 100), "            ��� " + winner + " ��ʤ!");
            }
            else
            {
                GUI.Label(new Rect(320, 150, 160, 100), "                ƽ�֣�");
            }
            //���¿�ʼ
            if (GUI.Button(new Rect(350, 200, 100, 50), "���¿�ʼ"))
            {
                StartGame();
            }
            ///�˳���Ϸ
            if (GUI.Button(new Rect(350, 250, 100, 50), "�˳���Ϸ"))
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

        //�жϺ���
        for (int i = 0; i < 3; i++)
        {
            if (chessBox[i,0]!=0 && (chessBox[i, 0] == chessBox[i, 1]) && (chessBox[i, 0] == chessBox[i, 2])) winner = chessBox[i, 1];
        }
        //�ж�����
        for (int i = 0; i < 3; i++)
        {
            if (chessBox[0,i]!=0 && (chessBox[0, i] == chessBox[1, i]) && (chessBox[1, i] == chessBox[2, i])) winner = chessBox[1, i];
        }
        //�ж�б����
        if ((chessBox[1, 1] == chessBox[2, 2]) && (chessBox[0, 0] == chessBox[2, 2])) winner = chessBox[1, 1];
        if ((chessBox[0, 2] == chessBox[1, 1]) && (chessBox[2, 0] == chessBox[1, 1])) winner = chessBox[1, 1];

        if (count < 9 && winner == 0) return false;
        return true;
    }


    public void ExitGame()
    {
        //�˳���Ϸ
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}

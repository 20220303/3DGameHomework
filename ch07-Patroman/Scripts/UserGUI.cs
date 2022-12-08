using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {

    private IUserAction action;
    private GUIStyle score_style = new GUIStyle();
    private GUIStyle text_style = new GUIStyle();
    private GUIStyle over_style = new GUIStyle();
    private int show_time = 8;
    void Start ()
    {
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
        text_style.normal.textColor = new Color(205, 179, 128, 1);
        text_style.fontSize = 16;
        score_style.normal.textColor = new Color(3,101,100,1);
        score_style.fontSize = 16;
        over_style.fontSize = 25;
        //好的方法实现一个时间差，StartCoroutine函数和yield return成对出现。
        StartCoroutine(ShowTip());
    }

    void Update()
    {
        //这里只控制人物的微笑
        action.Attack();

    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 5, 200, 50), "分数:", text_style);
        GUI.Label(new Rect(55, 5, 200, 50), action.GetScore().ToString(), score_style);
        if(action.GetGameover() && action.GetScore() != 20)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 250, 100, 100), "游戏结束", over_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 150, 100, 50), "重新开始"))
            {
                //重新开始
                action.Restart();
                return;
            }
        }//七个房间就胜利
        else if(action.GetScore() == 20)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 250, 100, 100), "恭喜胜利！", over_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 150, 100, 50), "重新开始"))
            {
                //重新开始
                action.Restart();
                return;
            }
        }

        if(show_time > 0)
        {
            GUI.Label(new Rect(0 ,30, 100, 100), "你被困在了无脸人世界，请向场景中的八个无脸人打招呼", text_style);
            GUI.Label(new Rect(0, 50, 100, 100), "但是请注意，不要靠的太近，否则他们会攻击你", text_style);
            GUI.Label(new Rect(0, 70, 100, 100), "WASD控制角色移动,靠近无脸人能够获得一分，获得二十分就能够胜利", text_style);
            GUI.Label(new Rect(0, 90, 100, 100), "鼠标左键微笑，无脸人会因为嫉妒你的快乐而消失...", text_style);
            GUI.Label(new Rect(0, 110, 100, 100), "不过，赶尽杀绝可不是什么好办法", text_style);
        }
    }

    public IEnumerator ShowTip()
    {
        while (show_time >= 0)
        {
            yield return new WaitForSeconds(1);
            show_time--;
        }
    }
}

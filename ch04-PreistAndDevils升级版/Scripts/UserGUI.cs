using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PerfabsController;
using interfaceApplication;


class UserGUI:MonoBehaviour
{
    //实现向用户展示的界面
    //成功或者失败，重新开始
    private UserAction action;
    public int status = 0;
	//设置标签的样式
	GUIStyle style;

	void Start()
	{
		action = Director.getInstance().currentSceneController as UserAction;
		style = new GUIStyle();
		style.fontSize = 40;
		style.alignment = TextAnchor.MiddleCenter;
	}

	void OnGUI()
	{
		if (status == 1)
		{
			GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "Gameover!",style);

			if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart"))
			{
				status = 0;
				action.restart();
			}
		}
		else if (status == 2)
		{
			GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "Congratulations!",style);

			if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart"))
			{
				status = 0;
				action.restart();
			}
		}

	}
}

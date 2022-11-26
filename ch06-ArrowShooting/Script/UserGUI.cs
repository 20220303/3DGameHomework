using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
	private IUserAction action;
	GUIStyle style = new GUIStyle();
	GUIStyle style2 = new GUIStyle();
	private bool game_start = false;


	public Camera cam;
	private float holdTime =0;

	void Start()
	{
		action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
		style.normal.textColor = new Color(0, 0, 0, 1);
		style.fontSize = 16;
		style2.normal.textColor = new Color(1, 1, 1);
		style2.fontSize = 40;
		holdTime = 0;
		cam = Camera.main;
	}
	void Update()
	{
		if (game_start)
		{
			if (action.haveArrowOnPort())
			{
				if (Input.GetMouseButton(0))
				{

					holdTime += Time.deltaTime;
					Vector3 fwd = cam.transform.forward;
					fwd.Normalize();

					if (Input.GetMouseButton(0))
					{

						action.MoveBow(fwd, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

						//���ƹ�������
						//this.transform.localScale += new Vector3(Input.GetAxis("Mouse X")*0.1f, Input.GetAxis("Mouse Y")*0.01f, Input.GetAxis("Mouse X") * 0.1f);
					}


				}

				if (Input.GetMouseButtonUp(0))
				{

					action.Shoot(holdTime);
					holdTime = 0;
					
				}
			}//���¿ո�װ���
			if (Input.GetKeyDown(KeyCode.Space))
			{
				action.create();
			}
		}
	}
	private void OnGUI()
	{
		if (game_start)
		{

			GUI.Label(new Rect(10, 5, 200, 50), "����: ", style);
			GUI.Label(new Rect(10, 45, 200, 50), "����:", style);
			GUI.Label(new Rect(10, 85, 200, 50), "����:", style);
			GUI.Label(new Rect(55, 5, 200, 50), action.GetWind(), style);
			GUI.Label(new Rect(55, 45, 200, 50), action.GetScore().ToString(), style);
			GUI.Label(new Rect(55, 85, 200, 50), holdTime.ToString(), style);
			
			if (GUI.Button(new Rect(Screen.width - 100, 5, 100, 50), "���¿�ʼ"))
			{
				action.Restart();
				return;
			}

		}
		else
		{
			GUI.Label(new Rect(Screen.width / 2 - 120, Screen.width / 2 - 320, 100, 100), "Arrow Shooting", style2);
			if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 150, 100, 50), "��Ϸ��ʼ"))
			{
				game_start = true;
				action.BeginGame();
			}
		}

	}
}
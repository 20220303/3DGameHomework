using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserGUI : MonoBehaviour
{
	private IUserAction action;
	GUIStyle style = new GUIStyle();
	GUIStyle style2 = new GUIStyle();
	GUIStyle style3 = new GUIStyle();
	private bool game_start = false;
	private float deltatime = 0;
	public scoreRecorder recorder;
	public AudioSource music;//音源AudioSource相当于播放器，而音效AudioClip相当于磁带
	public AudioClip shoot;//这里我要给主角添加跳跃的音效


	public Camera cam;
	private float holdTime =0;
	private int arrow_sum = 21;

	void Start()
	{
		action = SSDirector.GetInstance().CurrentScenceController as IUserAction;



		recorder = singleton<scoreRecorder>.Instance;
		//给对象添加一个AudioSource组件
		music = gameObject.AddComponent<AudioSource>();
		//设置不一开始就播放音效
		music.playOnAwake = false;
		//加载音效文件，我把跳跃的音频文件命名为jump
		shoot = Resources.Load<AudioClip>("music/shoot");
		//把音源music的音效设置为jump
		music.clip = shoot;


		style.normal.textColor = new Color(0, 0, 0, 1);
		style.fontSize = 32;
		style2.normal.textColor = new Color(1, 1, 1);
		style2.fontSize = 64;
		style3.normal.textColor = new Color(1, 1, 1);
		style3.fontSize = 32;
		holdTime = 0;
		cam = Camera.main;
	}


	void Update()
	{

        if (game_start)
        {
			
			
			if (action.haveArrowOnPort())
            {


				//更新箭的位置
				//action.UpdateArrowPositioin();

				//这里改成每2秒射击一次
				deltatime += Time.deltaTime;
				Vector3 fwd = cam.transform.forward;
				fwd.Normalize();


				if (deltatime >= 1.3)
				{
					//action.MoveBow(fwd, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
					holdTime = deltatime;
					action.Shoot(holdTime);
					holdTime = 0;
					deltatime = 0;
				}


				
			}
            else if(arrow_sum>0)
            {

				deltatime += Time.deltaTime;
				if (deltatime >= 0.5)
                {
					action.create();
					deltatime = 0;
					//播放音效
					music.Play();
					arrow_sum--;
				}
				
			}


        }


	}
	private void OnGUI()
	{
		action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
		if (game_start)
		{

			GUI.Label(new Rect(10, 5, 200, 50), "风向: ", style);
			GUI.Label(new Rect(10, 45, 200, 50), "分数:", style);
			//GUI.Label(new Rect(10, 85, 200, 50), "力量:", style);
			//Debug.Log("风&&&&&&&&&&&&&&&&&&" + action.GetScore());


			GUI.Label(new Rect(90, 5, 200, 50), action.GetWind(), style);
			//GUI.Label(new Rect(90, 45, 200, 50), action.GetScore(), style);
			GUI.Label(new Rect(90, 45, 200, 50), recorder.score.ToString(), style);
			//GUI.Label(new Rect(90, 85, 200, 50), holdTime.ToString(), style);

			if (GUI.Button(new Rect(Screen.width - 200, 5, 200, 100), "重新开始") )
			{
				Time.timeScale = 1;
				action.Restart();
				return;
			}

            if (arrow_sum==0 && recorder.score >= 50)
            {
				GUI.Label(new Rect(Screen.width / 2 - 220, Screen.width / 2 - 300, 100, 100), "Congratulations!", style2);
				Time.timeScale = 0;
			}
			else if (arrow_sum == 0 && recorder.score < 50)//射完了箭
            {
				GUI.Label(new Rect(Screen.width / 2 - 220, Screen.width / 2 - 300, 100, 100), "Please Try Again!", style2);
				Time.timeScale = 0;
			}

		}
		else
		{
			Time.timeScale = 1;
			GUI.Label(new Rect(Screen.width / 2 - 220, Screen.width / 2 - 400, 100, 100), "Arrow Shooting", style2);
			GUI.Label(new Rect(Screen.width / 2 - 180, Screen.width / 2 - 300, 100, 100), " 获得50分即游戏胜利！", style3);
			if (GUI.Button(new Rect(Screen.width / 2 - 120, Screen.width / 2 - 150, 200, 100), "游戏开始"))
			{
				game_start = true;
				action.BeginGame();
			}
		}

	}
}
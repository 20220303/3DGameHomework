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
	public AudioSource music;//��ԴAudioSource�൱�ڲ�����������ЧAudioClip�൱�ڴŴ�
	public AudioClip shoot;//������Ҫ�����������Ծ����Ч


	public Camera cam;
	private float holdTime =0;
	private int arrow_sum = 21;

	void Start()
	{
		action = SSDirector.GetInstance().CurrentScenceController as IUserAction;



		recorder = singleton<scoreRecorder>.Instance;
		//���������һ��AudioSource���
		music = gameObject.AddComponent<AudioSource>();
		//���ò�һ��ʼ�Ͳ�����Ч
		music.playOnAwake = false;
		//������Ч�ļ����Ұ���Ծ����Ƶ�ļ�����Ϊjump
		shoot = Resources.Load<AudioClip>("music/shoot");
		//����Դmusic����Ч����Ϊjump
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


				//���¼���λ��
				//action.UpdateArrowPositioin();

				//����ĳ�ÿ2�����һ��
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
					//������Ч
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

			GUI.Label(new Rect(10, 5, 200, 50), "����: ", style);
			GUI.Label(new Rect(10, 45, 200, 50), "����:", style);
			//GUI.Label(new Rect(10, 85, 200, 50), "����:", style);
			//Debug.Log("��&&&&&&&&&&&&&&&&&&" + action.GetScore());


			GUI.Label(new Rect(90, 5, 200, 50), action.GetWind(), style);
			//GUI.Label(new Rect(90, 45, 200, 50), action.GetScore(), style);
			GUI.Label(new Rect(90, 45, 200, 50), recorder.score.ToString(), style);
			//GUI.Label(new Rect(90, 85, 200, 50), holdTime.ToString(), style);

			if (GUI.Button(new Rect(Screen.width - 200, 5, 200, 100), "���¿�ʼ") )
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
			else if (arrow_sum == 0 && recorder.score < 50)//�����˼�
            {
				GUI.Label(new Rect(Screen.width / 2 - 220, Screen.width / 2 - 300, 100, 100), "Please Try Again!", style2);
				Time.timeScale = 0;
			}

		}
		else
		{
			Time.timeScale = 1;
			GUI.Label(new Rect(Screen.width / 2 - 220, Screen.width / 2 - 400, 100, 100), "Arrow Shooting", style2);
			GUI.Label(new Rect(Screen.width / 2 - 180, Screen.width / 2 - 300, 100, 100), " ���50�ּ���Ϸʤ����", style3);
			if (GUI.Button(new Rect(Screen.width / 2 - 120, Screen.width / 2 - 150, 200, 100), "��Ϸ��ʼ"))
			{
				game_start = true;
				action.BeginGame();
			}
		}

	}
}
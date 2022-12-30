using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controllor : MonoBehaviour, IUserAction, ISceneController
{
	public scoreRecorder recorder;
	public ArrowFactory arrow_factory;
	public ArrowFlyActionManager action_manager;
	public UserGUI user_gui;
	SSDirector director;




	private GameObject AB;
	private GameObject arrow;
	private GameObject target;
	private GameObject ImageTargetAB;
	private bool game_start = false;
	private string wind = "";
	private float wind_directX;
	private float wind_directY;
	public string GetScore() {

		return recorder.getScore(); 
	}
	public string GetWind() {
		
		return wind; 
	}
	public void Restart() { SceneManager.LoadScene(0); }
	public void BeginGame() { game_start = true; }
	void Update() { if (game_start) arrow_factory.FreeArrow(); }
	public void LoadResources() {
		//找到场景中的弓和靶
		target = GameObject.Find("target");
		AB = GameObject.Find("AB");
		ImageTargetAB = GameObject.Find("ImageTargetAB");//这个主要是控制弓箭的方向


	}
	public bool haveArrowOnPort() { return (arrow != null); }
	void Start()
	{
		director = SSDirector.GetInstance();
		director.CurrentScenceController = this;
		LoadResources();

		arrow_factory = singleton<ArrowFactory>.Instance;
		arrow_factory.initiate(AB, ImageTargetAB);
		recorder = gameObject.AddComponent<scoreRecorder>() as scoreRecorder;
		user_gui = gameObject.AddComponent<UserGUI>() as UserGUI;
		action_manager = gameObject.AddComponent<ArrowFlyActionManager>() as ArrowFlyActionManager;
		
	}
	public void create()
	{
		if (arrow == null)
		{

			wind_directX = Random.Range(-2f, 2f);
			wind_directY = Random.Range(-2f, 2f);
			CreateWind();
			arrow = arrow_factory.GetArrow();
		}
	}
	public void UpdateArrowPositioin()
	{

		
		arrow.transform.SetPositionAndRotation(ImageTargetAB.transform.position, Quaternion.Euler(ImageTargetAB.transform.rotation.x, ImageTargetAB.transform.rotation.y+90, ImageTargetAB.transform.rotation.z));
		
	}


	public void MoveBow(Vector3 fwd,float Xinput,float Yinput)
	{
		if (!game_start) { return; }
		//这里就不直接用触屏移动弓箭了，使用AR的方式移动并旋转角度
		//Vector3 vaxis = Vector3.Cross(fwd, Vector3.right);
		//AB.transform.Rotate(vaxis, -Xinput * 5, Space.World);
		//Vector3 haxis = Vector3.Cross(fwd, Vector3.up);
		//AB.transform.Rotate(haxis, -Yinput * 5, Space.World);
		
	}


	public void Shoot(float holdTime)
	{
		if (game_start)
		{
			Vector3 wind = new Vector3(wind_directX, wind_directY, 0);
			action_manager.ArrowFly(arrow, wind, holdTime);
			//这里添加一个音效控件
			arrow = null;

		}
	}
	public void CreateWind()
	{
		string Horizontal = "", Vertical = "", level = "";
		if (wind_directX > 0)
		{
			Horizontal = "西";
		}
		else if (wind_directX <= 0)
		{
			Horizontal = "东";
		}
		if (wind_directY > 0)
		{
			Vertical = "南";
		}
		else if (wind_directY <= 0)
		{
			Vertical = "北";
		}
		if ((wind_directX + wind_directY) / 2 > -0.5f && (wind_directX + wind_directY) / 2 < 0.5f)
		{
			level = "1 级";
		}
		else if ((wind_directX + wind_directY) / 2 > -1f && (wind_directX + wind_directY) / 2 < 1f)
		{
			level = "2 级";
		}
		else if ((wind_directX + wind_directY) / 2 > -1.5f && (wind_directX + wind_directY) / 2 < 1.5f)
		{
			level = "3 级";
		}
		else if ((wind_directX + wind_directY) / 2 > -2f && (wind_directX + wind_directY) / 2 < 2f)
		{
			level = "4 级";
		}

		wind = Horizontal + Vertical + "风" + " " + level;
	}

}
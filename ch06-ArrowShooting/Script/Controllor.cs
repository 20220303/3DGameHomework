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


	private GameObject AB;
	private GameObject arrow;
	private GameObject target;
	private bool game_start = false;
	private string wind = "";
	private float wind_directX;
	private float wind_directY;
	public int GetScore() { return recorder.score; }
	public string GetWind() { return wind; }
	public void Restart() { SceneManager.LoadScene(0); }
	public void BeginGame() { game_start = true; }
	void Update() { if (game_start) arrow_factory.FreeArrow(); }
	public void LoadResources() {

		target = Instantiate(Resources.Load("Prefabs/target", typeof(GameObject))) as GameObject;
		AB = Instantiate(Resources.Load("Prefabs/AB", typeof(GameObject))) as GameObject;

	}
	public bool haveArrowOnPort() { return (arrow != null); }
	void Start()
	{
		SSDirector director = SSDirector.GetInstance();
		director.CurrentScenceController = this;
		LoadResources();

		arrow_factory = singleton<ArrowFactory>.Instance;
		arrow_factory.initiate(AB);
		recorder = gameObject.AddComponent<scoreRecorder>() as scoreRecorder;
		user_gui = gameObject.AddComponent<UserGUI>() as UserGUI;
		action_manager = gameObject.AddComponent<ArrowFlyActionManager>() as ArrowFlyActionManager;
		
	}
	public void create()
	{
		if (arrow == null)
		{

			wind_directX = Random.Range(-0.4f, 0.4f);
			wind_directY = Random.Range(-0.4f, 0.4f);
			CreateWind();
			arrow = arrow_factory.GetArrow();
		}
	}
	public void MoveBow(Vector3 fwd,float Xinput,float Yinput)
	{
		if (!game_start) { return; }

		Vector3 vaxis = Vector3.Cross(fwd, Vector3.right);
		AB.transform.Rotate(vaxis, -Xinput * 5, Space.World);
		Vector3 haxis = Vector3.Cross(fwd, Vector3.up);
		AB.transform.Rotate(haxis, -Yinput * 5, Space.World);
		
	}


	public void Shoot(float holdTime)
	{
		if (game_start)
		{
			Vector3 wind = new Vector3(wind_directX, wind_directY, 0);
			action_manager.ArrowFly(arrow, wind, holdTime);
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
		if ((wind_directX + wind_directY) / 0.2f > -0.1f && (wind_directX + wind_directY) / 0.2f < 0.1f)
		{
			level = "1 级";
		}
		else if ((wind_directX + wind_directY) / 0.2f > -0.2f && (wind_directX + wind_directY) / 0.2f < 0.2f)
		{
			level = "2 级";
		}
		else if ((wind_directX + wind_directY) / 0.2f > -0.3f && (wind_directX + wind_directY) / 0.2f < 0.3f)
		{
			level = "3 级";
		}
		else if ((wind_directX + wind_directY) / 0.2f > -0.5f && (wind_directX + wind_directY) / 0.2f < 0.5f)
		{
			level = "4 级";
		}

		wind = Horizontal + Vertical + "风" + " " + level;
	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ArrowCollider : MonoBehaviour
{
	public Controllor scene_controller;
	public scoreRecorder recorder;

	void Start()
	{
		scene_controller = SSDirector.GetInstance().CurrentScenceController as Controllor;
		recorder = singleton<scoreRecorder>.Instance;
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.name == "T1" || c.gameObject.name == "T2" || c.gameObject.name == "T3" || c.gameObject.name == "T4" || c.gameObject.name == "T5")
		{
			gameObject.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			gameObject.SetActive(false);
			//Debug.Log(this.gameObject.transform.position.x);
			//Debug.Log(this.gameObject.transform.position.y);

			float dis = Mathf.Sqrt(this.gameObject.transform.position.x * this.gameObject.transform.position.x + this.gameObject.transform.position.y * this.gameObject.transform.position.y);
			//Debug.Log("dis"+dis.ToString());
			float point = 0;
			if (dis >= 0 && dis < 0.5) point = 4f;
			else if(dis >= 0.5 && dis < 1) point = 3f;
			else if(dis >= 1 && dis < 1.5) point = 2f;
			else if(dis >= 1.5 && dis < 2) point = 1f;
			else if(dis >= 2 && dis < 2.5) point = 0f;


			recorder.Record((int)Mathf.Floor(point));
		}

	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PerfabsController;
using interfaceApplication;

class ClickGUI:MonoBehaviour
{

	//这里控制点击之后发生的事情，关于船和物体
	UserAction action;
	PAndVController POrVCtr;


	public void setController(PAndVController POrVCtrl)
	{
		POrVCtr = POrVCtrl;
	}

	void Start()
	{
		action = Director.getInstance().currentSceneController as UserAction;
	}

    //控制鼠标点击事件
    void OnMouseDown()
	{
		if (gameObject.name == "boat")
		{
			//只要点击了boat，就让它摇晃一下

			//action.boatShake();

			action.moveBoat();

		}
		else
		{
			action.PAndVIsClicked(POrVCtr);

		}
	}

}

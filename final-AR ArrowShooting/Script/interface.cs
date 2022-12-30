using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneController
{
	void LoadResources();
}

public interface IUserAction
{
	void MoveBow(Vector3 fwd, float Xinput, float Yinput);
	void Shoot(float holdTime);
	string GetScore();
	void Restart();
	string GetWind();
	void BeginGame();
	void create();
	bool haveArrowOnPort();
	void UpdateArrowPositioin();
}

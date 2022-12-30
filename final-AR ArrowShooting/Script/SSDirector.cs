using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSDirector : object
{
	private static SSDirector _instance;
	public ISceneController CurrentScenceController { get; set; }
	public static SSDirector GetInstance()
	{
		if (_instance == null)
		{
			_instance = new SSDirector();
		}
		return _instance;
	}
}
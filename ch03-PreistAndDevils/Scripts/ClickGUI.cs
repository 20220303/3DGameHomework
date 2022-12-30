using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;

public class ClickGUI : MonoBehaviour {
	UserAction action;
	MyCharacterController characterController;

	public void setController(MyCharacterController characterCtrl) {
		characterController = characterCtrl;
	}

	void Start() {
		action = Director.getInstance ().currentSceneController as UserAction;
	}

	void OnMouseDown() {
		if (gameObject.name == "boat") {
			Debug.Log("action.moveBoat ();");
			action.moveBoat ();
			Debug.Log("re action.moveBoat ();");

		} else {
			Debug.Log("action.characterIsClicked (characterController);");
			action.characterIsClicked (characterController);
			
		}
	}


}

using UnityEngine;
using System.Collections;

public class SingleLevelSelectButton : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ClickButton(int i){

		switch (i) {
		case 0:
			userDataManager.level = userDataManager.LEVEL.EASY;
			Application.LoadLevel ("GameScene");
			break;
		case 1:
			userDataManager.level = userDataManager.LEVEL.NORMAL;
			Application.LoadLevel ("GameScene");
			break;
		case 2:
			userDataManager.level = userDataManager.LEVEL.HARD;
			Application.LoadLevel ("GameScene");
			break;
		}
	}
}

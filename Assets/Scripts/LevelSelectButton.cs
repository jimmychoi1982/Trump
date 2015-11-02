using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour {

	[SerializeField] private Button _easyButton;
	[SerializeField] private Button _nomalButton;
	[SerializeField] private Button _hardButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ClickButton(int level){

		switch ((userDataManager.LEVEL)level) {
		case userDataManager.LEVEL.EASY:

			_easyButton.image.color = Color.gray;
			_nomalButton.image.color = Color.white;
			_hardButton.image.color = Color.white;

			userDataManager.level = userDataManager.LEVEL.EASY;
			break;
		case userDataManager.LEVEL.NORMAL:

			_easyButton.image.color = Color.white;
			_nomalButton.image.color = Color.gray;
			_hardButton.image.color = Color.white;

			userDataManager.level = userDataManager.LEVEL.NORMAL;
			break;
		case userDataManager.LEVEL.HARD:

			_easyButton.image.color = Color.white;
			_nomalButton.image.color = Color.white;
			_hardButton.image.color = Color.gray;

			userDataManager.level = userDataManager.LEVEL.HARD;
			break;
		}
	}

	public void SetLevel(int level){

		ClickButton (level);
	}
}

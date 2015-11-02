using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FooterControl : MonoBehaviour {

	[SerializeField] private GameObject _homeButtonObj;
	[SerializeField] private GameObject _gameButtonObj;
	[SerializeField] private GameObject _friendButtonObj;
	[SerializeField] private GameObject _settingButtonObj;

	[SerializeField] private GameObject _multiSingleSelectPanelObj;
	[SerializeField] private GameObject _singleButtonObj;
	[SerializeField] private GameObject _multiButtonObj;

	[SerializeField] private VolumeSlider _bgmVolumeSlider;
	[SerializeField] private VolumeSlider _seVolumeSlider;

	private Vector3 _singleMultiButtonOriginVec;

	private enum KIND{
		Home,
		Game,
		Friend,
		Setting,
		Single,
		Multi
	}


	// Use this for initialization
	void Start () {
	
		Debug.Log (Application.loadedLevelName);

		_singleMultiButtonOriginVec = _singleButtonObj.transform.localPosition;

		_multiSingleSelectPanelObj.SetActive (false);

		string sceneName = Application.loadedLevelName;

		if (sceneName == "HomeScene") {

			_homeButtonObj.GetComponent<Button> ().enabled = false;
			setButtonColor (KIND.Home);
		} else if (sceneName == "FriendScene") {

			_friendButtonObj.GetComponent<Button> ().enabled = false;
			setButtonColor (KIND.Friend);
		} else if (sceneName == "SettingScene") {

			_settingButtonObj.GetComponent<Button> ().enabled = false;
			setButtonColor (KIND.Setting);
		}
	}


	private void setButtonColor(KIND kind){

		switch (kind) {

		case KIND.Home:

			_homeButtonObj.GetComponent<Image> ().color = new Color32 (168, 212, 221, 255);
			break;
		case KIND.Game:

			_gameButtonObj.GetComponent<Image> ().color = new Color32 (168, 212, 221, 255);
			break;
		case KIND.Friend:

			_friendButtonObj.GetComponent<Image> ().color = new Color32 (168, 212, 221, 255);
			break;
		case KIND.Setting:

			_settingButtonObj.GetComponent<Image> ().color = new Color32 (168, 212, 221, 255);
			break;
		}
	}

	public void FooterButton(int kind){

		if (Application.loadedLevelName == "SettingScene") {
			_bgmVolumeSlider.saveVolume ();
			_seVolumeSlider.saveVolume ();
		}

		switch ((KIND)kind) {

		case KIND.Home:

			Application.LoadLevel ("HomeScene");
			break;
		case KIND.Game:

//			Debug.Log ("GameScene制作中");
			Application.LoadLevel ("GameScene");
			break;
		case KIND.Single:

			Application.LoadLevel ("SingleScene");
			break;
		case KIND.Multi:

			Application.LoadLevel ("MultiScene");
			break;
		case KIND.Friend:

//			Debug.Log ("FriendScene製作中");
			Application.LoadLevel ("FriendScene");
			break;
		case KIND.Setting:

			Application.LoadLevel ("SettingScene");
			break;
		}
	}

	public void GameButton(){

		setButtonColor (KIND.Game);

		Vector3 multiButtonVec = new Vector3 (
			_singleMultiButtonOriginVec.x + 96f, 
			_singleMultiButtonOriginVec.y + 179f, 0
		);

		Vector3 singleBUttonVec = new Vector3 (
			_singleMultiButtonOriginVec.x - 96f, 
			_singleMultiButtonOriginVec.y + 179f, 0
		);

		_multiSingleSelectPanelObj.SetActive (true);

		iTween.MoveTo (_multiButtonObj, iTween.Hash (
			"x", multiButtonVec.x,
			"y", multiButtonVec.y,
			"time", 1f,
			"isLocal", true
		));

		iTween.MoveTo (_singleButtonObj, iTween.Hash (
			"x", singleBUttonVec.x,
			"y", singleBUttonVec.y,
			"time", 1f,
			"isLocal", true
		));
	}

	public void CloseGameButton(){

		_gameButtonObj.GetComponent<Image> ().color = new Color32 (255, 255, 255, 255);

		iTween.MoveTo (_multiButtonObj, iTween.Hash (
			"x", _singleMultiButtonOriginVec.x,
			"y", _singleMultiButtonOriginVec.y,
			"time", 0.5f,
			"isLocal", true
		));

		iTween.MoveTo (_singleButtonObj, iTween.Hash (
			"x", _singleMultiButtonOriginVec.x,
			"y", _singleMultiButtonOriginVec.y,
			"time", 0.5f,
			"isLocal", true,
			"oncomplete", "singleButtonMoveComplete",
			"oncompletetarget", gameObject
		));
	}

	private void singleButtonMoveComplete(){

		_multiSingleSelectPanelObj.SetActive (false);
	}
}

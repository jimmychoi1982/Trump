using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupManager : MonoBehaviour {

	[SerializeField] private GameObject _buttonPanelobj;              // ButtonPanel
	[SerializeField] private GameObject _waitForStartPopupobj;        // WaitForStartPopup

	[SerializeField] private CreateRoomPopupControl _createRoomPopupControl;          // CreateRoomPopupControl Script
	[SerializeField] private JoinRoomPopupControl _joinRoomPopupControl;            // JoinRoomPopupControl Script

	[SerializeField] private InputField _roomNameInputField;
	private string _roomName;
	private string _roomPassword; //次のバージョンを実装

	// Use this for initialization
	void Start () {

		_createRoomPopupControl.gameObject.SetActive (false);
		_joinRoomPopupControl.gameObject.SetActive (false);
		// 他シーンからMultiシーンへ遷移したとき
		iTween.ScaleTo (_buttonPanelobj, iTween.Hash ("x", 1, "y", 1, "time", 1.5f, "islocal", true));
	
	}

	public void Reset(){

		_createRoomPopupControl.gameObject.SetActive (false);
		_joinRoomPopupControl.gameObject.SetActive (false);
	}

	public void ClickCreateButton(){

		_createRoomPopupControl.Init ();
		_createRoomPopupControl.gameObject.SetActive (true);
		movePopup (_buttonPanelobj, _createRoomPopupControl.gameObject);
	}

	public void ClickJoinButton(){

		_joinRoomPopupControl.Init ();
		_joinRoomPopupControl.gameObject.SetActive (true);
		movePopup (_buttonPanelobj, _joinRoomPopupControl.gameObject);
	}

	public void movePopup(GameObject obj1, GameObject obj2){

		iTween.MoveTo (obj1, iTween.Hash ("x", -8));
		iTween.MoveTo (obj2, iTween.Hash ("x", 0));
	}

	public void backPopup(GameObject obj1, GameObject obj2){

		iTween.MoveTo (obj1, iTween.Hash ("x", 8));
		iTween.MoveTo (obj2, iTween.Hash ("x", 0));
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateRoomPopupControl : MonoBehaviour {

	[SerializeField] private PopupManager _popupManager;

	[SerializeField] private WaitForStartPopup _waitForStartPopup;   // WaitForChallengerPopup
	[SerializeField] private GameObject _buttonPanelobj;              // ButtonPanel

	[SerializeField] private GameObject _loadingPanelObj;

	[SerializeField] private InputField _roomNameInputField;

	[SerializeField] private LevelSelectButton _levelSelectButton;

	private string _roomName;
	private string _roomPassword; //次のバージョンを実装

	public void Init(){

		_levelSelectButton.SetLevel ((int)userDataManager.LEVEL.EASY);

		if (!PhotonNetwork.connected) {
			_loadingPanelObj.SetActive (true);
			// ロビーに入室
			PhotonNetwork.ConnectUsingSettings (null);
		}
	}

	public void CreateRoomButton(){

		createRoom (_roomNameInputField.text);
	}

	/// <summary>
	/// Room Input Field Valueを変更すれば、_roomNameを更新
	/// </summary>
	public void SetRoomName(){

		_roomName = _roomNameInputField.text;
	}

	/// <summary>
	/// Creates the room.次のバージョン実装する予定
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="password">Password.</param>
	private void createRoom(string name, string password){
	
		// TODO:
	}

	/// <summary>
	/// Creates the room.
	/// </summary>
	/// <param name="name">Name.</param>
	private void createRoom(string name){

		// ロビーに入室
		if (roomCheck ()) {
			PhotonNetwork.CreateRoom (_roomName);
		}
	}

	private void OnJoinedLobby (){

		Debug.Log ("Create Room Popup OnJoinedLobby");

		_loadingPanelObj.SetActive (false);
	}

	private void OnJoinedRoom(){

		Debug.Log (PhotonNetwork.playerName + " " + PhotonNetwork.room.name + " に入室しました ");

		userDataManager.multiState = userDataManager.MULTI_STATE.Master;
		_waitForStartPopup.Init ();

		_popupManager.movePopup (gameObject, _waitForStartPopup.gameObject);
	}

	/// <summary>
	/// Roomが存在しているかどうか
	/// </summary>
	/// <returns><c>true</c>, if check was roomed, <c>false</c> otherwise.</returns>
	private bool roomCheck(){

		foreach (var r in PhotonNetwork.GetRoomList()) {

			if (r.name == _roomName) {

				Debug.Log ("Roomが存在している");
				return false;
			}
		}

		return true;
	}

	public void BackButton(){

		userDataManager.multiState = userDataManager.MULTI_STATE.None;
		_roomNameInputField.text = "";
		_popupManager.backPopup (gameObject, _buttonPanelobj);
		_popupManager.Reset ();
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JoinRoomPopupControl : MonoBehaviour {

	[SerializeField] private Transform _contentTran;
	[SerializeField] private GameObject _roomButtonObj;

	[SerializeField] private GameObject _buttonPanelObj;
	[SerializeField] private GameObject _loadingPanelObj;

	[SerializeField] private WaitForStartPopup _waitForStartPopup;
	[SerializeField] private PopupManager _poopupManager;

	public void Init(){

		userDataManager.multiState = userDataManager.MULTI_STATE.Guest;

		if (!PhotonNetwork.connected) {

			_loadingPanelObj.SetActive (true);
			// ロビーに入室
			PhotonNetwork.ConnectUsingSettings (null);
		} else {

			UpdateRoomScollView ();
		}
	}
//
//	public void SetRoomName(string name){
//	
//		_roomName = name;
//	}
//
	public void BackButton(){
	
		userDataManager.multiState = userDataManager.MULTI_STATE.None;
		_poopupManager.backPopup (gameObject, _buttonPanelObj);
		_poopupManager.Reset ();
	}

	public void JoinRoomButton(string name){
	
		userDataManager.multiState = userDataManager.MULTI_STATE.Guest;

		if (name != "") {

			PhotonNetwork.JoinRoom (name); // OnJoinedRoom()を呼び出す
		} else {

			Debug.LogError ("Room name is none!!");
		}
	}

	private void OnJoinedLobby(){

		Debug.Log ("Join Room Popup OnJoinedLobby");

		UpdateRoomScollView ();
	}

	/// <summary>
	/// Updates the room scoll view.
	/// </summary>
	public void UpdateRoomScollView(){
	
		if (!_loadingPanelObj.activeSelf) {

			_loadingPanelObj.SetActive (true);
		}

		// ScrollViewを初期化
		foreach (Transform node in _contentTran) {

			Destroy (node.gameObject);
		}

		// Room Scrollviewを設定
		RoomInfo[] roomInfoList = PhotonNetwork.GetRoomList ();

		Debug.Log (roomInfoList.Length);

		foreach (var info in roomInfoList) {

			GameObject node = Instantiate (_roomButtonObj) as GameObject;
			node.transform.SetParent (_contentTran);
			node.transform.localScale = new Vector3 (1, 1, 1);

			node.GetComponent<RoomButton> ().SetRoomButton (info.name);
		}

		Debug.Log ("Room ScrollView Update 完了");

		_loadingPanelObj.SetActive (false);
	}

	private void OnJoinedRoom(){
	
		_poopupManager.movePopup (gameObject, _waitForStartPopup.gameObject);
		_waitForStartPopup.Init ();
//		_roomName = "";
	}


}

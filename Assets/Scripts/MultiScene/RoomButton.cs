using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomButton : MonoBehaviour {

	[SerializeField] private Text _roomNameText;
	[SerializeField] private Image _userPhotoImage;

	private const string JoinRoomPopupPath = "Canvas/JoinRoomPopup";
	private JoinRoomPopupControl _joinRoomPopupControl;

	void Start(){

		_joinRoomPopupControl = GameObject.Find (JoinRoomPopupPath).GetComponent<JoinRoomPopupControl> ();
	}

	/// <summary>
	/// Sets the room button.次のバージョンを実装する予定
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="iconIndex">Icon index.</param>
	public void SetRoomButton (string name, int iconIndex){

		// TODO:
	}

	public void SetRoomButton (string name){

		_roomNameText.text = name;
	}

	public void OnClick(){

		_joinRoomPopupControl.JoinRoomButton (_roomNameText.text);
	}
}

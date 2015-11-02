using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaitForStartPopup : MonoBehaviour {

	[SerializeField] private PopupManager _popupManager;
	[SerializeField] private GameObject _createRoomPopupObj;
	[SerializeField] private GameObject _joinRoomPopupObj;

	[SerializeField] private Image _masterPhotoImage;
	[SerializeField] private Text _masterWinningPercentageText;

	[SerializeField] private Sprite[] _iconSprites;
	[SerializeField] private Sprite _iconNone;

	[SerializeField] private Image _guestPhotoImage;
	[SerializeField] private Text _guestWinningPercentageText;

	[SerializeField] private Button _startButton;

	[SerializeField] private PhotonView _myPhotoView;

	public void Init(){

		Debug.Log ("My multi state :" + userDataManager.multiState);

		if (userDataManager.multiState == userDataManager.MULTI_STATE.Master) {

			Debug.Log ( "Room name: " + PhotonNetwork.room.name + " level" + userDataManager.level + "has created");

			_startButton.gameObject.SetActive (true);
			setStartButtonEnable (false);
			setMasterInformation ();
			setGuestInformation (-1, "Waiting...");

		} else if (userDataManager.multiState == userDataManager.MULTI_STATE.Guest) {

			_startButton.gameObject.SetActive (false);
			setGuestInformation ();

			// 相手のGuest Informationを設定
			_myPhotoView.RPC (
				"setGuestInformation", 
				PhotonTargets.Others, 
				userDataManager.IconIndex, 
				_guestWinningPercentageText.text
			);
		} else {

			// TODO: Error Popup
			Debug.LogError ("userDataManager.multiState is None!!");
		}
	}

	private void setStartButtonEnable(bool enabled){
	
		_startButton.enabled = enabled;

		if (enabled) {
		
			_startButton.image.color = Color.white;
		} else {

			_startButton.image.color = Color.gray;
		}
	}

	public void BackButton(){

		if (userDataManager.multiState == userDataManager.MULTI_STATE.Master) {

			_myPhotoView.RPC ("closeRoom", PhotonTargets.Others);

		} else if (userDataManager.multiState == userDataManager.MULTI_STATE.Guest) {

			// 相手のGuest Informationを設定
			_myPhotoView.RPC (
				"setGuestInformation", 
				PhotonTargets.Others, 
				-1, 
				"Waiting..."
			);
		}

		Debug.Log (PhotonNetwork.room.name + " から退室します");

		PhotonNetwork.LeaveRoom ();
	}

	/// <summary>
	/// Master退室の時、Roomを閉じる、Guestが部屋探すPopupに戻る
	/// </summary>
	[PunRPC]
	private void closeRoom(){
			
		_popupManager.backPopup (gameObject, _joinRoomPopupObj);
	}

	void OnLeftRoom(){
	
		if (userDataManager.multiState == userDataManager.MULTI_STATE.Master) {

			_popupManager.backPopup (gameObject, _createRoomPopupObj);

		} else if (userDataManager.multiState == userDataManager.MULTI_STATE.Guest) {

			_popupManager.backPopup (gameObject, _joinRoomPopupObj);
		}
	}
		
	private void setMasterInformation(){

		_masterPhotoImage.sprite = _iconSprites[userDataManager.IconIndex];

		float sum = userDataManager.winTime + userDataManager.loseTime;

		if (sum == 0) {
			_masterWinningPercentageText.text = "0%";
		} else {

			_masterWinningPercentageText.text = string.Format (
				"{0:0.00}%", 
				userDataManager.winTime / sum
			);
		}
	}

	private void OnPhotonPlayerConnected(){

		Debug.Log ("New player has joined!");

		// 新しい入室のPlayerのMaster informationを設定
		_myPhotoView.RPC (
			"setMasterInformation", 
			PhotonTargets.Others,
			userDataManager.IconIndex,
			_masterWinningPercentageText.text
		);

		// 新しい入室のPlayerの難易度を設定

		_myPhotoView.RPC (
			"setLevel",
			PhotonTargets.Others,
			(int)userDataManager.level
		);
			
		// Game start条件が揃ったら、Start Button有効化にする
		setStartButtonEnable (true);
	}

	private void setGuestInformation(){

		_guestPhotoImage.sprite = _iconSprites[userDataManager.IconIndex];

		string percentageString = "";

		float sum = userDataManager.winTime + userDataManager.loseTime;

		if (sum == 0) {
			_guestWinningPercentageText.text = "0%";
		} else {

			_guestWinningPercentageText.text = string.Format (
				"{0:0.00}%", 
				userDataManager.winTime / sum
			);
		}

		_myPhotoView.RPC ("setGuestInformation", PhotonTargets.Others, userDataManager.IconIndex, percentageString);
	}

	/// <summary>
	/// ゲーム難易度を設定
	/// </summary>
	/// <param name="level">Level.</param>
	[PunRPC]
	private void setLevel(int level){

		userDataManager.level = (userDataManager.LEVEL)level;
	}

	[PunRPC]
	private void setGuestInformation(int iconIndex, string percentage){

		if (iconIndex == -1) {

			_guestPhotoImage.sprite = _iconNone;
		} else {
		
			_guestPhotoImage.sprite = _iconSprites [userDataManager.IconIndex];
		}

		_guestWinningPercentageText.text = percentage;
	}

	[PunRPC]
	private void setMasterInformation(int iconIndex, string percentage){

		_masterPhotoImage.sprite = _iconSprites[userDataManager.IconIndex];
		_masterWinningPercentageText.text = percentage;
	}

	public void StartButton(){

		_myPhotoView.RPC ("startGame", PhotonTargets.All);
	}

	[PunRPC]
	private void startGame(){

		// Masterが先にMulti Game Sceneに入るのを確保
		if (userDataManager.multiState == userDataManager.MULTI_STATE.Master) {

			Application.LoadLevel ("GameScene_Multi");
		} else if (userDataManager.multiState == userDataManager.MULTI_STATE.Guest) {

			StartCoroutine (loadMultiGameScene());
		}
	}

	private IEnumerator loadMultiGameScene(){

		yield return new WaitForSeconds (0.05f);
		Application.LoadLevel ("GameScene_Multi");

	}
}

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;

public class MultiGameManager : MonoBehaviour
{

	[SerializeField] private Transform[] _cardParent; // index:0 easy, 1 normal, 2 hard
	[SerializeField] private GameObject _gameOverPanel;
	[SerializeField] private GameObject _loadingPanel;

	[SerializeField] private PhotonView _myPhotoView;

	[SerializeField] private GameObject _maskObj; // 画面操作無効化のMask
	[SerializeField] private MultiErrorPanel _multiErrorPanel; // ErrorのPopup

	[SerializeField] private GameObject _turnInformationPrefab; // TurnのInformation
	[SerializeField] private Transform _turnChangePanel;
	[SerializeField] private GameObject _countDownObj;
	[SerializeField] private Image _bgImage;

	[SerializeField] private Text _turnText;
	private string MY_TURN_TEXT = "My Turn";
	private string OTHER_PLAYER_TURN_TEXT = "Other Player Turn";

	// レベルのPair総数
	private const int PAIR_COUNT_EASY = 6;
	private const int PAIR_COUNT_NORMAL = 10;
	private const int PAIR_COUNT_HARD = 15;

	private List<GameObject> m_clickedCard = new List<GameObject>();

	public enum CHANGE_TURN_TYPE {
		YourTurn,
		OtherPlayerTurn
	}

	private bool _timerStart = false; // Time Count down開始
	private float _currenTime = 0f; // 今の行動時間
	private const float MAX_TIME = 10f; // 最大行動時間

	// Test用============================
	[SerializeField] private Text _text;
	[SerializeField] private Text _playerCount;
	[SerializeField] private Text _viewID;
	[SerializeField] private Text _currentTurn;
	//============================================

	private bool _gameStart = false; //ゲーム開始してるかどうか
	private bool _gameOver = false;

	private userDataManager.MULTI_STATE _currentMultiState; //今行動中State

	// Use this for initialization
	void Start (){

		// Loading panel
//		_loadingPanel.SetActive (true);

		// Count Downを初期化
		_currenTime = MAX_TIME;

		if (!PhotonNetwork.connected) {

			// ロビーに入室
			PhotonNetwork.ConnectUsingSettings (null);
		} 

		if (userDataManager.multiState == userDataManager.MULTI_STATE.Guest) {

			_myPhotoView.RPC ("guestIsReady", PhotonTargets.All);
		}
	}

	[PunRPC]
	private void guestIsReady(){

		_gameStart = true;
	}
		
	/** 削除予定
	private void OnJoinedLobby (){

		Debug.Log ("OnJoinedLobby");
		_text.text = "OnJoinedLobby";
		PhotonNetwork.JoinRandomRoom ();
	}
	**/

	/** 削除予定
	private void OnJoinedRoom(){

		if (_multiState == MULTI_STATE.None) {
			_multiState = MULTI_STATE.Guest;
		}

		Debug.Log ("入室しました " + _multiState);
		_text.text = "入室しました " + _multiState;

		//Debug
		_viewID.text = "viewID" + _myPhotoView.viewID.ToString ();
	}

	private void OnPhotonRandomJoinFailed (){

		Debug.Log ("OnPhotonRandomJoinFailed");
		_text.text = "OnPhotonRandomJoinFailed";

		PhotonNetwork.CreateRoom (null);

		_multiState = MULTI_STATE.Master;
	}
	**/
		
	/// <summary>
	/// Raises the photon player disconnected event.
	/// </summary>
	/// <param name="player">Player.</param>
	void OnPhotonPlayerDisconnected(PhotonPlayer player){

		Debug.Log ("Player :" + player.name + " 退室した！");

		_timerStart = false;

		if (!_gameOver) {
		
			_multiErrorPanel.SetPopup ("Error", "相手が退室しました！");
			_multiErrorPanel.gameObject.SetActive (true);
		}
	}

	void Update (){

		if (_gameStart) {

			_gameStart = false;
			gameStart ();
		}

		if (_timerStart) {

			_currenTime -= Time.deltaTime;

			float apha = _currenTime / MAX_TIME;

			if (apha > 0) {
				_bgImage.color = new Color (255, 255, 255, apha);
			}
		}
	}

	void FixedUpdate(){

		// Count down処理
		if (_currenTime > 0) {

			_countDownObj.GetComponent<Text> ().text = string.Format ("{0:00.00}s", _currenTime);
		} else if (_currenTime <= 0) {

			if (userDataManager.multiState == _currentMultiState) {
				_myPhotoView.RPC ("changeTurn", PhotonTargets.All);
			}
		}
	}

	private void gameStart(){

		Debug.Log ("Game start");
		_gameOverPanel.SetActive (false);

		init (); //カードを初期化
	}


	private void init (){

		Debug.Log ("Init game");

		_currenTime = MAX_TIME; // Count down初期化
		_currentMultiState = userDataManager.MULTI_STATE.Master;// MULTI_STATE.Master; //今のTurnを初期化

		if (userDataManager.multiState == userDataManager.MULTI_STATE.Master) {
			string[] layoutCardList = getLayoutCards ();

			int level = (int)userDataManager.level;

			for (int i = 0; i < _cardParent [level].childCount; ++i) {

				int kind = int.Parse (layoutCardList [i].Split ('_') [0]);
				int number = int.Parse (layoutCardList [i].Split ('_') [1]);

				_cardParent [level].GetChild (i).GetComponent <Card> ().setCard (number, kind);

				_cardParent [level].GetChild (i).GetComponent <Card> ().turnCardToBack ();
			}

			_cardParent [level].gameObject.SetActive (true);

			_myPhotoView.RPC ("setQuestLayoutCard", PhotonTargets.Others, layoutCardList, level);
		}

		// 最初Masterじゃない場合、行動できなくする、Count downしない
		if (userDataManager.multiState == userDataManager.MULTI_STATE.Guest) {
	
			_maskObj.SetActive (true);

			ShowTurnInformation (CHANGE_TURN_TYPE.OtherPlayerTurn);
			_turnText.text = OTHER_PLAYER_TURN_TEXT;
		} else { 

			// 自分最初がMatserの場合、行動する、Count down開始
			_turnChangePanel.gameObject.SetActive (true);
			ShowTurnInformation (CHANGE_TURN_TYPE.YourTurn);

			_turnText.text = MY_TURN_TEXT;
		}

		_loadingPanel.SetActive (false);
		_timerStart = true;
	}
		
	/// <summary>
	/// Sets the quest layout card list.
	/// </summary>
	/// <param name="layoutCardList">Layout card list.</param>
	/// <param name="level">Level.</param>
	[PunRPC]
	private void setQuestLayoutCard(string[] layoutCardList, int level){

		for (int i = 0; i < _cardParent [level].childCount; ++i) {

			int kind = int.Parse (layoutCardList [i].Split ('_') [0]);
			int number = int.Parse (layoutCardList [i].Split ('_') [1]);

			_cardParent [level].GetChild (i).GetComponent <Card> ().setCard (number, kind);

			_cardParent [level].GetChild (i).GetComponent <Card> ().turnCardToBack ();
		}

		_cardParent [level].gameObject.SetActive (true);
	}

	/// <summary>
	/// Gets the layout cards dictionary
	/// </summary>
	/// <returns>The layout cards.</returns>
	private string[] getLayoutCards(){

		int pairCount = 0;
		switch (userDataManager.level) {

		case userDataManager.LEVEL.EASY:

			pairCount = PAIR_COUNT_EASY;
			break;
		case userDataManager.LEVEL.NORMAL:

			pairCount = PAIR_COUNT_NORMAL;
			break;
		case userDataManager.LEVEL.HARD:

			pairCount = PAIR_COUNT_HARD;
			break;
		}

		List<string> cardTempList = new List<string> ();

		for (int i = 0; i < pairCount; ++i) {

			int number = UnityEngine.Random.Range (1, 14);
			int kind = UnityEngine.Random.Range (0, 4);

			cardTempList.Add (string.Format("{0}_{1}", kind, number));
			cardTempList.Add (string.Format("{0}_{1}", kind, number));
		}

		string[] cardList = cardTempList.OrderBy (i => Guid.NewGuid ()).ToArray ();

		return cardList;
	}

	public void Click(string index){

		_myPhotoView.RPC ("ClickCard", PhotonTargets.All, index);
	}

	[PunRPC]
	public void ClickCard (string index){

		if (_timerStart == false) {

			_timerStart = true;
		}

		if (m_clickedCard.Count < 2) {

			int parenIndex = (int)userDataManager.level;
			GameObject obj = GameObject.Find ("Canvas/" + _cardParent[parenIndex].name + "/" + index);
			StartCoroutine (obj.GetComponent<Card> ().turnCardToFront (0.1f));
			m_clickedCard.Add (obj);

			if (m_clickedCard.Count == 2) {

				// 間違う時
				if (m_clickedCard [0].GetComponent<Card> ().number != m_clickedCard [1].GetComponent<Card> ().number ||
				    m_clickedCard [0].GetComponent<Card> ().kind != m_clickedCard [1].GetComponent<Card> ().kind) {

					foreach (var c in m_clickedCard) {
					
						StartCoroutine (c.GetComponent <Card> ().turnCardToBack (0.5f));
					}
						
					// player turn変更
					changeTurn ();
				}

				m_clickedCard.Clear ();

				// 勝利処理
				if (isWin ()) {

					Debug.Log (userDataManager.multiState + " is Will");
					gameOver ();
				}				
			}
		}
	}
		
	/// <summary>
	/// Changes the turn.
	/// </summary>
	[PunRPC]
	private void changeTurn(){

		_timerStart = false;
		_currenTime = MAX_TIME;

		// player turn変更
		if (_currentMultiState == userDataManager.MULTI_STATE.Master){// MULTI_STATE.Master) {

			_currentMultiState = userDataManager.MULTI_STATE.Guest; //MULTI_STATE.Guest;
		} else if (_currentMultiState == userDataManager.MULTI_STATE.Guest) {

			_currentMultiState = userDataManager.MULTI_STATE.Master;
		}

		_currentTurn.text = "Current turn " + _currentMultiState;

		if (userDataManager.multiState != _currentMultiState) { //相手のTurnの場合

			ShowTurnInformation (CHANGE_TURN_TYPE.OtherPlayerTurn);
			_maskObj.SetActive (true);

			_turnText.text = OTHER_PLAYER_TURN_TEXT;
		} else { //自分のTurnの場合

			ShowTurnInformation (CHANGE_TURN_TYPE.YourTurn);
			_maskObj.SetActive (false);

			_turnText.text = MY_TURN_TEXT;
		}
			
		// クリックした正面側のカードが残っている場合、
		if (m_clickedCard.Count > 0) {

			foreach (var c in m_clickedCard) {

				StartCoroutine(c.GetComponent<Card> ().turnCardToBack (0.3f));
			}
		}
		_timerStart = true;
		_bgImage.color = new Color (255f, 255f, 255f, 1);
	}

	private bool isWin(){

		int index = (int)userDataManager.level;

		foreach (Transform t in _cardParent[index]) {

			if (m_clickedCard.Count > 0) {
				return false;
			} else if (t.GetComponent<Card> ().isBackSide ()) {
				return false;
			}
		}

		return true;
	}
		
	/// <summary>
	/// Game over.
	/// </summary>
	private void gameOver(){
	
		_gameOverPanel.SetActive (true);

		if (_currentMultiState == userDataManager.multiState) {

			// 勝つ時
			_gameOverPanel.GetComponent<GameOverPanel> ().SetPopup (
				"Congratulation! You Win!", true
			);

			userDataManager.winTime += 1;
		} else {

			// 負け時
			_gameOverPanel.GetComponent<GameOverPanel> ().SetPopup (
				"It's a pity, You Lose~~", false
			);
			userDataManager.loseTime += 1;
		}
			
		// 勝率を保存
		int total = userDataManager.winTime + userDataManager.loseTime;
		float percentage = (userDataManager.winTime / total) * 100;
		userDataManager.percentage = Mathf.FloorToInt (percentage);

		_gameOverPanel.GetComponent<GameOverPanel> ().StartAnimation ();
		_timerStart = false;
		_maskObj.SetActive (false);

		_gameOver = true;
	}

	/// <summary>
	/// Shows the information.
	/// </summary>
	/// <param name="type">Type.</param>
	private void ShowTurnInformation(CHANGE_TURN_TYPE type){

		_turnChangePanel.gameObject.SetActive (true);

		GameObject prefab = Instantiate (_turnInformationPrefab) as GameObject;
		prefab.transform.SetParent (_turnChangePanel);
		prefab.transform.localScale = new Vector3 (1, 1, 1);
		prefab.transform.localPosition = new Vector3 (630, 0, 0);

		prefab.GetComponent<TurnInformation> ().SetImage (type);
		prefab.GetComponent<TurnInformation> ().StartAnimation ();
	}

	public void BackButton(){

		Application.LoadLevel ("HomeScene");
	}

	public void OKButton(){

		PhotonNetwork.Disconnect ();
		Application.LoadLevel ("MultiResultScene");
	}
}

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Transform[] _cardParentTran; //index:0 easy, 1 normal, 2 hard
	[SerializeField] private GameOverPanel _gameOverPanel;


	private const int PAIR_COUNT_EASY = 6;
	private const int PAIR_COUNT_NORMAL = 10;
	private const int PAIR_COUNT_HARD = 15;

	private List<GameObject> m_clickedCard = new List<GameObject>();

	private bool _timerStart = false;
	private float _time = 0f;

	// Use this for initialization
	void Start (){

		_gameOverPanel.gameObject.SetActive (false);
		init (); //カードを初期化
	}
	// Update is called once per frame
	void Update (){
	
		if (_timerStart) {
			_time += Time.deltaTime;
//			Debug.Log (_time);
		}
	}

	private void init (){
	
		Debug.Log ("Init game");

		string[] layoutCardList = getLayoutCards ();

		int level = (int)userDataManager.level;

		for (int i = 0; i < _cardParentTran [level].childCount; ++i) {

			int kind = int.Parse (layoutCardList [i].Split ('_') [0]);
			int number = int.Parse (layoutCardList [i].Split ('_') [1]);

			_cardParentTran [level].GetChild (i).GetComponent <Card> ().setCard (number, kind);

			_cardParentTran [level].GetChild (i).GetComponent <Card> ().turnCardToBack ();
		}

		_cardParentTran [level].gameObject.SetActive (true);
		_timerStart = true;
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

	public void ClickCard (string index){

		if (_timerStart == false) {

			_timerStart = true;
		}

		if (m_clickedCard.Count < 2) {

			int parenIndex = (int)userDataManager.level;
			GameObject obj = GameObject.Find ("Canvas/" + _cardParentTran[parenIndex].name + "/" + index);
			obj.GetComponent<Card> ().turnCardToFront ();
			m_clickedCard.Add (obj);

			if (m_clickedCard.Count == 2) {

				if (m_clickedCard [0].GetComponent<Card> ().number != m_clickedCard [1].GetComponent<Card> ().number ||
				    m_clickedCard [0].GetComponent<Card> ().kind != m_clickedCard [1].GetComponent<Card> ().kind) {

					foreach (var c in m_clickedCard) {

						StartCoroutine (c.GetComponent <Card> ().turnCardToBack (0.5f));
					}
				}

				m_clickedCard.Clear ();

				if (isWin ()) {
					_gameOverPanel.gameObject.SetActive (true);
					_gameOverPanel.SetPopup (
						"Your Clear Time is: " + string.Format("{0:00.00}s", _time), true
					);
					_gameOverPanel.StartAnimation ();
					_timerStart = false;
				}
			}
		}
	}

	private bool isWin(){

		int index = (int)userDataManager.level;
		foreach (Transform t in _cardParentTran[index]) {

			if (m_clickedCard.Count > 0) {
				return false;
			} else if (t.GetComponent<Card> ().isBackSide ()) {
				return false;
			} 
		}

		return true;
	}

	public void ResetButton(){

		init ();
		_gameOverPanel.gameObject.SetActive (false);
	}

	public void EndButton(){
	
		Debug.Log (userDataManager.level + "is over clear time is : " + _time);

		switch (userDataManager.level) {

		case userDataManager.LEVEL.EASY:

			userDataManager.easyClearTime = (int)(_time * 1000); //クリア時間がmillisecondsで記録
			break;

		case userDataManager.LEVEL.NORMAL:

			userDataManager.normalClearTime = (int)(_time * 1000); //クリア時間がmillisecondsで記録
			break;

		case userDataManager.LEVEL.HARD:

			userDataManager.hardClearTime = (int)(_time * 1000); //クリア時間がmillisecondsで記録
			break;
		}

		KiiManagerMulti.SaveUserScopeData (() => {

			KiiManagerMulti.SaveApplicationScope (() => {

				Application.LoadLevel ("ResultScene");
			});
		});
		/**
		KiiBucket userBucket = KiiUser.CurrentUser.Bucket ("myBasicData");
		KiiQuery allQuery = new KiiQuery ();

		userBucket.Query (allQuery, (KiiQueryResult<KiiObject> result, Exception ex) => {

			if (ex != null){
				Debug.Log ("Connect error");
				return;
			}

			foreach (KiiObject obj in result){

				obj [clearTimeKind] = userDataManager.easyClearTime;
				obj.Save ((KiiObject savedObj, Exception ex2) => {

					if (ex2 != null){
						Debug.Log ("Connect error");
						return;
					}
						
					Application.LoadLevel ("ResultScene");
				});
			}
		});
		**/
	}
}

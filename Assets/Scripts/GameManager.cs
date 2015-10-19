using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Transform[] _cardParent; //index:0 easy, 1 normal, 2 hard
	[SerializeField] private GameObject GameOverPanel;


	private const int PAIR_COUNT_EASY = 6;
	private const int PAIR_COUNT_NORMAL = 10;
	private const int PAIR_COUNT_HARD = 15;

	private List<GameObject> m_clickedCard = new List<GameObject>();

	private bool _timerStart = false;
	private float _time = 0f;

	// Use this for initialization
	void Start (){

		GameOverPanel.SetActive (false);
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

		userDataManager.level = userDataManager.LEVEL.EASY; //Test

		// 難易度を取得
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

		// カードのGameObjectを取得
		List<GameObject> cardObjs = new List<GameObject> ();
		int index = (int)userDataManager.level;
		for (int i = 0; i < _cardParent[index].childCount; ++i) {

			cardObjs.Add (_cardParent[index].GetChild(i).gameObject);
		}

		// ペーアの形でカードをレイアウト
		for (int i = 0; i < pairCount; ++i) {

			int number = UnityEngine.Random.Range (1, 14);
			int kind = UnityEngine.Random.Range (0, 4);

			int cardIndex = UnityEngine.Random.Range (0, cardObjs.Count);

			cardObjs [cardIndex].GetComponent<Card> ().setCard (number, kind);
			cardObjs [cardIndex].GetComponent<Card> ().turnCardToBack();
			cardObjs.RemoveAt (cardIndex);

			cardIndex = UnityEngine.Random.Range (0, cardObjs.Count);
			cardObjs [cardIndex].GetComponent<Card> ().setCard (number, kind);
			cardObjs [cardIndex].GetComponent<Card> ().setCard (number, kind);
			cardObjs [cardIndex].GetComponent<Card> ().turnCardToBack();
			cardObjs.RemoveAt (cardIndex);
		}

		_cardParent [index].gameObject.SetActive (true);
	}

	public void ClickCard (string index){

		if (_timerStart == false) {

			_timerStart = true;
		}

		if (m_clickedCard.Count < 2) {

			int parenIndex = (int)userDataManager.level;
			GameObject obj = GameObject.Find ("Canvas/" + _cardParent[parenIndex].name + "/" + index);
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
					GameOverPanel.SetActive (true);
					GameOverPanel.GetComponent <GameOverPanel> ().SetInformationText (
						"Your Clear Time is: " + string.Format("{0:00.00}s", _time)
					);
					_timerStart = false;
				}
			}
		}
	}

	private bool isWin(){

		int index = (int)userDataManager.level;
		foreach (Transform t in _cardParent[index]) {

			if (m_clickedCard.Count > 0) {
				return false;
			}else if (t.GetComponent<Card> ().isBackSide()) {
				return false;
			}
		}

		return true;
	}

	public void ResetButton(){

		init ();
		GameOverPanel.SetActive (false);
	}

	public void EndButton(){
	
		userDataManager.time = (int)_time;

		KiiBucket userBucket = KiiUser.CurrentUser.Bucket ("myBasicData");
		KiiQuery allQuery = new KiiQuery ();

		userBucket.Query (allQuery, (KiiQueryResult<KiiObject> result, Exception ex) => {

			if (ex != null){
				Debug.Log ("Connect error");
				return;
			}

			foreach (KiiObject obj in result){

				obj ["time"] = userDataManager.time;
				obj.Save ((KiiObject savedObj, Exception ex2) => {

					if (ex2 != null){
						Debug.Log ("Connect error");
						return;
					}
						
					Application.LoadLevel ("ResultScene");
				});
			}
		});
	}
}

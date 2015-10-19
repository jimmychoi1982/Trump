using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	private const int PAIR_COUNT = 4;

	private List<GameObject> m_clickedCard = new List<GameObject>();

	[SerializeField]
	private Transform CardParent;

	[SerializeField]
	private GameObject GameOverPanel;

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
			Debug.Log (_time);
		}
	}

	private void init (){

		// カードのGameObjectを取得
		List<GameObject> cardObjs = new List<GameObject> ();

		for (int i = 0; i < CardParent.childCount; ++i) {

			cardObjs.Add (CardParent.GetChild(i).gameObject);
		}

		// ペーアの形でカードをレイアウト
		for (int i = 0; i < 6; ++i) {

			int number = UnityEngine.Random.Range (1, 14);
			int kind = UnityEngine.Random.Range (0, 4);

			int index = UnityEngine.Random.Range (0, cardObjs.Count);

			cardObjs [index].GetComponent<Card> ().setCard (number, kind);
			cardObjs [index].GetComponent<Card> ().turnCardToBack();
			cardObjs.RemoveAt (index);

			index = UnityEngine.Random.Range (0, cardObjs.Count);
			cardObjs [index].GetComponent<Card> ().setCard (number, kind);
			cardObjs [index].GetComponent<Card> ().setCard (number, kind);
			cardObjs [index].GetComponent<Card> ().turnCardToBack();
			cardObjs.RemoveAt (index);
		}
	}

	public void ClickCard (string index){

		if (_timerStart == false) {

			_timerStart = true;
		}

		if (m_clickedCard.Count < 2) {

			GameObject obj = GameObject.Find ("Canvas/Cards/" + index);
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

		foreach (Transform t in CardParent) {

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
		Application.LoadLevel ("ResultScene");
	}
}

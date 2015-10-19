using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

	[SerializeField]
	private GameObject cardObj;

	[SerializeField]
	private RectTransform canvas;

	[SerializeField]
	private Transform cardsPanel;

	private float m_screenWidth = 0;
	private float m_screenHeight = 0;

	// Use this for initialization
	void Start () {

		m_screenWidth = canvas.rect.width;
		m_screenHeight = canvas.rect.height;

		StartCoroutine (generateCards ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator generateCards(){
	
		while (true) {

			float posX = UnityEngine.Random.Range (-m_screenWidth / 2, m_screenWidth / 2);
			float posY = -m_screenHeight / 2;

			GameObject obj = Instantiate (cardObj);

			int number = UnityEngine.Random.Range (1, 14);
			int kind = UnityEngine.Random.Range (0, 4);

			obj.GetComponent <Card> ().setCard (number, kind);
			obj.GetComponent <Card> ().upDateCard();

			obj.transform.SetParent (cardsPanel);
			obj.transform.localPosition = new Vector2 (posX, posY);
			obj.transform.localScale = new Vector3 (1, 1, 1);

			yield return new WaitForSeconds (1f);
		}

		yield return null;
	}



	public void StartButton(){
	
		Application.LoadLevel ("GameScene");
	}

}

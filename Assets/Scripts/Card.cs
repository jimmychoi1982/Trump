using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour{

	public enum KIND{
		Spade,
		Heart,
		Club,
		Diamond
	}

	[SerializeField]
	private Image image;
	[SerializeField]
	private Sprite backSideSprite;
	[SerializeField]
	private Button button;
	[SerializeField]
	private Transform parent;

	private int _number; //カード自身の番号
	private int _kind; //カードのタイプ
	public bool _isBack;

	public int number {
		get{ return _number; }
		set{ _number = value; }
	}

	public int kind {
		get{ return _kind; }
		set{ _kind = value; }
	}

	// Use this for initialization
	void Start (){
	
		_isBack = true;
	}
	// Update is called once per frame
	void Update (){
	
	}

	private Sprite getCardImage (KIND kind, int number){

		string pathString = "";

		switch (kind) {

		case KIND.Spade:
			pathString = "s";
			break;
		case KIND.Heart:
			pathString = "h";
			break;
		case KIND.Club:
			pathString = "c";
			break;
		case KIND.Diamond:
			pathString = "d";
			break;
		}

		pathString += string.Format ("{0:00}", number);

		return Resources.Load<Sprite> ("Sprites/" + pathString);
	}

//	public void Click (){
//
//		if (image.sprite != backSideSprite) {
//
//			turnCardToBack ();
//		} else {
//
//			image.sprite = getCardImage ((KIND)_kind, _number);
//		}
//	}

	public void turnCardToBack (){

		_isBack = true;
		image.sprite = backSideSprite;
		button.enabled = true;
	}

	public IEnumerator turnCardToBack (float time){
	
		_isBack = true;

		yield return new WaitForSeconds (time);

		iTween.RotateTo (gameObject, iTween.Hash (
			"y", 90, 
			"time", time,
			"isLocal", true
		));

		yield return new WaitForSeconds (time);

		image.sprite = backSideSprite;
		button.enabled = true;

		iTween.RotateTo (gameObject, iTween.Hash (
			"y", 0, 
			"time", time,
			"isLocal", true
		));
	}

	public void turnCardToFront(){

		_isBack = false;
		image.sprite = getCardImage ((KIND)_kind, _number);
		button.enabled = false;
	}

	public IEnumerator turnCardToFront(float time){

		_isBack = false;

		iTween.RotateTo (gameObject, iTween.Hash (
			"y", 90, 
			"time", time,
			"isLocal", true
		));

		yield return new WaitForSeconds (time);

		image.sprite = getCardImage ((KIND)_kind, _number);
		button.enabled = false;

		iTween.RotateTo (gameObject, iTween.Hash (
			"y", 0, 
			"time", time,
			"isLocal", true
		));
	}

	public void setCard(int number, int kind){

		_number = number;
		_kind = kind;
	}

	public bool isBackSide(){
		return _isBack;
	}

	public void upDateCard(){
		image.sprite = getCardImage ((KIND)_kind, _number);
	}

	public void IsDown(){
		foreach (Transform t in parent) {
			if (t == transform) {
				continue;
			}

			t.GetComponent <Button> ().enabled = false;
		}
	}
}

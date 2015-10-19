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

	private int _number;
	//カード自身の番号
	private int _kind;
	//カードのタイプ
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

		image.sprite = backSideSprite;
		button.enabled = true;
	}

	public IEnumerator turnCardToBack (float time){
	
		yield return new WaitForSeconds (time);

		if (image.sprite != backSideSprite) {
			image.sprite = backSideSprite;
			button.enabled = true;
		}
	}

	public void turnCardToFront(){
		if (image.sprite == backSideSprite) {
			image.sprite = getCardImage ((KIND)_kind, _number);
			button.enabled = false;
		}
	}

	public void setCard(int number, int kind){

		_number = number;
		_kind = kind;
	}

	public bool isBackSide(){
		return image.sprite == backSideSprite;
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

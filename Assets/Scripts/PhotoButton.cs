using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PhotoButton : MonoBehaviour {

	[SerializeField] private Image _btnImage; // ボタンの画像

	private const string USERPHOTOVIEW = "Canvas/Popup/Photo select popup";  // Photo select popup までのPATH
	private GameObject _popupGameObject;                               // ゲームオブジェクトの箱

	// Use this for initialization
	void Start () {
	
		_popupGameObject = GameObject.Find (USERPHOTOVIEW);   // 取得したゲームオブジェクトを格納する
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Photo button が押された時
	public void clickPhotobutton() {
	
		Transform popuptransform = _popupGameObject.transform;
		Photoselectpopup pspopup = popuptransform.GetComponent<Photoselectpopup> ();

		int index = int.Parse (gameObject.name);
		pspopup.changeUserphotoview (_btnImage.sprite, index);  // User photo view を変更
	}

	// Photo Button の画像変更
	public void setSprite(Sprite sp) {

		_btnImage.sprite = sp;
	}
}

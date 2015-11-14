using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Photoselectpopup : MonoBehaviour {

	[SerializeField] private Image _userPhotoViewImage;                // User photo view
	[SerializeField] private GameObject _photoButtonGameObject;        // 自動生成する Photo button
	[SerializeField] private GameObject _contentGameObject;            // 自動生成した Photo button の親オブジェクト
	[SerializeField] private EditphotoAnimation _editPhotoGameObject;  // Edit photo
	[SerializeField] private Sprite[] _userpvSprite;

	[SerializeField] private SettingSceneManager _settingSceneManager; //Setting Scene Manager , 通信処理


	// Use this for initialization
	void Start () {

		// Profile panelを初期化
		_userPhotoViewImage.sprite = _userpvSprite[userDataManager.IconIndex];
		transform.localScale = new Vector3 (0, 0, 0);

		for (int i = 0; i < _userpvSprite.Length; i++) {

			GameObject obj = Instantiate (_photoButtonGameObject);     // Photo button を自動生成
			obj.transform.SetParent( _contentGameObject.transform );       // Photo button を content の子オブジェクトにする
			obj.transform.localScale = new Vector3 (1, 1, 1);          // Photo button のスケールを決める
			Transform objtransform = obj.transform;
			PhotoButton pb = objtransform.GetComponent<PhotoButton> ();
			pb.setSprite(_userpvSprite[i]);                            // Photo button の画像を設定

			obj.name = i.ToString (); // Index番号は名前にする
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Photo select button を押した時
	public void clickPhotoSelectButton(){

		float localX = transform.localScale.x;

		if (localX == 0) {

			transform.localScale = new Vector3 (1, 1, 1);
			_editPhotoGameObject.animationControl(true);
		} else{

			transform.localScale = new Vector3 (0, 0, 0);
			_editPhotoGameObject.animationControl(false);
		}
	}
			// User photo view を変更する
	public void changeUserphotoview(Sprite sprite, int index){

		clickPhotoSelectButton ();            // ポップアップを消す

		_userPhotoViewImage.sprite = sprite;  // User photo view の変更

		userDataManager.IconIndex = index;
		_settingSceneManager.SaveProfileText (); // 変更したIcon情報がサーバーに更新
	}
}

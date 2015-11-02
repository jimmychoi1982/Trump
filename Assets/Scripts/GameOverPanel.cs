using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverPanel : MonoBehaviour {

	[SerializeField] private Text _informationText;
	[SerializeField] private GameObject _popupObj;
	[SerializeField] private Image _image;

	[SerializeField] private Sprite _winSprite;
	[SerializeField] private Sprite _loseSprite;

	public void SetPopup(string text, bool isWin){

		_informationText.text = text;

		if (isWin) {

			_image.sprite = _winSprite;
		} else {

			_image.sprite = _loseSprite;
		}
	}

	public void StartAnimation(){
	
		// Popup初期化
		_popupObj.transform.localPosition = 
			new Vector3 (
				610f, _popupObj.transform.localPosition.y, 0f
			);

		iTween.MoveTo (_popupObj, iTween.Hash (
			"x", 0f, 
			"isLocal", true,
			"time", 0.5f
		));
	}
}

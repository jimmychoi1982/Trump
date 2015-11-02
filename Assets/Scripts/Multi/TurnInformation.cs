using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnInformation : MonoBehaviour {

	[SerializeField] private Image _image;

	[SerializeField] private Sprite _yourTurnImage;
	[SerializeField] private Sprite _otherPlayerTurnImage;

	public void SetImage(MultiGameManager.CHANGE_TURN_TYPE type){

		switch (type) {

		case MultiGameManager.CHANGE_TURN_TYPE.OtherPlayerTurn:

			_image.sprite = _otherPlayerTurnImage;
			break;
		case MultiGameManager.CHANGE_TURN_TYPE.YourTurn:

			_image.sprite = _yourTurnImage;
			break;
		}
	}

	public void StartAnimation(){

		iTween.MoveTo (gameObject, iTween.Hash(
			"x", 0,
			"time", 1f,
			"isLocal", true,
			"oncomplete", "moveCompleted"
//			"oncompletetarget", gameObject
		));
	}

	private void moveCompleted(){

		iTween.MoveTo (gameObject, iTween.Hash(
			"x", -630f,
			"time", 1f,
			"isLocal", true,
			"oncomplete", "animationCompleted"
		));
	}

	private void animationCompleted(){

		transform.parent.gameObject.SetActive (false);
		Destroy (gameObject);
	}
}

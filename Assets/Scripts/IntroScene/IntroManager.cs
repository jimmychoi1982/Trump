using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour {

	[SerializeField] private Animator _logoAnimator;

	[SerializeField] private GameObject _textObj;

	// Use this for initialization
	IEnumerator Start () {

		yield return new WaitForSeconds (2.0f);

		_textObj.SetActive (true);

		yield return new WaitForSeconds (1.8f);

		Application.LoadLevel ("TItleScene_Multi");
	}
}

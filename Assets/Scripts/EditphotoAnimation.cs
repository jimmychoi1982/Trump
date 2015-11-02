using UnityEngine;
using System.Collections;

public class EditphotoAnimation : MonoBehaviour {

	private Animator _anmAnimator;     // アニメーションのため

	// Use this for initialization
	void Start () {
	
		_anmAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Edit photo がクリックされた時のアニメーション
	public void animationControl(bool click){
		if (click) {
			_anmAnimator.SetBool ("click", true);
		} else {
			_anmAnimator.SetBool ("click", false);
		}
	}
}

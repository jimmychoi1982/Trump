using UnityEngine;
using System.Collections;

public class MoveScene : MonoBehaviour {

	[SerializeField] private GameObject _popupobj;

	// Use this for initialization
	void Start () {

		// 他シーンからMultiシーンへ遷移したとき
		iTween.ScaleTo (_popupobj, iTween.Hash ("x", 1, "y", 1, "time", 1.5f, "islocal", true));
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

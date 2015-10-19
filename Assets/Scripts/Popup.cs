using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {

	[SerializeField]
	private GameObject Panel_Obj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OkButton(){

		Panel_Obj.SetActive (false);
	}
}

using UnityEngine;
using System.Collections;

public class TitleCard : MonoBehaviour {

	private float MAX_POS_Y = 900f;
	// Use this for initialization
	void Start () {

		Destroy (gameObject, 10f);

		float speed = UnityEngine.Random.Range (20, 100);

		iTween.MoveTo (gameObject, iTween.Hash("y", MAX_POS_Y, "speed", speed, "isLocal", true));
	}
	
	// Update is called once per frame
	void Update () {
	}
}

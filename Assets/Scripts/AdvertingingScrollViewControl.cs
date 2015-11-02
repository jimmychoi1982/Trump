using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AdvertingingScrollViewControl : MonoBehaviour {

	[SerializeField] private GameObject _contentObj;
	[SerializeField] private RectTransform _contextTran;

	private GameObject[] _adNodeObj;

	private int _nodeCount = 3;
	private const float NODE_WIDTH = 500f;
	private float _spacing;

	private int _nodeIndex = 0;

	private bool _nodeIsMoving = false;

	// Use this for initialization
	void Start () {

		_spacing = _contentObj.GetComponent <HorizontalLayoutGroup> ().spacing;
		_nodeCount = _contentObj.transform.childCount;

		StartCoroutine (autoMoveNode ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	IEnumerator autoMoveNode(){

		while (true) {

			yield return new WaitForSeconds (3);
			if (_nodeIsMoving == false) {

				ToRightButton ();
			}
		}
	}

	private void moveToNextNode(){

		float x = -(NODE_WIDTH / 2 + (NODE_WIDTH + _spacing) * _nodeIndex);

		_nodeIsMoving = true;
		iTween.MoveTo (_contentObj, iTween.Hash (
			"x", x, 
			"time", 1f,
			"isLocal", true,
			"oncomplete", "nodeMoveOncomplete",
			"oncompletetarget", gameObject
		));
	}

	private void nodeMoveOncomplete(){

		_nodeIsMoving = false;
	}

	public void ToLeftButton(){
	
		_nodeIndex--;
		if (_nodeIndex < 0) {
			_nodeIndex = _nodeCount - 1;
		}
		moveToNextNode ();
	}

	public void ToRightButton(){

		_nodeIndex++;
		if (_nodeIndex > _nodeCount - 1) {
			_nodeIndex = 0;
		}
		moveToNextNode ();
	}
}

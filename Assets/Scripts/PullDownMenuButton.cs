using UnityEngine;
using System.Collections;

public class PullDownMenuButton : MonoBehaviour {

	[SerializeField] private GameObject _pulldownMenuObj;

	public void OpenPullDownMenu(){

		_pulldownMenuObj.SetActive (true);
	}

	public void ClosePullDownMenu(){

		_pulldownMenuObj.SetActive (false);
	}
}

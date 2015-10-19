using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Node : MonoBehaviour {

	[SerializeField] private Text _userNameText;
	[SerializeField] private Text _timeText;

	public void setNode (string userName, int time){

		_userNameText.text = userName;
		_timeText.text = time.ToString() + "ms";
	}
}

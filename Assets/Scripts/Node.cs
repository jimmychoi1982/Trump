using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Node : MonoBehaviour {

	[SerializeField] private Text _userIDText;
	[SerializeField] private Text _timeText;

	public void setNode (string userID, int time){

		_userIDText.text = userID;
		_timeText.text = time.ToString();
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverPanel : MonoBehaviour {

	[SerializeField]
	private Text _informationText;

	public void SetInformationText(string text){

		_informationText.text = text;
	}
}

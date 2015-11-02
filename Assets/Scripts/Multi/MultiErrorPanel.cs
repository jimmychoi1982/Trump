using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiErrorPanel : MonoBehaviour {

	[SerializeField] private Text _titleText;
	[SerializeField] private Text _informationText;

	public void OKButton(){

		PhotonNetwork.LeaveRoom ();
	}
		
	private void OnLeftRoom(){
	
		Application.LoadLevel ("HomeScene");
	}

	/// <summary>
	/// Sets the popup.
	/// </summary>
	/// <param name="titleText">Title text.</param>
	/// <param name="informationText">Information text.</param>
	public void SetPopup(string titleText, string informationText){

		_titleText.text = titleText;
		_informationText.text = informationText;
	}
}

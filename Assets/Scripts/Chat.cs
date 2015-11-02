using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Chat : MonoBehaviour {

	[SerializeField] private CreateMessage _cmsgCreateMessage;  // メッセージを生成するクラス
	[SerializeField] private InputField _ipfInputField;
	[SerializeField] private Text _mySendMessageText;           // InputFieldに入力されたメッセージ

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Sendボタンを押す
	public void clickSendButton(){

		if (_mySendMessageText.text != "") {
			_cmsgCreateMessage.addMessage (_mySendMessageText.text);  // メッセージの追加
			_ipfInputField.text = "";   // InputFieldの初期値
		}
	}
}

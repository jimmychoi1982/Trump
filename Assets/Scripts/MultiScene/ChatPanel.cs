using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour {

	[SerializeField] private PhotonView _myPhotoView;

	[SerializeField] private InputField _inputField;

	[SerializeField] private GameObject _myMessage;  // 自分発言の表示用Prefab
	[SerializeField] private GameObject _otherPlayerMessage; //別人発言の表示用Prefab
	[SerializeField] private Transform _contentTran;     // 追加するメッセージの親オブジェクト

	[SerializeField] private List<GameObject> _messageList;        // メッセージの数を管理する配列

	private const int _MAXMESSAGE = 10;          // メッセージを残す最大数

	void Start(){

		_messageList = new List<GameObject> ();
	}

	/// <summary>
	/// メッセージを表示
	/// </summary>
	/// <param name="message">Message.</param>
	public void addMyMessage(string message){

		// メッセージが10個あるとき一番古いメッセージを消して、新しいものを追加する
		if (_messageList.Count == _MAXMESSAGE) {

			Destroy (_messageList [0].gameObject);
			_messageList.RemoveAt (0);
		}

		_myMessage.transform.FindChild ("Text").GetComponent <Text> ().text = "My: " + message;

		GameObject obj = Instantiate (_myMessage);         // メッセージ Prefabを生成
		obj.transform.SetParent (_contentTran);       // content の子オブジェクトにする
		obj.transform.localScale = new Vector3 (1, 1, 1);
		_messageList.Add (obj);

		// Message Viewを更新
		_myPhotoView.RPC ("updateMessageView", PhotonTargets.Others, message);
	}

	/// <summary>
	/// MessageView更新
	/// </summary>
	[PunRPC]
	private void updateMessageView(string message){

		// メッセージが10個あるとき一番古いメッセージを消して、新しいものを追加する
		if (_messageList.Count == 10) {

			Destroy (_messageList [0].gameObject);
			_messageList.RemoveAt (0);
		}
			
		_otherPlayerMessage.transform.FindChild ("Text").GetComponent <Text> ().text = "Other: " + message;

		GameObject obj = Instantiate (_otherPlayerMessage);         // メッセージを生成
		obj.transform.SetParent (_contentTran);       // content の子オブジェクトにする
		obj.transform.localScale = new Vector3 (1, 1, 1);
		_messageList.Add (obj);
	}

	public void SendButton(){
	
		addMyMessage (_inputField.text);
	}
}

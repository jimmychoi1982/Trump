using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateMessage : MonoBehaviour {

	[SerializeField] private Text _mySentMessageText;    // 追加するメッセージ
	[SerializeField] private GameObject _addMessageobj;  // シーンに追加するメッセージプレハブ
	[SerializeField] private GameObject _contentobj;     // 追加するメッセージの親オブジェクト

	[SerializeField] private GameObject[] _sentMessageobj;        // メッセージの数を管理する配列
	private const int _MAXMESSAGE = 10;          // メッセージを残す最大数
	private int _currentMessageNumber;           // 現在メッセージがいくつあるか

	// Use this for initialization
	void Start () {

		_currentMessageNumber = 2;        // メッセージ例の分
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// メッセージの追加
	public void addMessage(string message){

		// メッセージが10個あるとき一番古いメッセージを消して、新しいものを追加する
		if (_currentMessageNumber >= _MAXMESSAGE) {
			Destroy (_sentMessageobj [0]);
			for (int i = 0; i < (_sentMessageobj.Length - 1); i++) {
				_sentMessageobj [i] = _sentMessageobj [i + 1];
			}
		}

		if (_currentMessageNumber < _MAXMESSAGE) {
			_currentMessageNumber++;
		}

		_mySentMessageText.text = " My: " + message;
		GameObject obj = Instantiate (_addMessageobj);         // メッセージを生成
		obj.transform.SetParent (_contentobj.transform);       // content の子オブジェクトにする
		obj.transform.localScale = new Vector3 (1, 1, 1);
		_sentMessageobj [_currentMessageNumber - 1] = obj;     // 配列にメッセージオブジェクトを格納
	}
}

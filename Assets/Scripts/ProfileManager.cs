using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour {

	[SerializeField] private InputField _nameInputField;
	[SerializeField] private InputField _greetInputField;

	[SerializeField] private GameObject _loadingPrefab;

	private enum TEXT_KIND
	{
		Name,
		Greet
	}

	// Use this for initialization
	void Start () {
	
		initInputFieldText ();
	}

	/// <summary>
	/// 初始化 Input Field显示的文字
	/// </summary>
	private void initInputFieldText(){

		_nameInputField.text = userDataManager.userName;
		_greetInputField.text = userDataManager.greet;
	}

	// 変更されたときに userDataManager へ値を渡しておく
	public void newInputText(int newText) {

		switch ((TEXT_KIND)newText) {

		case TEXT_KIND.Name:

			userDataManager.userName = _nameInputField.text;
			Debug.Log (_nameInputField.text);
			break;
		case TEXT_KIND.Greet:

			userDataManager.greet = _greetInputField.text;
			Debug.Log (_greetInputField.text);
			break;
		}

		saveProfileText ();         // 保存
	}

	// 変更内容を保存する
	private void saveProfileText(){
	
		Debug.Log(userDataManager.userName + " " + userDataManager.greet);

		_loadingPrefab.SetActive (true);

		KiiManagerMulti.SaveUserScopeData (() => {

			KiiManagerMulti.SaveApplicationScope (() => {
			
				_loadingPrefab.SetActive (false);
			});
		});
	}
}

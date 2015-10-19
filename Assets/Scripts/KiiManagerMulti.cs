using UnityEngine;
using System.Collections;
using System;
using JsonOrg;
using KiiCorp.Cloud.Storage;
using UnityEngine.UI;
using System.Collections.Generic;
using Aiming.IteratorTasks;

public class KiiManagerMulti : MonoBehaviour
{
	[SerializeField] private GameObject _registerPanelObj;
	[SerializeField] private Text _infomationText;
	[SerializeField] private Text _userNameText;
	[SerializeField] private GameObject _loadingPanelObj;

	[SerializeField] private InputField _userNameInputField;
	[SerializeField] private Transform _inputPanelTransform;


	private string _userName; //TODO
	private bool _isNewUser = false;
//	private bool _loading = false;

	void Start ()
	{
		DontDestroyOnLoad (gameObject);

		string kiiUserID = "";
		string kiiPassword = "";

		// 今端末がUser情報があるかどうか
		if (PlayerPrefs.HasKey ("UserID")) {

			kiiUserID = PlayerPrefs.GetString ("UserID");
			kiiPassword = PlayerPrefs.GetString ("UserPW");

			Debug.Log ("Loading");
			_loadingPanelObj.SetActive (true);
			LoginUser (kiiUserID, kiiPassword);
		} else {

			kiiUserID = RandomCodeGenerate (10);
			kiiPassword = RandomCodeGenerate (6);

			_isNewUser = true;
			Debug.Log ("Loading");
			_loadingPanelObj.SetActive (true);
			RegistUser (kiiUserID, kiiPassword);
		}

	}
	// Update is called once per frame
	void Update ()
	{
	}

	/// <summary>
	/// ランダムの文字列を取得
	/// </summary>
	/// <returns>The code generate.</returns>
	/// <param name="codeLength">Code length.</param>
	public string RandomCodeGenerate (int codeLength)
	{

		string allCode = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWYZ";
		string outPutCode = "";
		for (int i = 0; i < codeLength; ++i) {

			int rndTmp = UnityEngine.Random.Range (0, allCode.Length);
			outPutCode += allCode.Substring (rndTmp, 1);
		}

		return outPutCode;
	}

	/// <summary>
	/// 入力されたユーザーID, passwordのユーザーが存在しなければ新規作成
	/// </summary>
	/// <returns><c>true</c>, if user was registed, <c>false</c> otherwise.</returns>
	/// <param name="userName">User name.</param>
	/// <param name="password">Password.</param>
	public void RegistUser (string userID, string userPassword)
	{

		// ユーザー名とPasswordが有効かどうか
		if (!KiiUser.IsValidUserName (userID) || !KiiUser.IsValidPassword (userPassword)) {

			Debug.Log ("Failed user regist:" + userID + ":");
			return;
		}

		KiiUser.Builder builder = KiiUser.BuilderWithName (userID);
		KiiUser user = builder.Build ();

		user.Register (userPassword, (KiiUser registeredUser, Exception e) => {

			if (e != null) {
			
				Debug.LogError ("Failed user regist :" + userID + " : " + e);

				return;
			} else {

				Debug.Log ("Success userName RegistUser : " + userID);

				LoginUser (userID, userPassword);
			}
		});
	}

	/// <summary>
	/// ログイン
	/// </summary>
	/// <returns><c>true</c>, if user was logined, <c>false</c> otherwise.</returns>
	/// <param name="userName">User name.</param>
	/// <param name="password">Password.</param>
	public void LoginUser (string userID, string userPassword)
	{
		KiiUser.LogIn (userID, userPassword, (KiiUser user, Exception ex) => {

			if (ex != null) {
				// Error処理
				return;
			}
		
			Debug.Log ("Login Complete UserID:" + userID + ":" + userPassword);

			kiiDataInitialize (userID, userPassword);
		});
	}

	/// <summary>
	/// Kiiデータ初期化 廃棄予定
	/// </summary>
	/// <returns><c>true</c>, if data initialize was kiied, <c>false</c> otherwise.</returns>
	public void kiiDataInitialize (string userID, string userPassword)
	{
		if (_isNewUser) {
			// ユーザーのパケットを定義
			KiiBucket userBucket = KiiUser.CurrentUser.Bucket ("myBasicData");
			KiiObject basicDataObj = userBucket.NewKiiObject ();

			// 保存データ定義
			basicDataObj ["time"] = 0;
			basicDataObj ["userName"] = "";

			basicDataObj ["userID"] = userID;
			basicDataObj ["userPW"] = userPassword;


			basicDataObj.Save ((KiiObject obj, Exception ex) => {

				if (ex != null) {
					// TODO Error処理
					Debug.Log ("Connect Error");
				}

				Debug.Log ("Kii data initialize completed");

				// Wait for user name comfirm : ConfirmUserName()
				_loadingPanelObj.SetActive (false);
				_inputPanelTransform.gameObject.SetActive (true);
			});
		} else {

			loadKiiData ();
		}
	}

	/// <summary>
	/// 保存されているデータを読み込み
	/// </summary>
	/// <returns><c>true</c>, if kii data was loaded, <c>false</c> otherwise.</returns>
	public void loadKiiData ()
	{

//		while (KiiUser.CurrentUser == null) {
//			Debug.Log ("Current user is not found");
//		}

		Debug.Log (KiiUser.CurrentUser.Bucket ("myBasicData"));

		// Kii的搜索功能
		KiiQuery allQuery = new KiiQuery ();

		KiiUser.CurrentUser.Bucket ("myBasicData").Query (allQuery, (KiiQueryResult<KiiObject> result, Exception ex) => {

			if (ex != null) {
			
				Debug.Log (ex);
				return;
			}

			foreach (KiiObject obj in result) {

				userDataManager.time = (int)obj ["time"];
				userDataManager.userName = (string)obj ["userName"];
				userDataManager.userID = (string)obj ["userID"];
				userDataManager.userPW = (string)obj ["userPW"];

				Debug.Log ("Load Kii data has completed");
			}
				
			_userNameText.text = userDataManager.userName;
			_loadingPanelObj.SetActive (false);

			PlayerPrefs.SetString ("UserID", userDataManager.userID);
			PlayerPrefs.SetString ("UserPW", userDataManager.userPW);

			_isNewUser = false;

			Debug.Log ("Load complete!");
		});
	}

	/// <summary>
	/// 現在のデータを保存
	/// </summary>
	/// <returns><c>true</c>, if kii data was saved, <c>false</c> otherwise.</returns>
	public void SaveUserScopeData ()
	{

		// Kii的搜索功能
		KiiQuery allQuery = new KiiQuery ();

		KiiUser.CurrentUser.Bucket ("myBasicData").Query (allQuery, (KiiQueryResult<KiiObject> result, Exception ex) => {

			if (ex != null) {

				return;
			}

			foreach (KiiObject obj in result) {

				obj ["time"] = userDataManager.time;
				obj.Save ((KiiObject saveObj, Exception e) => {

					if (e != null){

						return;
					}

					Debug.Log ("Save completed");
				});
			}
		});
	}

	public void SaveApplicationScope(){

		KiiQuery allQuery = new KiiQuery ();

		string userID = PlayerPrefs.GetString ("UserID");

		Kii.Bucket ("Ranking").Query (allQuery, (KiiQueryResult<KiiObject> result, Exception ex) => {

			if (ex != null){
				Debug.Log ("Connect error");
			}
				
			foreach (KiiObject obj in result){
				if (obj["userID"].ToString() == userID){
					obj["time"] = userDataManager.time;

					Debug.Log ("Save Application Scope is complete::" + userID + ":: " + userDataManager.time);
				}
			}
		});
	}

	private void initializeApplicationScope(){
	
		KiiObject obj = Kii.Bucket ("Ranking").NewKiiObject ();

		obj ["userName"] = _userName;
		obj ["time"] = 0;

		obj.Save ((KiiObject savedObj, Exception ex) => {
			if (ex != null){
				Debug.Log ("Connect error");
			}

			Debug.Log ("Initialize Application Scope is complete");
			loadKiiData ();
		});
	}

	/// <summary>
	/// ユーザー情報をクリアする（端末内）
	/// </summary>
	public void ResetButton ()
	{

		PlayerPrefs.DeleteAll ();
	}

//	public void ShowUserRank(){
//	
//		KiiQuery allQuery = new KiiQuery ();
//		allQuery.SortByDesc ("userID"); //按指定字段降序排列。
//
//		string userID = "";
//		int time = 0;
//
//		Kii.Bucket ("Ranking").Query (allQuery, (KiiQueryResult<KiiObject> result, Exception ex) => {
//
//			if (ex != null){
//				Debug.Log ("Connect error");
//			}
//
//			foreach (KiiObject obj in result){
//
//				userID = obj["userID"].ToString();
//				time = (int)obj["time"];
//
//				Debug.Log (obj["userID"].ToString() + ":: " + (int)obj["time"]);
//
////				GameObject.Find ("ResultManager").GetComponent <ResultManager>().SetScollView(userID, time);
//			}
//
//			Debug.Log ("Show complete");
//		});
//	}

	/// <summary>
	/// Sets the name of the user.
	/// </summary>
	public void SetUserName(){
		_userName = _userNameInputField.text;
	}

	/// <summary>
	/// Confirms the name of the user.
	/// </summary>
	public void ConfirmUserName(){

		_loadingPanelObj.SetActive (true);

		KiiBucket userBucket = KiiUser.CurrentUser.Bucket ("myBasicData");
		KiiQuery allQuery = new KiiQuery ();

		userBucket.Query (allQuery, (KiiQueryResult<KiiObject> result, Exception ex) => {

			if (ex != null){
				Debug.Log ("Connect error");
				return;
			}

			Debug.Log (_userName);
			foreach (KiiObject obj in result){
				obj ["userName"] = _userName;

				obj.Save((KiiObject savedObj, Exception ex2) => {

					if (ex != null){
						Debug.Log ("Connect error");
						return;
					}

					initializeApplicationScope ();
					return;
				});
			}
		});
	}
}

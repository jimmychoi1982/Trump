using UnityEngine;
using System.Collections;
using System;
using JsonOrg;
using KiiCorp.Cloud.Storage;
using UnityEngine.UI;

public class KiiManager : MonoBehaviour
{
	[SerializeField]
	private GameObject _registerPanelObj;
	[SerializeField]
	private Text _infomationText;
	private string _kiiUserID;
	private string _kiiPassword;
	private string _userName;
	private bool _isLoading = false;
	// Use this for initialization
	void Start ()
	{

		// 今端末がUser情報があるかどうか
		if (PlayerPrefs.HasKey ("userID")) {

			_kiiUserID = PlayerPrefs.GetString ("userID");
			_kiiPassword = PlayerPrefs.GetString ("userPW");
		} else {

			_kiiUserID = randomCodeGenerate (10);
			_kiiPassword = randomCodeGenerate (6);
		}

		bool registCheck = RegistUser (_kiiUserID, _kiiPassword);

		if (registCheck) { // 新規ユーザー成功
		
			bool loginCheck = loginUser (_kiiUserID, _kiiPassword);
			if (loginCheck) {

				bool initData = kiiDataInitialize ();
				if (!initData) {

					initData = kiiDataInitialize ();
				}

				if (!initData) {

					Debug.LogError ("保存失败");
					return;
				}

				if (initData) {

					PlayerPrefs.SetString ("userID", _kiiUserID);
					PlayerPrefs.SetString ("userPW", _kiiPassword);

					_infomationText.text = "Thank you for Resister!";
					_registerPanelObj.SetActive (true);
				}
			} else {
				Debug.LogError ("ログイン失败");
				return;
			}
		} else { // 新規ユーザー失敗

//			statusText.text = "Loading server data";

			// ログインチェック
			bool loginCheck = loginUser (_kiiUserID, _kiiPassword);
			if (loginCheck) { // ログイン成功

				bool loadDataCheck = loadKiiData ();
				if (!loadDataCheck) {

					loadDataCheck = loadKiiData ();
				}

				if (!loadDataCheck) {

					Debug.LogError ("读取数据失败");
					return;
				}
				if (loadDataCheck) {

					_infomationText.text = "Welcome back " + _kiiUserID;
					_registerPanelObj.SetActive (true);
					Debug.Log ("ログイン成功");
				}

			} else { // ログイン失敗
				Debug.LogError ("ログイン失敗");
			}
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
	string randomCodeGenerate (int codeLength)
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
	public bool RegistUser (string userName, string password)
	{

		bool success = false;

		// ユーザー名とPasswordが有効かどうか
		if (!KiiUser.IsValidUserName (userName) || !KiiUser.IsValidPassword (password)) {

			Debug.Log ("Failed user regist:" + userName + ":");
			return false;
		}

		KiiUser.Builder builder = KiiUser.BuilderWithName (userName);
		KiiUser user = builder.Build ();

//		try{
//
//			user.Register (password);
//			Debug.Log ("Success userName RegistUser : " + userName);
//		}
//		catch (System.Exception exception){
//
//			Debug.Log ("Failed user regist :" + userName + " : " + exception);
//			user = null;
//			return false;
//		}

		user.Register (_kiiPassword, (KiiUser registeredUser, Exception e) => {

			if (e != null) {

				success = false;
				Debug.LogError ("Failed user regist :" + userName + " : " + e);
				return;
			} else {

				success = true;
				Debug.Log ("Success userName RegistUser : " + userName);
				return;
			}
		});


		Debug.Log (success);
		return success;
	}

	/// <summary>
	/// ログイン
	/// </summary>
	/// <returns><c>true</c>, if user was logined, <c>false</c> otherwise.</returns>
	/// <param name="userName">User name.</param>
	/// <param name="password">Password.</param>
	public bool loginUser (string userName, string password)
	{

		KiiUser user;

		try {

			user = KiiUser.LogIn (userName, password);
			Debug.Log ("Success user login : " + userName);
		} catch (System.Exception exception) {

			Debug.LogError ("Failed user login : " + userName + " : " + exception);
			user = null;
			return false;
		}

		return true;
	}


	/// <summary>
	/// Kiiデータ初期化
	/// </summary>
	/// <returns><c>true</c>, if data initialize was kiied, <c>false</c> otherwise.</returns>
	bool kiiDataInitialize ()
	{

		// ユーザーのパケットを定義
		KiiBucket userBucket = KiiUser.CurrentUser.Bucket ("myBasicData");
		KiiObject basicDataObj = userBucket.NewKiiObject ();

		// 保存データ定義
		basicDataObj ["time"] = 0;

		// オプジェクト保存
		try {

			basicDataObj.Save ();
		} catch (System.Exception exception) {

			Debug.LogError (exception);
			return false;
		}

		return true;
	}

	/// <summary>
	/// 保存されているデータを読み込み
	/// </summary>
	/// <returns><c>true</c>, if kii data was loaded, <c>false</c> otherwise.</returns>
	bool loadKiiData ()
	{

		// Kii的搜索功能
		KiiQuery allQuery = new KiiQuery ();
		try {

			KiiQueryResult<KiiObject> result = KiiUser.CurrentUser.Bucket ("myBasicData").Query (allQuery);

			foreach (KiiObject obj in result) {

				userDataManager.time = (int)obj ["time"];
			}
		} catch (System.Exception exception) {

			Debug.Log (exception);
			return false;
		}

		return true;
	}

	/// <summary>
	/// 現在のデータを保存
	/// </summary>
	/// <returns><c>true</c>, if kii data was saved, <c>false</c> otherwise.</returns>
	public static bool SaveKiiData ()
	{

		KiiQuery allQuery = new KiiQuery ();
		try {

			KiiQueryResult<KiiObject> result = KiiUser.CurrentUser.Bucket ("myBasicData").Query (allQuery);

			foreach (KiiObject obj in result) {

				obj ["time"] = userDataManager.time;
			}
		} catch (System.Exception exception) {

			Debug.Log (exception);
			return false;
		}

		return true;
	}

	/// <summary>
	/// ユーザー情報をクリアする（端末内）
	/// </summary>
	public void ResetButton ()
	{

		PlayerPrefs.DeleteAll ();
	}
}

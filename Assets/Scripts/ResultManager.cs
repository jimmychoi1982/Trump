using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;

public class ResultManager : MonoBehaviour
{
	[SerializeField] private GameObject _nodeObj;
	[SerializeField] private Transform _contentTran;

//	List<KiiObject> test;
	// Use this for initialization
	void Start ()
	{
	
//		GameObject.Find ("KiiInit").GetComponent <KiiManagerMulti> ().ShowUserRank ();

//		test = new List<KiiObject> ();

		ShowUserRank ();
	}
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void SetScollView (string userName, int time)
	{
		GameObject nodeObj = Instantiate (_nodeObj) as GameObject;
		nodeObj.GetComponent <Node> ().setNode (userName, time);
		nodeObj.transform.SetParent (_contentTran);
		nodeObj.transform.localScale = new Vector3 (1, 1, 1);
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

			//			ShowUserRank();
		});
	}

	public void ShowUserRank(){

		KiiQuery allQuery = new KiiQuery ();

		allQuery.SortByDesc ("time"); //按指定字段降序排列。
		allQuery.Limit = 10;

		string userName = "";
		int time = 0;

		Kii.Bucket ("Ranking").Query (allQuery, (KiiQueryResult<KiiObject> result, Exception ex) => {

			if (ex != null){
				Debug.Log ("Connect error:: " + ex);
				return;
			}
				
			foreach (KiiObject obj in result){

				userName = obj["userName"].ToString();
				time = (int)obj["time"];

				SetScollView (userName, time);
//				Uri uri = obj.Uri;
//				KiiObject obj2 = KiiObject.CreateByUri(uri);
//				obj2.Refresh ((KiiObject refreshedObj, Exception ex2) => {
//					if (ex2 != null){
//						Debug.Log ("Get KiiOjbect is Fail");
//					}
//						
//					Debug.Log (obj2["time"].ToString());
//
//					test.Add(obj2);
//				});
			}

//			Debug.Log ("Show complete");
		});
	}
}

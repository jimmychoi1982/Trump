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

		//Update Application Bucket
		KiiBucket applicationBucket = Kii.Bucket ("Ranking");
		KiiQuery allQuery = new KiiQuery ();

		applicationBucket.Query (allQuery, (KiiQueryResult<KiiObject> result, Exception ex) => {

			if (ex != null){

				Debug.Log ("Connect error");
				return;
			}

			foreach (KiiObject obj in result){

				if ((string)obj["userName"] == userDataManager.userName){

					obj ["time"] = userDataManager.time;
					obj.Save ((KiiObject savedObj, Exception ex2) => {

						if (ex2 != null){

							Debug.Log ("Connect error");
							return;
						}

						Debug.Log (userDataManager.userName + "has been updated: time::" + userDataManager.time);

						// Show Ranking popup
						ShowUserRank ();
					});
				}
			}
		});
	}

	// Update is called once per frame
	void Update ()
	{
	
	}

	/// <summary>
	/// Sets the ranking scoll view.
	/// </summary>
	/// <param name="userName">User name.</param>
	/// <param name="time">Time.</param>
	public void SetScollView (string userName, int time)
	{
		GameObject nodeObj = Instantiate (_nodeObj) as GameObject;
		nodeObj.GetComponent <Node> ().setNode (userName, time);
		nodeObj.transform.SetParent (_contentTran);
		nodeObj.transform.localScale = new Vector3 (1, 1, 1);
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
			}
		});
	}

	public void BackToTitleButton(){

		Application.LoadLevel ("TItleScene_Multi");
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;

public class ResultManager : MonoBehaviour
{
	[SerializeField] private GameObject _nodeObj;
	[SerializeField] private Transform _contentTran;

	[SerializeField] private Text _resultTitleText;

//	List<KiiObject> test;
	// Use this for initialization
	void Start ()
	{
		KiiManagerMulti.SaveUserScopeData (() => {

			KiiManagerMulti.SaveApplicationScope (() => {

				ShowUserRank();
			});
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
	
		string clearTimeKind = "";

		switch (userDataManager.level) {

		case userDataManager.LEVEL.EASY:

			clearTimeKind = "easyClearTime";
			_resultTitleText.text = "Easy";
			break;

		case userDataManager.LEVEL.NORMAL:

			clearTimeKind = "normalClearTime";
			_resultTitleText.text = "Normal";
			break;

		case userDataManager.LEVEL.HARD:

			clearTimeKind = "hardClearTime";
			_resultTitleText.text = "Hard";
			break;
		}

		KiiQuery allQuery = new KiiQuery ();

		allQuery.SortByAsc (clearTimeKind); //按指定字段降序排列。
		allQuery.Limit = 10;

		string userName = "";
		int time = 0;

		Kii.Bucket ("ApplicationData").Query (allQuery, (KiiQueryResult<KiiObject> result, Exception ex) => {

			if (ex != null){
				Debug.Log ("Connect error:: " + ex);
				return;
			}
				
			foreach (KiiObject obj in result){
			
				if ((int)obj[clearTimeKind] > 0){

					userName = obj["userName"].ToString();
					time = (int)obj[clearTimeKind];

					SetScollView (userName, time);
				}
			}
		});
	}

	public void BackToTitleButton(){

		Application.LoadLevel ("HomeScene");
	}
}

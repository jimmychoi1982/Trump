using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;

public class MultiResultManager : MonoBehaviour {

	[SerializeField] private GameObject _multiRankingObj;
	[SerializeField] private Transform _contentTran;

	[SerializeField] private Text _levelText;

	[SerializeField] private Sprite[] iconSprites;

	void Start ()
	{
		KiiManagerMulti.SaveUserScopeData (() => {

			KiiManagerMulti.SaveApplicationScope (() => {

				ShowUserRank();
			});
		});
	}
	
	/// <summary>
	/// Sets the ranking scoll view.
	/// </summary>
	/// <param name="userName">User name.</param>
	/// <param name="time">Time.</param>
	public void SetScollView (int iconIndex, string userName, string percentage)
	{
		Sprite icon = iconSprites [iconIndex];

		GameObject nodeObj = Instantiate (_multiRankingObj) as GameObject;
		nodeObj.GetComponent <MultiRankingObj> ().SetNode (icon, userName, percentage);
		nodeObj.transform.SetParent (_contentTran);
		nodeObj.transform.localScale = new Vector3 (1, 1, 1);
	}

	public void ShowUserRank(){

		switch (userDataManager.level) {

		case userDataManager.LEVEL.EASY:
		
			_levelText.text = "Easy";
			break;

		case userDataManager.LEVEL.NORMAL:

			_levelText.text = "Normal";
			break;

		case userDataManager.LEVEL.HARD:

			_levelText.text = "Hard";
			break;
		}

		KiiQuery allQuery = new KiiQuery ();

		allQuery.SortByDesc ("percentage"); //按指定字段降序排列。
		allQuery.Limit = 10;

		Kii.Bucket ("ApplicationData").Query (allQuery, (KiiQueryResult<KiiObject> result, Exception ex) => {

			if (ex != null){
				Debug.Log ("Connect error:: " + ex);
				return;
			}

			foreach (KiiObject obj in result){

				int iconIndex = (int)obj ["iconIndex"];
				string userName = (string)obj["userName"];
				int percentage = (int)obj["percentage"];
				string percentageStr = percentage + "%";

				SetScollView (iconIndex, userName, percentageStr);
			}
		});
	}

	public void BackToTitleButton(){

		Application.LoadLevel ("HomeScene");
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;

public class RankingScrollView : MonoBehaviour {

	[SerializeField] private Transform _contentTran;
	[SerializeField] private GameObject _nodeObj;
	[SerializeField] private Sprite[] _buttonSprite;

	[SerializeField] private Button[] _tabObj;

	public void Init(){

		ShowUserRank (userDataManager.LEVEL.EASY);
		_tabObj [0].image.sprite = _buttonSprite[1];
	}

	public void ShowUserRank(userDataManager.LEVEL level){

		string sortByKey = "";

		switch (level) {

		case userDataManager.LEVEL.EASY:

			sortByKey = "easyClearTime";
			break;
		case userDataManager.LEVEL.NORMAL:

			sortByKey = "normalClearTime";
			break;
		case userDataManager.LEVEL.HARD:

			sortByKey = "hardClearTime";
			break;
		}

		KiiQuery allQuery = new KiiQuery ();

		allQuery.SortByAsc (sortByKey); //按指定字段降序排列。
		allQuery.Limit = 10;

		string userName = "";
		int time = 0;

		Kii.Bucket ("ApplicationData").Query (allQuery, (KiiQueryResult<KiiObject> result, Exception ex) => {

			if (ex != null){
				Debug.Log ("Connect error:: " + ex);
				return;
			}

			foreach (KiiObject obj in result){

				if ((int)obj[sortByKey] > 0){

					userName = obj["userName"].ToString();
					time = (int)obj[sortByKey];

					SetScollView (userName, time);
				}
			}
		});
	}

	public void SetScollView (string userName, int time)
	{
		GameObject nodeObj = Instantiate (_nodeObj) as GameObject;
		nodeObj.GetComponent <Node> ().setNode (userName, time);
		nodeObj.transform.SetParent (_contentTran);
		nodeObj.transform.localScale = new Vector3 (1, 1, 1);
	}

	public void TabButton(int level){
	
		foreach (Transform t in _contentTran) {

			Destroy (t.gameObject);
		}

		ShowUserRank ((userDataManager.LEVEL)level);

		for (int i = 0; i < 3; ++i) {

			if (i == level) {
				_tabObj [i].image.sprite = _buttonSprite[1];
				continue;
			}
			_tabObj [i].image.sprite = _buttonSprite[0];
		}
	}
}

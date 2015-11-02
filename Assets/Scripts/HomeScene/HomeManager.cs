using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;

public class HomeManager : MonoBehaviour {

	[SerializeField] private ProfileView _profileView;
	[SerializeField] private RankingScrollView _rankingScrollView;
	[SerializeField] private AdvertisingScrollView _advertisingScrollView;

	[SerializeField] private Sprite[] _iconSprites;

	// Use this for initialization
	void Start () {
	
		initProfileVew ();
		_rankingScrollView.Init ();
	}

	private void initProfileVew(){
	
		_profileView.SetProfileView (
			_iconSprites [userDataManager.IconIndex], userDataManager.userName
		);

		string TimeString = string.Format ("{0:000.000}s", userDataManager.easyClearTime / 1000);
		_profileView.SetPullDownMenu (userDataManager.LEVEL.EASY, TimeString);

		TimeString = string.Format ("{0:000.000}s", userDataManager.normalClearTime / 1000);
		_profileView.SetPullDownMenu (userDataManager.LEVEL.NORMAL, TimeString);

		TimeString = string.Format ("{0:000.000}s", userDataManager.hardClearTime / 1000);
		_profileView.SetPullDownMenu (userDataManager.LEVEL.HARD, TimeString);
	}

	// Update is called once per frame
	void Update () {
	
	}
}

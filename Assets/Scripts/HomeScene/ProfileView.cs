using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ProfileView : MonoBehaviour {

	[SerializeField] private Image _playerPhotoImage;
	[SerializeField] private Text _nameText;

	[SerializeField] private Image _levelImage;
	[SerializeField] private Text _bestTimeText;

	[SerializeField] private GameObject _pullDownMenuObj;

	[Serializable]
	private class PullDownMenu
	{
		[SerializeField] internal Text _easyBestTimeText;
		[SerializeField] internal Image _easyLevelIcon;

		[SerializeField] internal Text _normalBestTimeText;
		[SerializeField] internal Image _normalLevelIcon;

		[SerializeField] internal Text _hardBestTimeText;
		[SerializeField] internal Image _hardLevelIcon;
	}

	[SerializeField] private PullDownMenu _pullDownMenu;

	/// <summary>
	/// Sets the profile view.
	/// </summary>
	/// <param name="playerPhotoSprite">Player photo sprite.</param>
	/// <param name="name">Name.</param>
	public void SetProfileView(Sprite playerPhotoSprite, string name){

		_playerPhotoImage.sprite = playerPhotoSprite;
		_nameText.text = name;
	}

	/// <summary>
	/// Sets the pull down menu.
	/// </summary>
	/// <param name="level">Level.</param>
	/// <param name="bestTime">Best time.</param>
	public void SetPullDownMenu(userDataManager.LEVEL level, string bestTime){

		switch (level) {

		case userDataManager.LEVEL.EASY:

			_pullDownMenu._easyBestTimeText.text = bestTime;

			// Pull Down Menuの初期表示、Easyに設定する
			_bestTimeText.text = bestTime;
			_levelImage.sprite = _pullDownMenu._easyLevelIcon.sprite;
			break;
		case userDataManager.LEVEL.NORMAL:

			_pullDownMenu._normalBestTimeText.text = bestTime;
			break;
		case userDataManager.LEVEL.HARD:

			_pullDownMenu._hardBestTimeText.text = bestTime;
			break;
		}
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiRankingObj : MonoBehaviour {

	[SerializeField] private Image _iconImage;
	[SerializeField] private Text _userNameText;
	[SerializeField] private Text _percentageText;

	public void SetNode(Sprite icon, string userName, string percentage){

		_iconImage.sprite = icon;
		_userNameText.text = userName;
		_percentageText.text = percentage;
	}
}

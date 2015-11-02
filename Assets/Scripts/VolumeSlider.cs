using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour {
 
	[SerializeField] private Slider _sliderSlider;    // 各スライダーを受け取る
	[SerializeField] private bool _bgmbool;           // BGMかSEかの判別

	private float _bgmvolumefloat;                    // 現在のBGMボリューム
	private float _sevolumefloat;                     // 現在のSEボリューム
	private const string VOLUMEBGM = "savevolumeBGM"; // BGMボリュームのキー
	private const string VOLUMESE = "savevolumeSE";   // SEボリュームのキー

	// Use this for initialization
	void Start () {
	
		_sliderSlider = GetComponent<Slider> ();
		if (_bgmbool) {                                            // 前回のボリューム値を呼び出してセットする
			_sliderSlider.value = PlayerPrefs.GetFloat (VOLUMEBGM);
		} else {
			_sliderSlider.value = PlayerPrefs.GetFloat (VOLUMESE);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// ボリュームが変化すると呼び出される
	public void changeVolume(float f){

		if (_bgmbool) {                             // 現在のボリューム値を取得
			_bgmvolumefloat = _sliderSlider.value;
			Debug.Log ("BGM" + _bgmvolumefloat);
		} else {
			_sevolumefloat = _sliderSlider.value;
			Debug.Log ("SE" + _sevolumefloat);
		}
	}

	// 今回のボリューム値を保存する
	public void saveVolume () {

		if (_bgmbool) {
			PlayerPrefs.SetFloat (VOLUMEBGM, _bgmvolumefloat);
			Debug.Log (_bgmvolumefloat);
		} else {
			PlayerPrefs.SetFloat (VOLUMESE, _sevolumefloat);
			Debug.Log (_sevolumefloat);
		}
	}
}

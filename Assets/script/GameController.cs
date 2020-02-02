using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using UnityEngine.Advertisements;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class GameController : MonoBehaviour {

	public GameObject[] charactors;

	public GameObject titleCanvas;
	public GameObject titlePanel;
	public GameObject canvas;
	public GameObject moveCanvas;
	public GameObject resultImage;
	public GameObject ScoreObj;
	public GameObject MaxScoreObj;
	public GameObject PointObj;
	public GameObject rewardButton;

	public GameObject reawrdedTextObj;

	public GameObject soundButtonObj;
	public Sprite soundOnImage;
	public Sprite soundOffImage;

	Image soundButtonImage;

	TextMeshProUGUI scoreText;
	TextMeshProUGUI maxScoreText;
	TextMeshProUGUI pointText;

	bool dead = false;
	int score;
	int maxScore;
	int point;
	int uploadScore;
	bool isNetConnect;

	// Use this for initialization
	void Start () {
		Invoke ("HideTitleCanvas", 1.5f);
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		if (dead) {
			if(Input.GetButtonDown("Cancel")){
				DialogManager.Instance.SetLabel ("YES", "NO", "CLOSE");
				DialogManager.Instance.ShowSelectDialog ("QUIT?", (bool result) => {
					if(result){
						Application.Quit();
					}
				});
			}
		}
	}

	private void HideTitleCanvas(){
		RectTransform rect = titlePanel.GetComponent<RectTransform> ();
		rect.DOScaleX (0f,0.2f).OnComplete(()=>{
			titleCanvas.SetActive (false);
		});
	}

	void Init(){
		dead = false;
		soundButtonImage = soundButtonObj.GetComponent<Image> ();
		bool isSound = EncryptedPlayerPrefs.LoadBool (Const.KEY_SOUND, true);
		if (isSound) {
			soundButtonImage.sprite = soundOnImage;
			SoundController.Instance.SetSound (true);
		} else {
			soundButtonImage.sprite = soundOffImage;
			SoundController.Instance.SetSound (false);
		}

		int charaNo = EncryptedPlayerPrefs.LoadInt (Const.KEY_CHARA, 0);
		GameObject chara = charactors [charaNo];
		Instantiate (chara, chara.transform.position, chara.transform.rotation);
		score = 0;
		scoreText = ScoreObj.GetComponent<TextMeshProUGUI> (); 

		maxScoreText = MaxScoreObj.GetComponent<TextMeshProUGUI> (); 
		pointText = PointObj.GetComponent<TextMeshProUGUI> (); 
		maxScore = EncryptedPlayerPrefs.LoadInt (Const.KEY_MAX_SCORE, 0);
		point = EncryptedPlayerPrefs.LoadInt (Const.KEY_POINT, 0);

		uploadScore = EncryptedPlayerPrefs.LoadInt (Const.KEY_UPD_SCORE, 0);
		maxScoreText.text = maxScore.ToString();
		pointText.text = point.ToString();
		rewardButton.SetActive (false);
		reawrdedTextObj.SetActive (false);
		Admanager.bannerView.Hide ();
		isNetConnect = (Application.internetReachability != NetworkReachability.NotReachable);
	}

	public void OnDead(){
		SoundController.Instance.PlaySe (SoundController.Instance.f1Se);
		moveCanvas.SetActive (false);

		EncryptedPlayerPrefs.SaveInt (Const.KEY_POINT, point);
		if (score > maxScore) {
			maxScore = score;
			EncryptedPlayerPrefs.SaveInt (Const.KEY_MAX_SCORE, score);
		}

		Application.CaptureScreenshot ("screenShot.png");
		StartCoroutine("OnRetry");
	}

	IEnumerator OnRetry(){
		yield return new WaitForSeconds (1.0f);

		string imagePath = Application.persistentDataPath + "/screenShot.png";

		if (File.Exists (imagePath)) {
			
			byte[] image = File.ReadAllBytes (imagePath);
			Texture2D texture = new Texture2D (0,0);
			texture.LoadImage (image);

			Sprite imageSprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f));
			resultImage.GetComponent<Image> ().sprite = imageSprite;
		}
		canvas.SetActive (true);
		dead = true;
		Admanager.bannerView.Show ();
		int random = Random.Range (0, 100);
		if (random <= 10) {
			if (Advertisement.IsReady ("rewardedVideo")) {
				rewardButton.SetActive (true);
			}
		}

		#if UNITY_ANDROID
		//レビュー依頼はAndroidのみ
		if (isNetConnect) {
			//ネット接続しているときのみ
			if (score >= 100) {
				bool reviewFlg2 = EncryptedPlayerPrefs.LoadBool (Const.KEY_REVIEW_FLG2, false);
				bool isReviewOn = EncryptedPlayerPrefs.LoadBool (Const.KEY_REVIEW_ON_FLG, false);
				if (!isReviewOn && !reviewFlg2) {
					// 50の時にレビューしてない場合は100以上で再度レビュー依頼
					DialogManager.Instance.SetLabel ("YES!", "NO..", "CLOSE");
					DialogManager.Instance.ShowSelectDialog ("Amazing!\nPlease Review!", (bool result) => {
						if (result) {
							Application.OpenURL (Const.ANDROID_URL);
							EncryptedPlayerPrefs.SaveBool (Const.KEY_REVIEW_FLG2, true);
							EncryptedPlayerPrefs.SaveBool (Const.KEY_REVIEW_ON_FLG, true);
						} else {
							EncryptedPlayerPrefs.SaveBool (Const.KEY_REVIEW_FLG2, true);
							EncryptedPlayerPrefs.SaveBool (Const.KEY_REVIEW_ON_FLG, true);
						}
					});
				}
			}else if (score >= 50) {
				//50以上出したら一回だけレビュー依頼
				bool reviewFlg = EncryptedPlayerPrefs.LoadBool (Const.KEY_REVIEW_FLG, false);
				if (!reviewFlg) {
					DialogManager.Instance.SetLabel ("YES!", "NO..", "CLOSE");
					DialogManager.Instance.ShowSelectDialog ("Please Review.", (bool result) => {
						if (result) {
							Application.OpenURL (Const.ANDROID_URL);
							EncryptedPlayerPrefs.SaveBool (Const.KEY_REVIEW_FLG, true);
							EncryptedPlayerPrefs.SaveBool (Const.KEY_REVIEW_ON_FLG, true);
						} else {
							EncryptedPlayerPrefs.SaveBool (Const.KEY_REVIEW_FLG, true);
						}
					});
				}
			}
		}
		#endif
	}

	public void OnShare(){
		SoundController.Instance.PlaySe (SoundController.Instance.buttonSe);

		string url = "";
		#if UNITY_ANDROID
		url = Const.ANDROID_URL;
		#elif UNITY_IPHONE
		url = Const.IOS_URL;
		#endif
		string fileName = Application.persistentDataPath + "/screenShot.png";
		string shareText = "Super high speed highway \nToo Fast!!\nScore " + score + "\n";
		if (File.Exists (fileName)) {
			SocialConnector.SocialConnector.Share (shareText,url,fileName);
		} else {
			SocialConnector.SocialConnector.Share (shareText,url);
		}
	}

	public void Retry(){
		Admanager.bannerView.Hide ();
		SoundController.Instance.PlaySe (SoundController.Instance.buttonSe);
		titleCanvas.SetActive (true);
		RectTransform rect = titlePanel.GetComponent<RectTransform> ();
		rect.DOScaleX (1f, 0.2f);
		Invoke ("MainScene",0.5f);

	}

	private void MainScene(){
		SceneManager.LoadScene ("main");
	}

	public void ScoreIncrement(){
		score += 1;
		scoreText.text = score.ToString ();

		point += 1;
		pointText.text = point.ToString();

		if (score > maxScore) {
			maxScoreText.text = score.ToString();
		}
	}

	public void ToShop(){
		SoundController.Instance.PlaySe (SoundController.Instance.buttonSe);
		Invoke ("Shop", 0.5f);
	}

	public void Shop(){
		SceneManager.LoadScene ("shop");
	}
	
		
	public void ShowRewardedAd()
	{
		if (Advertisement.IsReady ("rewardedVideo")) {
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show ("rewardedVideo", options);
		}
	}
	
	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			AdSuccess ();
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}

	private void AdSuccess(){
		SoundController.Instance.PlaySe (SoundController.Instance.rewardedSe);
		rewardButton.SetActive (false);
		reawrdedTextObj.SetActive (true);
		point += 100;
		EncryptedPlayerPrefs.SaveInt (Const.KEY_POINT, point);
		Sequence seq = DOTween.Sequence ();
		RectTransform rect = reawrdedTextObj.GetComponent<RectTransform> ();
		seq.Append (rect.DOScaleX (1f, 0.5f).OnComplete(()=>{
			pointText.text = point.ToString();
		}));
		seq.Append (rect.DOScaleX (0f, 0.5f).SetDelay (1f));
	}

	public void OnClickSoundButton(){
		if (SoundController.Instance.IsSoundOn()) {
			soundButtonImage.sprite = soundOffImage;
			SoundController.Instance.SetSound (false);
			EncryptedPlayerPrefs.SaveBool (Const.KEY_SOUND, false);
		} else {
			soundButtonImage.sprite = soundOnImage;
			SoundController.Instance.SetSound (true);
			EncryptedPlayerPrefs.SaveBool (Const.KEY_SOUND, true);
		}
	}


	//ランキング表示
	public void ViewRanking(){
		SoundController.Instance.PlaySe (SoundController.Instance.buttonSe);

		//ハイスコア更新
		if (maxScore > uploadScore) {
			Social.ReportScore (maxScore, Const.LEADERBOARD_ID, (bool success) => {
				if (success) {
					EncryptedPlayerPrefs.SaveInt (Const.KEY_UPD_SCORE, maxScore);
				}
				((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(Const.LEADERBOARD_ID);
			});
		} else {
			((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(Const.LEADERBOARD_ID);
		}

	}
}

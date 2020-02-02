using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;
public class Admanager : MonoBehaviour {

	public static BannerView bannerView;
	public static Admanager Instance;

	void Awake(){
		if (Instance) {
			DestroyImmediate (gameObject);
		} else {
			UnityAdRequest ();
			RequestBanner ();
		}
	}

	private void UnityAdRequest(){

		string gameId = "";
		#if UNITY_ANDROID
		gameId = Const.UNITY_ANDROID_GAME_ID;
		#elif UNITY_IPHONE
		gameId = Const.UNITY_IOS_GAME_ID;
		#endif

		Advertisement.Initialize (gameId);
	}

	private void RequestBanner()
	{
		if (bannerView != null) {
			return;
		}
		// 広告ユニット ID を記述します
		string adUnitId = "";
		#if UNITY_ANDROID
		adUnitId = Const.ADMOB_BANNER_ANDROID_UNIT_ID;
		#elif UNITY_IPHONE
		adUnitId = Const.ADMOB_BANNER_IOS_UNIT_ID;
		#endif

		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().AddKeyword("game").AddTestDevice(Const.TEST_DEVICE_ID).Build();
		// Load the banner with the request.
		bannerView.LoadAd(request);

	}
}

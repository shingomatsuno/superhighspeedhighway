using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
public class PlayGames : MonoBehaviour {

	public static PlayGames Instance;

	void Awake(){
		if (Instance) {
			DestroyImmediate (gameObject);
		} else {
			Instance = this;
			DontDestroyOnLoad (gameObject);
			PlayGamesPlatform.Activate ();
			Social.localUser.Authenticate ((bool success) => {
				if(success){

				}else{

				}
			});
		}
	}
}

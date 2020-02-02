using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

	public static SoundController Instance;

	public AudioClip moveSe;
	public AudioClip buttonSe;
	public AudioClip f1Se;
	public AudioClip casher;
	public AudioClip damage;
	public AudioClip rewardedSe;
	AudioSource audioSource;

	void Awake(){
		Instance = this;
		audioSource = GetComponent<AudioSource> ();
		audioSource.mute = !EncryptedPlayerPrefs.LoadBool (Const.KEY_SOUND, true);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlaySe(AudioClip se){
		audioSource.PlayOneShot (se);
	}

	public void SetSound(bool isSound){
		audioSource.mute = !isSound;
	}

	public bool IsSoundOn(){
		return !audioSource.mute;
	}

	public void ToggleSound(){
		audioSource.mute = !audioSource.mute;
	}
}

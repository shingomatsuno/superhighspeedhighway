using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using DG.Tweening;
using TMPro;
public class Player : MonoBehaviour {

	GameObject gameManager;
	bool dead = false;
	float x = 0;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("GameManager");
		transform.DOScaleY (0.4f, 0.2f).SetLoops (-1, LoopType.Yoyo);
	}

	// Update is called once per frame
	void Update () {
		if (dead) {
			return;
		}
		Vector3 p = transform.position;
		if (CrossPlatformInputManager.GetButtonDown ("Right")) {
			if (p.x != 1.5f) {
				SoundController.Instance.PlaySe (SoundController.Instance.moveSe);
				x = 1.5f;
				transform.DOMoveX (x, 0.01f);
			}
		} else	if (CrossPlatformInputManager.GetButtonDown ("Center")) {
			if (p.x != 0f) {
				SoundController.Instance.PlaySe (SoundController.Instance.moveSe);
				x = 0f;
				transform.DOMoveX (x, 0.01f);
			}
		} else	if (CrossPlatformInputManager.GetButtonDown ("Left")) {
			if (p.x != -1.5f) {
				SoundController.Instance.PlaySe (SoundController.Instance.moveSe);
				x = -1.5f;
				transform.DOMoveX (x, 0.01f);
			}
		}


	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Point") {
			if (!dead) {
				gameManager.SendMessage ("ScoreIncrement");
			}
		}
	}

	public void OnDead(){
		if (transform.DOPlay () == 1) {
			transform.DOKill ();
		}
		SoundController.Instance.PlaySe (SoundController.Instance.damage);
		dead = true;
	}
}

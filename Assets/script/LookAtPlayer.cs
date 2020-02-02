using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

	GameObject player;
	bool dead = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (dead) {
			if (player != null) {
				transform.LookAt (player.transform.position);
			}
		}
	}

	public void OnDead(GameObject obj){
		player = obj;
		dead = true;
		transform.LookAt (player.transform.position);
	}
}

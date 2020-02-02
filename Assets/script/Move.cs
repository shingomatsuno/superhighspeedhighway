using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){
		Vector3 v =	rb.velocity;
		v.z = -Const.SPEED;
		rb.velocity = v;
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Player") {
			GameObject player = collision.gameObject;
			player.transform.SetParent(transform);
			Vector3 vector;
			if (gameObject.tag == "Pato") {
				vector = new Vector3 (0f,0.4f,1.7f);
			}else if (gameObject.tag == "Truck") {
				vector = new Vector3 (1.87f,0.4f,0f);
			}else if (gameObject.tag == "Truck2") {
				vector = new Vector3 (0f,0.36f,1.78f);
			}else if (gameObject.tag == "Wagon") {
				vector = new Vector3 (0f,0.6f,1.7f);
			}else {
				vector = new Vector3 (0f,0.6f,1.3f);
			}
			player.transform.localPosition = vector;
			float angle = Random.Range (0f, 360f);
			player.transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,angle));
			GameObject.FindGameObjectWithTag ("MainCamera").SendMessage("OnDead",player);
			player.SendMessage("OnDead");
			GameObject.Find("GameManager").SendMessage("OnDead");
		}
	}
}

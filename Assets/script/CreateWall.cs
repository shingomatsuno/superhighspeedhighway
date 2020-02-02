using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWall : MonoBehaviour {

	public GameObject[] objArray;
	public GameObject point;
	public float interval;
	// Use this for initialization
	void Start () {
		StartCoroutine ("MakeWall");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator MakeWall(){
		yield return new WaitForSeconds (2.5f);

		while (true) {
			Vector3 p = transform.position;
			int random = Random.Range (0, objArray.Length);
			GameObject obj1 = objArray [random];
			int random2 = Random.Range (0, objArray.Length);
			GameObject obj2 = objArray [random2];
			int random3 = Random.Range (0, 3);
			switch (random3) {
			case 0:
				Instantiate (obj1, new Vector3 (1.5f, 0.25f, p.z), obj1.transform.transform.rotation);
				Instantiate (obj2, new Vector3 (0f, 0.25f, p.z), obj2.transform.transform.rotation);
				Instantiate (point, new Vector3 (-1.5f, 0.75f, p.z), point.transform.transform.rotation);
				break;
			case 1:
				Instantiate (point, new Vector3 (1.5f, 0.75f, p.z), point.transform.transform.rotation);
				Instantiate (obj1, new Vector3 (0f, 0.25f, p.z), obj1.transform.transform.rotation);
				Instantiate (obj2, new Vector3 (-1.5f, 0.25f, p.z), obj2.transform.transform.rotation);
				break;
			case 2:
				Instantiate (obj1, new Vector3 (1.5f, 0.25f, p.z), obj1.transform.transform.rotation);
				Instantiate (point, new Vector3 (0f, 0.75f, p.z), point.transform.transform.rotation);
				Instantiate (obj2, new Vector3 (-1.5f, 0.25f, p.z), obj2.transform.transform.rotation);
				break;
			}

			yield return new WaitForSeconds (interval);
		}
	}
}

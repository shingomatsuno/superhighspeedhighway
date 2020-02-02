using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuiteGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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

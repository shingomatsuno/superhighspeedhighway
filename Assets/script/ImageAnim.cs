using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ImageAnim : MonoBehaviour {

	RectTransform image;

	// Use this for initialization
	void Start () {
		image = GetComponent<RectTransform> ();
		image.DOScaleY (1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

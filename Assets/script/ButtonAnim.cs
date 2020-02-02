using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ButtonAnim : MonoBehaviour {

	RectTransform image;
	// Use this for initialization
	void Start () {
		image = GetComponent<RectTransform> ();
		image.DOScale (0.8f, 0.5f).SetLoops (-1, LoopType.Yoyo);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

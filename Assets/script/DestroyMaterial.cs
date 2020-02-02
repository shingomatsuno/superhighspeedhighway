using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMaterial : MonoBehaviour {

	void OnDestroy(){
		MeshRenderer render = GetComponent<MeshRenderer> ();
		if (render != null && render.materials != null) {
			foreach (Material m in render.materials) {
				DestroyImmediate (m);
			}
		}
	}
}

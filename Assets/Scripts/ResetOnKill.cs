using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnKill : MonoBehaviour {

	Vector3 originalPos;
	void Start () {
		originalPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "KillPlane") {
			gameObject.transform.position = originalPos;
		}	
	}
}

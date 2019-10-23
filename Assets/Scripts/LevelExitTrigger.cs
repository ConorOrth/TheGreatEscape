using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExitTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.tag == "Player") 
		{
			GameControl.instance.beatLevel ();
		}
	}
}

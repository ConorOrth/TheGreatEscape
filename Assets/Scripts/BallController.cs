using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {


	public GameObject player;

	public Transform chainAttach;

	public float maxDistance = 6f;


	Rigidbody2D rb2d;

	void Start () 
	{
		rb2d = gameObject.GetComponent<Rigidbody2D> ();
	}

	void Update () 
	{
		LineRenderer lineRenderer = GetComponent<LineRenderer> ();

		lineRenderer.SetPosition (0, transform.position);
		lineRenderer.SetPosition (1, chainAttach.position);

		if (getPlayerDistance () > maxDistance) {
			gameObject.transform.position = player.transform.position;
		}

		
	}

	float getPlayerDistance(){
		return (player.transform.position - gameObject.transform.position).magnitude;
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	public float moveSpeed = 3f;
	public float sprintSpeedIncrease = 0.1f;
	public float sprintMaxSpeed = 6f;
	public float jumpForce = 600f;
	public float grabDistance = 2f;
	public float throwForce = 10f;

	public LayerMask whatIsGround;
	public LayerMask whatIsGrabbable;
	public BoxCollider2D groundCheck;
	public Transform holdPoint;
	public Transform grabOrigin;
	public GameObject ball;


	bool facingRight = true;
	bool isGrounded = false;
	bool isRunning = false;
	bool carryingObj = false;

	Rigidbody2D rb2d;
	Animator animator;

	RaycastHit2D grab;

	float vx = 0f;
	float vy = 0f;
	float sprint = 0f;
	float chainD;

	void Awake () 
	{
		rb2d = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();

		Physics2D.IgnoreLayerCollision (gameObject.layer, LayerMask.NameToLayer ("Ignore Raycast"));

		DistanceJoint2D chain = ball.GetComponent<DistanceJoint2D> ();
		chainD = chain.distance;

		//playerLayer = gameObject.layer;
		//platformLayer = LayerMask.NameToLayer ("Platforms");
	}
	

	void LateUpdate () 
	{
	
		//=========[Movement]=====================================

		vx = Input.GetAxisRaw ("Horizontal");
			

		vy = rb2d.velocity.y;

		isGrounded = groundCheck.IsTouchingLayers(whatIsGround);

		if (isGrounded && Input.GetButtonDown("Jump")) 
		{
			Jump ();
		}

		if (Input.GetButtonUp ("Jump") && vy > 0f)
			vy = vy * 0.5f;

		//Apply Velocity to RB
		if (Input.GetButton ("Sprint")) {

			if (sprint > sprintMaxSpeed) 
			{
				sprint = sprintMaxSpeed;
			} 
			else 
			{
				sprint += sprintSpeedIncrease;
			}

			rb2d.velocity = new Vector2 (vx * sprint, vy);

		} 

		else 
		{
			rb2d.velocity = new Vector2 (vx * moveSpeed, vy);
			sprint = moveSpeed;
		}

		//=========[ChainPull]======================================

		DistanceJoint2D chain = ball.GetComponent<DistanceJoint2D> ();

		if (Input.GetButton ("ChainPull")) 
		{
			

			if (!carryingObj && !isRunning && isGrounded) 
			{
				if (chain.distance > .5f)
					chain.distance -= .05f;
				if (chain.distance <= .5f) {
					AttemptGrab ();
				}

				animator.SetBool ("ChainPull", true);
			}

		}

		if (Input.GetButtonUp ("ChainPull")) 
		{
			chain.distance = chainD;
			animator.SetBool ("ChainPull", false);
		}

		//=========[Grab/Throw]=====================================

		if (Input.GetButtonDown ("Grab")) 
		{
			if (!carryingObj) 
			{
				AttemptGrab ();

			}
			else if (CanDrop())
			{
				carryingObj = false;
				grab.collider.enabled = true;
				grab.collider.GetComponent<Rigidbody2D> ().isKinematic = false;

				Rigidbody2D grab_rb2d = grab.collider.gameObject.GetComponent<Rigidbody2D> ();

				if (grab_rb2d != null) 
				{
					if (vx != 0 || !isGrounded) 
					{
						grab_rb2d.velocity = new Vector2 (transform.localScale.x, 1) * throwForce;
					} 
					else 
					{
						grab_rb2d.velocity = new Vector2 (0, vy);
					}

				}
			}	

		}

		if (carryingObj) 
		{
			
			grab.collider.gameObject.transform.position = holdPoint.position;

			float x = rb2d.velocity.x;
			float y = rb2d.velocity.y;
			rb2d.velocity = new Vector2 (x * .75f, y - .05f);

		}


		//=========[TurningAround]=====================================

		Vector3 localScale = transform.localScale;
		if ( vx > 0 ) facingRight = true;
		if (vx < 0)
			facingRight = false;

		if ((facingRight && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
			localScale.x *= -1;

		transform.localScale = localScale;

		//=========[Animation]=====================================

		isRunning = vx != 0 ? true : false;
		animator.SetBool ("Running", isRunning);
		animator.SetBool ("Carrying", carryingObj);

	}

	void AttemptGrab()
	{
		grab = Physics2D.Raycast (grabOrigin.position, Vector2.right * transform.localScale.x, grabDistance, whatIsGrabbable);

		if (grab.collider != null) 
		{
			carryingObj = true;
			grab.collider.enabled = false;
			grab.collider.GetComponent<Rigidbody2D> ().isKinematic = true;

		}
	}

	void Jump ()
	{
		vy = 0f;
		rb2d.AddForce(new Vector2(0, jumpForce));
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.green;

		Gizmos.DrawLine (grabOrigin.position,grabOrigin.position + Vector3.right * transform.localScale.x * grabDistance);
	}

	bool CanDrop ()
	{
		if (!carryingObj)
			return false;

		if (holdPoint.GetComponent<Collider2D>().IsTouchingLayers(whatIsGround)) 
		{
			Debug.Log ("Overlapping Point");
			return false;
		}

		return true;
	}
						
		
}

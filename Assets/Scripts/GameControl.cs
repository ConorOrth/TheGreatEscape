using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {


	public static GameControl instance;
	public GameObject pauseMenu;
	public GameObject victoryMenu;

	bool isPaused = false;
	bool canPause = true;



	// Use this for initialization
	void Awake () {
		Debug.Log ("GameControllerAwake");
		if (instance == null) {
			instance = this;
			//DontDestroyOnLoad (this.gameObject);
			Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Ignore Raycast"), LayerMask.NameToLayer ("Ignore Raycast"));
		} 
		//Removes Current gameObject if another GameController Exists
		else if (instance != null) {
			Destroy (gameObject); 
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("Escape") && canPause) {
			togglePause ();
		}

	}

	public void togglePause()
	{
		if (isPaused) {
			pauseMenu.SetActive (false);
			Time.timeScale = 1;
			isPaused = false;
		} 

		else {
			pauseMenu.SetActive (true);
			Time.timeScale = 0;
			isPaused = true;
		}
	}

	public void levelExit()
	{
		isPaused = false;
		Time.timeScale = 1;
	}

	public void levelEnter()
	{
		canPause = true;
		Time.timeScale = 1;
	}

	public void beatLevel()
	{
		victoryMenu.SetActive (true);
		Time.timeScale = 0;
		isPaused = false;
		canPause = false;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

	[Space]
	[Header("GameObjects")]

	public GameObject train;
	public GameObject mainCamera;
	public GameObject mainMenuCanvas;
	public GameObject gameOverCanvas;

	[Space]
	[Header("Scripts")]

	public TrackSpawnerScript trackSpawnerScript;

	[Space]
	[Header("Bools")]

	public bool gamePlaying;



	void Start () {
		
		trackSpawnerScript = GameObject.FindGameObjectWithTag ("Track Spawner").GetComponent<TrackSpawnerScript> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.R)) {

			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);

		}

	}

	public void GameOver(){

		GameObject gameOverCanvas_ = Instantiate (gameOverCanvas, transform.position, transform.rotation) as GameObject;
		gameOverCanvas_.GetComponent<Canvas> ().worldCamera = mainCamera.GetComponent<Camera>();
		gameOverCanvas_.GetComponent<Canvas> ().sortingLayerName = "UI";

	}


	public IEnumerator BeginGame(){


		//Exit the Main Menu
		mainMenuCanvas.GetComponent<MainMenuScript> ().StartCoroutine ("MainMenuExit");



		//Wait
		yield return new WaitForSeconds (0.5f);


		//Reset Game Values
		ResetGameValues();
		//Start Spawning Tiles
		trackSpawnerScript.StartCoroutine ("SpawnTiles");
		//set gameplaying to true


		yield return new WaitForSeconds (2);

		gamePlaying = true;




	}

	public void ResetGameValues(){



	}
}

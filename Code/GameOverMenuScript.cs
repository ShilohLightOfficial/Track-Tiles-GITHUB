using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenuScript : MonoBehaviour {
	
	public Text scoreLabel;

	// Use this for initialization
	void Start () {
		scoreLabel.text = GameObject.FindGameObjectWithTag ("Track Spawner").GetComponent<TrackSpawnerScript> ().tilesOnScreen.ToString ();
		print (GameObject.FindGameObjectWithTag ("Track Spawner").GetComponent<TrackSpawnerScript> ().tilesOnScreen.ToString ());
		print (GameObject.FindGameObjectWithTag ("Track Spawner").GetComponent<TrackSpawnerScript> ().allTilesGenerated);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoBackToHomeMenu(){


		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);

	}
}

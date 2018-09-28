using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPieceScript : MonoBehaviour {

	//The Entry, middle,and exit point
	public GameObject[] accessPoints;
	public GameObject shadow;
	public GameObject starPrefab;
	public GameObject instantiatedStar;

	public float degree;

	public TrackSpawnerScript trackSpawnerScript;

	public bool starPiece;

	public AudioClip[] rotateTrackSounds;


	public AudioSource RotateSource;
	// Use this for initialization
	void Start () {

		//EVERY PICECE HAS A RANDOM CHANCE OF BEING A STAR PIECE
		int ran = Random.Range (0, 5);

		if (ran == 0) {

			//SPAWN A STAR AND ENABLE VARIABLE
			instantiatedStar = Instantiate (starPrefab, transform.position, Quaternion.Euler (new Vector3 (0, 0, 0))) as GameObject;
			starPiece = true;
		}


		//Inital Variable Setups
		trackSpawnerScript = GameObject.FindGameObjectWithTag ("Track Spawner").GetComponent<TrackSpawnerScript> ();
		Instantiate (shadow , transform.position, transform.rotation);

		//CHOOSE A RANDOM ROTATION
		degree = 90 * Random.Range (0, 5);

		////BUG FIX FOR WHEN THE TILE SPAWNS SLIGHTLY OFF POSITION
		FixPosition ();

	}
	
	// Update is called once per frame
	void Update () {

		//ALWAYS SMOOTHLY ROTATE TO WHATEVER THE DEGREE VARAIBLE IS
		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, 0, degree), Time.deltaTime * 10);
	}

	public void Rotate(){

		//EVERY TOUCH ROTATE THE TRACK 90 DEGREES
		degree += 90;
		RotateSource.clip = rotateTrackSounds [Random.Range (0, rotateTrackSounds.Length)];
		RotateSource.Play ();

	}


	public void FixPosition(){


		//BUG FIX FOR WHEN THE TILE SPAWNS SLIGHTLY OFF POSITION
		if ((this.transform.position.x > trackSpawnerScript.centerColumnX && this.transform.position.x < (trackSpawnerScript.centerColumnX + 0.1f)) || (this.transform.position.x < trackSpawnerScript.centerColumnX && this.transform.position.x > (trackSpawnerScript.centerColumnX -0.1f) )) {

			//SET THEIR POSITION TO EXACTLY 0
			this.transform.position = new Vector2 (trackSpawnerScript.centerColumnX, transform.position.y);

		}




	}
}

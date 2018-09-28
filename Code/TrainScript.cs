using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour {
	
	public Vector2[]  point;

	public GameObject trackPiece;
	public GameObject trainSprite;

	public GameManagerScript gameManager;

	float count = 0.0f;

	public bool currentlyOnPiece;
	public bool crashed;
	public bool inBetweenPieces;
	public bool coinAlreadyCollected;

	public bool onStraightPiece;

	public float speed;
	public float startingRotation;

	public GameObject testEntryPos;
	public GameObject testExitPos;


	void Start () {

		count = 2;
	}
		

	void FixedUpdate() {
		

		if (gameManager.gamePlaying) {
			if (!crashed) {
				speed += 0.001f;

				//		█▀▀█ █▀▀▄   █▀▀█   ▀▀█▀▀ █▀▀█ █▀▀█ █▀▀ █░█
				//		█░░█ █░░█   █▄▄█   ░░█░░ █▄▄▀ █▄▄█ █░░ █▀▄
				//		▀▀▀▀ ▀░░▀   ▀░░▀   ░░▀░░ ▀░▀▀ ▀░░▀ ▀▀▀ ▀░▀
				if (count < 1.0f) {

		
					count += (1.0f * (speed / 120));


					Vector3 m1 = Vector3.Lerp (point [0], point [1], count);
					Vector3 m2 = Vector3.Lerp (point [1], point [2], count);

					this.transform.position = Vector3.Lerp (m1, m2, count);

					if (currentlyOnPiece && !onStraightPiece) {
					
						testEntryPos.transform.position = point [0];
						testExitPos.transform.position = point [2];

						if (point [2].y > point [0].y) {
						
							//ROTATE LEFT OR RIGHT DEPENDING ON TYPE OF ROUND PIECE
							if (point [2].x > (point [1].x + 0.5f) || point [0].x > (point [1].x + 0.5f)) {
				
								Quaternion rotation = Quaternion.Euler (0, 0, startingRotation - (90 * count));
								gameObject.transform.rotation = rotation;
								trainSprite.GetComponent<Animator> ().Play ("Train Turn Left Animation");



							} else if (point [2].x < (point [1].x - 0.5f) || point [0].x < (point [1].x - 0.5f)) {
				
								Quaternion rotation = Quaternion.Euler (0, 0, startingRotation + (90 * count));
								gameObject.transform.rotation = rotation;
								trainSprite.GetComponent<Animator> ().Play ("Train Turn Right Animation");
						
							} else {


							}

						} else {
						
							if (point [2].x > (point [1].x + 0.5f) || point [0].x > (point [1].x + 0.5f)) {

								Quaternion rotation = Quaternion.Euler (0, 0, startingRotation + (90 * count));
								gameObject.transform.rotation = rotation;
								trainSprite.GetComponent<Animator> ().Play ("Train Turn Right Animation");


							} else {

								Quaternion rotation = Quaternion.Euler (0, 0, startingRotation - (90 * count));
								gameObject.transform.rotation = rotation;
								trainSprite.GetComponent<Animator> ().Play ("Train Turn Left Animation");

							}

						}

					}



				} else {
			
					currentlyOnPiece = false;
					onStraightPiece = false;
					inBetweenPieces = true;


					transform.Translate (Vector3.up * (speed / 100));


				}
			}else {

				// CODE FOR WHEN CRASHED
				if (gameManager.gamePlaying) {
					gameManager.GameOver ();
					gameManager.gamePlaying = false;
				}
			}
		} 





		//DETECT IF TRAIN IS OFF SCREEN
		if (Mathf.Abs(transform.position.x) > Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, 0)).x) {

			crashed = true;

		}



		if (inBetweenPieces) {

			coinAlreadyCollected = false;
		}


	}

	//WHEN THE TRAIN HITS SOMETHING
	void OnTriggerEnter2D(Collider2D coll){

		//IF THAT SOMETHING IS A CRASH DETECTOR (ON THE BAD SIDE OF EVERY TRACK)
		if (coll.transform.tag == "Crash Detector") {

			print ("Crash");

			crashed = true;

			//PLAY THE WOBBLE
			trainSprite.GetComponent<Animator> ().Play ("Train Wobble Animation");

		}
			
		//IF THE LAYER IS THE TRACK PIECE LAYER
		if (coll.gameObject.layer == 10) {


			//IF THE TRACK PIECE THAT WE COLLIDED WITH HAS A STAR ON IT
			if (coll.transform.parent.GetComponent<TrackPieceScript> ().starPiece && !coinAlreadyCollected) {

				//FIND THE CORRECT STAR
				//ADD FORCE UP 
				coll.transform.parent.GetComponent<TrackPieceScript> ().instantiatedStar.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 3), ForceMode2D.Impulse);
				//ENABLE GRAVITY ON IT
				coll.transform.parent.GetComponent<TrackPieceScript> ().instantiatedStar.GetComponent<Rigidbody2D> ().gravityScale = 1;
				//PLAY THE STAR COLLECTED ANIMATION
				coll.transform.parent.GetComponent<TrackPieceScript> ().instantiatedStar.transform.GetChild (0).GetComponent<Animator> ().Play ("Star Collected");

				coll.transform.parent.GetComponent<TrackPieceScript> ().instantiatedStar.GetComponent<AudioSource> ().Play ();
				coinAlreadyCollected = true;


			}


			
			//▀▀█▀▀ █░░█ █▀▀█ █▀▀▄ ░▀░ █▀▀▄ █▀▀▀   █▀▀█ ░▀░ █▀▀ █▀▀ █▀▀
			//░░█░░ █░░█ █▄▄▀ █░░█ ▀█▀ █░░█ █░▀█   █░░█ ▀█▀ █▀▀ █░░ █▀▀
			//░░▀░░ ░▀▀▀ ▀░▀▀ ▀░░▀ ▀▀▀ ▀░░▀ ▀▀▀▀   █▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀
			//If the tag is turning piece and it isnt already riding on a turning piece
			if (coll.gameObject.transform.parent.tag == "TurningPiece" && !currentlyOnPiece) {
			

				//SET UP BLANK ENTRY/EXIT POINT
				Vector2 exitPoint = new Vector2 (0, 0);
				Vector2 entryPoint = new Vector2 (0, 0);


				//FIND THE ACCESS POINT THAT THE TRAIN DID NOT HIT AND SET THAT AS EXIT POINT
				for (int i = 0; i < 2; i++) {
				
					if (coll.gameObject.transform.position != coll.transform.parent.GetComponent<TrackPieceScript> ().accessPoints [i].transform.position) {
					
						exitPoint = coll.transform.parent.GetComponent<TrackPieceScript> ().accessPoints [i].transform.position;

					} else {
						entryPoint = coll.gameObject.transform.parent.GetComponent<TrackPieceScript> ().accessPoints [i].transform.position;
					}

				}

				//SET THE ARRY POINTS TO THE CORRECT POISITIONS
				point [0] = transform.position;
				point [2] = exitPoint;
				point [1] = coll.gameObject.transform.parent.transform.position;


				//RESET COUNT AND ENABLE TURNING (THIS IS ESSENTIALLY CALLING THE TURNING FUNCTION)
				count = 0;

				currentlyOnPiece = true;
				inBetweenPieces = false;

				startingRotation = gameObject.transform.rotation.eulerAngles.z;
			}


//		█▀▀ ▀▀█▀▀ █▀▀█ █▀▀█ ░▀░ █▀▀▀ █░░█ ▀▀█▀▀   █▀▀█ ░▀░ █▀▀ █▀▀ █▀▀
//		▀▀█ ░░█░░ █▄▄▀ █▄▄█ ▀█▀ █░▀█ █▀▀█ ░░█░░   █░░█ ▀█▀ █▀▀ █░░ █▀▀
//		▀▀▀ ░░▀░░ ▀░▀▀ ▀░░▀ ▀▀▀ ▀▀▀▀ ▀░░▀ ░░▀░░   █▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀
			if (coll.gameObject.transform.parent.tag == "StraightPiece" && !currentlyOnPiece && !crashed) {
			

				//SET UP BLANK ENTRY/EXIT POINT
				Vector2 exitPoint = new Vector2 (0, 0);
				Vector2 entryPoint = new Vector2 (0, 0);


				//FIND THE ACCESS POINT THAT THE TRAIN DID NOT HIT AND SET THAT AS EXIT POINT
				for (int i = 0; i < 2; i++) {

					if (coll.gameObject.transform.position != coll.transform.parent.GetComponent<TrackPieceScript> ().accessPoints [i].transform.position) {

						exitPoint = coll.transform.parent.GetComponent<TrackPieceScript> ().accessPoints [i].transform.position;

					} else {
						entryPoint = coll.gameObject.transform.parent.GetComponent<TrackPieceScript> ().accessPoints [i].transform.position;
					}

				}

				//SET THE ARRY POINTS TO THE CORRECT POISITIONS
				point [0] = transform.position;
				point [2] = exitPoint;
				point [1] = coll.gameObject.transform.parent.transform.position;


				//RESET COUNT AND ENABLE TURNING (THIS IS ESSENTIALLY CALLING THE TURNING FUNCTION)
				count = 0;
				currentlyOnPiece = true;
				onStraightPiece = true;

				inBetweenPieces = false;
			}

//			█▀▀ █▀▀█ █░░ ░▀░ ▀▀█▀▀   █▀▀█ ░▀░ █▀▀ █▀▀ █▀▀
//			▀▀█ █░░█ █░░ ▀█▀ ░░█░░   █░░█ ▀█▀ █▀▀ █░░ █▀▀
//			▀▀▀ █▀▀▀ ▀▀▀ ▀▀▀ ░░▀░░   █▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀

			if (coll.gameObject.transform.parent.tag == "SplitPiece" && !currentlyOnPiece && !crashed) {


				//SET UP BLANK ENTRY/EXIT POINT
				Vector2 exitPoint = new Vector2 (0, 0);
				Vector2 entryPoint = new Vector2 (0, 0);
				string exitPointName = "ExitPoint" + Random.Range (0, 2).ToString ();


				//FIND THE ACCESS POINT THAT THE TRAIN DID NOT HIT AND SET THAT AS EXIT POINT
				for (int i = 0; i < 3; i++) {

					//IF TRAIN COLLIDES WITH AN EXIT POINT FIRST
					if (coll.gameObject.name == "ExitPoint0" || coll.gameObject.name == "ExitPoint1") {
					
						//LOOK FOR THE ENTRY POINT AND MAKE THAT WHERE WE'RE GOING (CONFUSING)
						if (coll.transform.parent.GetComponent<TrackPieceScript> ().accessPoints [i].gameObject.name == "EntryPoint") {

							exitPoint = coll.transform.parent.GetComponent<TrackPieceScript> ().accessPoints [i].transform.position;

						} 

					} else if (coll.gameObject.name == "EntryPoint") {

						//LOOK FOR THE EXIT POINT AND MAKE THAT WHERE WE'RE GOING
						if (coll.transform.parent.GetComponent<TrackPieceScript> ().accessPoints [i].gameObject.name == exitPointName) {

							exitPoint = coll.transform.parent.GetComponent<TrackPieceScript> ().accessPoints [i].transform.position;

						} 
					}


				}

				//SET THE ARRY POINTS TO THE CORRECT POISITIONS
				point [0] = transform.position;
				point [1] = coll.gameObject.transform.parent.transform.position;
				point [2] = exitPoint;



				//RESET COUNT AND ENABLE TURNING (THIS IS ESSENTIALLY CALLING THE TURNING FUNCTION)
				count = 0;
				currentlyOnPiece = true;
				inBetweenPieces = false;

				startingRotation = gameObject.transform.rotation.eulerAngles.z;

			}



//			░░▀ █░░█ █▀▀▄ █▀▀ ▀▀█▀▀ ░▀░ █▀▀█ █▀▀▄   █▀▀█ ░▀░ █▀▀ █▀▀ █▀▀
//			░░█ █░░█ █░░█ █░░ ░░█░░ ▀█▀ █░░█ █░░█   █░░█ ▀█▀ █▀▀ █░░ █▀▀
//			█▄█ ░▀▀▀ ▀░░▀ ▀▀▀ ░░▀░░ ▀▀▀ ▀▀▀▀ ▀░░▀   █▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀
			if ((coll.gameObject.transform.parent.tag == "RightJunctionPiece" || coll.gameObject.transform.parent.tag == "LeftJunctionPiece") && !currentlyOnPiece && !crashed) {


				//SET UP BLANK ENTRY/EXIT POINT
				Vector2 exitPoint = new Vector2 (0, 0);
				Vector2 entryPoint = new Vector2 (0, 0);
				string exitPointName = "ExitPoint" + Random.Range (0, 2).ToString ();


				//FIND THE ACCESS POINT THAT THE TRAIN DID NOT HIT AND SET THAT AS EXIT POINT
				for (int i = 0; i < 3; i++) {

					//IF TRAIN COLLIDES WITH AN EXIT POINT FIRST
					if (coll.gameObject.name == "ExitPoint0" || coll.gameObject.name == "ExitPoint1") {

						//LOOK FOR THE ENTRY POINT AND MAKE THAT WHERE WE'RE GOING (CONFUSING)
						if (coll.transform.parent.GetComponent<TrackPieceScript> ().accessPoints [i].gameObject.name == "EntryPoint") {

							exitPoint = coll.transform.parent.GetComponent<TrackPieceScript> ().accessPoints [i].transform.position;

						} 

					} else if (coll.gameObject.name == "EntryPoint") {

						//LOOK FOR THE EXIT POINT AND MAKE THAT WHERE WE'RE GOING
						if (coll.transform.parent.GetComponent<TrackPieceScript> ().accessPoints [i].gameObject.name == exitPointName) {

							exitPoint = coll.transform.parent.GetComponent<TrackPieceScript> ().accessPoints [i].transform.position;

						} 
					}


				}

				//SET THE ARRY POINTS TO THE CORRECT POISITIONS
				point [0] = transform.position;
				point [1] = coll.gameObject.transform.parent.transform.position;
				point [2] = exitPoint;



				//RESET COUNT AND ENABLE TURNING (THIS IS ESSENTIALLY CALLING THE TURNING FUNCTION)
				count = 0;

				currentlyOnPiece = true;
				inBetweenPieces = false;


				startingRotation = gameObject.transform.rotation.eulerAngles.z;

			}
		}


	}






}
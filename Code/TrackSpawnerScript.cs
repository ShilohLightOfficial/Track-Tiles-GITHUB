using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSpawnerScript : MonoBehaviour {
	
	public List<GameObject> allTilesGenerated = new List<GameObject>();


	public float leftColumnX;
	public float rightColumnX;
	public float centerColumnX;


	public float[] columnXs;

	public GameObject secondLastTile;
	public GameObject secondLastTilePath2;
	public GameObject lastTile;
	public GameObject lastTilePath2;
	public GameObject nextTile;
	public GameObject nextTilePath2;

	public GameObject TESTleftPos;
	public GameObject TESTrightPos;
	public GameObject TESTabovePos;

	public GameObject TESTleftPosPath2;
	public GameObject TESTrightPosPath2;
	public GameObject TESTabovePosPath2;

	public GameObject turningPiece;
	public GameObject straightPiece;
	public GameObject leftJunctionPiece;
	public GameObject rightJunctionPiece;
	public GameObject splitPiece;

	public int tilesOnScreen;

	public bool canSpawnSecondPath;
	// Use this for initialization
	void Start () {

		//COLUMN POSITIONS
		leftColumnX = -1.7f;
		centerColumnX = 0.0f;
		rightColumnX = 1.7f;

		//FILL THE ARRAY WITH COLUMN POSITIONS
 		columnXs [0] = leftColumnX;
		columnXs [1] = centerColumnX;
		columnXs [2] = rightColumnX;



	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown (KeyCode.Space)) {
			
			allTilesGenerated [allTilesGenerated.Count-1].gameObject.GetComponent<TrackPieceScript> ().Rotate ();
		}
	}
		

	public IEnumerator SpawnTiles(){
		
		while (true) {


			yield return new WaitForSeconds (0.25f);
			SpawnNextTile ();

		}
	}

	public void SpawnNextTile(){

		//INITAL VARIABLES
		bool canSpawnLeft = true;
		bool canSpawnAbove = true;
		bool canSpawnRight = true;

		bool canSpawnLeftPath2 = true;
		bool canSpawnAbovePath2 = true;
		bool canSpawnRightPath2 = true;

		bool canSpawnTurningPiece = true;
		bool canSpawnStraightPiece = true;
		bool canSpawnSplitPiece = false;

		bool canSpawnTurningPiecePath2 = true;
		bool canSpawnStraightPiecePath2 = true;
		bool canSpawnSplitPiecePath2 = false;

		bool mustConnectPaths = false;

		Vector2 possibleSpotAbove = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width/2, Screen.height * 0.3f, 0));
		Vector2 possibleSpotLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 0));
		Vector2 possibleSpotRight = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 0));

		Vector2 possibleSpotAbovePath2 = Camera.main.ScreenToWorldPoint (new Vector3 (0,0,0));
		Vector2 possibleSpotLeftPath2 = Camera.main.ScreenToWorldPoint (new Vector3 (0,0,0));
		Vector2 possibleSpotRightPath2 = Camera.main.ScreenToWorldPoint (new Vector3 (0,0,0));


		//EVERY TILE SPAWN AFTER THE SECOND
		if (tilesOnScreen > 1) {	

			possibleSpotAbove = lastTile.transform.position + new Vector3 (0, 1.6f, 0);
			possibleSpotLeft = lastTile.transform.position + new Vector3 (-1.6f, 0, 0);
			possibleSpotRight = lastTile.transform.position + new Vector3 (1.6f, 0, 0);

			//IF THE LAST TWO ON SAME LEVEL 
			if (lastTile.transform.position.y == secondLastTile.transform.position.y) {

				//AND ITS A TURNING PIECE IN THE CENTER
				if (lastTile.tag == "TurningPiece" && lastTile.transform.position.x == centerColumnX) {
					canSpawnLeft = false;
					canSpawnRight = false;





					if (!canSpawnSecondPath) {
						

						//ON THIS SPECIAL CASE IT CAN BE A SPLIT PIECE
						int ran = Random.Range (0, 1);

						if (ran == 0) {

							canSpawnSplitPiece = true;
							canSpawnStraightPiece = false;
							canSpawnTurningPiece = false;

						} 

					}
					
					//AND THE MIDDLE IS STRAIGHT PIECE
				} else if (lastTile.tag == "StraightPiece" && lastTile.transform.position.x == centerColumnX) {

					canSpawnAbove = false;
					canSpawnStraightPiece = false;
					print ("middle is straight piece");

				}



				//LAST TWO NOT ON THE SAME LEVEL
			} else {

				///AND ITS A TURNING PIECE
				if (lastTile.tag == "TurningPiece") {

					canSpawnAbove = false;

					//IN THE CENTER OR IF THERE ARE TWO PATHS
					if (lastTile.transform.position.x == centerColumnX || canSpawnSecondPath) {
					
						canSpawnStraightPiece = false;

					}


					//AND ITS A STRAIGHT PIECE
				} else if (lastTile.tag == "StraightPiece") {

					canSpawnLeft = false;
					canSpawnRight = false;

					if (canSpawnSecondPath) {
						if (lastTilePath2.transform.position.y < lastTile.transform.position.y) {
							canSpawnTurningPiecePath2 = false;
						}
					}

				} else if (lastTile.tag == "SplitPiece") {

					canSpawnAbove = false;
					canSpawnSecondPath = true;
					canSpawnStraightPiece = false;

				}

			}

			if (lastTile.gameObject.tag == "LeftJunctionPiece" || lastTile.gameObject.tag == "RightJunctionPiece") {

				canSpawnLeft = false;
				canSpawnRight = false;

			}

			if (secondLastTile.gameObject.tag == "SplitPiece" && canSpawnSecondPath) {

				canSpawnLeft = false;
				canSpawnRight = false;


			}

			//FOR WHEN TWO PATHS ARE BEING GENERATED
			if (canSpawnSecondPath) {

				//never can spawn a split path when there is already two paths
				canSpawnSplitPiece = false;

				if (lastTile.tag == "SplitPiece") {

					lastTilePath2 = lastTile;
					secondLastTilePath2 = secondLastTile;
					canSpawnAbovePath2 = false;
					canSpawnStraightPiece = false;
					canSpawnStraightPiecePath2 = false;

				} 

				possibleSpotAbovePath2 = lastTilePath2.transform.position + new Vector3 (0, 1.6f, 0);
				possibleSpotLeftPath2 = lastTilePath2.transform.position + new Vector3 (-1.6f, 0, 0);
				possibleSpotRightPath2 = lastTilePath2.transform.position + new Vector3 (1.6f, 0, 0);


				//IF THIS IS THE LEVEL RIGHT ABOVE THE SPLIT LEVEL
				if (secondLastTilePath2.gameObject.tag == "SplitPiece") {

					canSpawnRightPath2 = false;
					canSpawnLeftPath2 = false;


				//LAST TWO PIECES ON PATH2 WERE ON THE SAME LEVEL
				} else if (lastTilePath2.transform.position.y == secondLastTilePath2.transform.position.y) {

					//AND ITS A TURNING PIECE 
					if (lastTilePath2.tag == "TurningPiece" ) {
						
						if (lastTilePath2.transform.position.x == columnXs [1]) {

							canSpawnLeftPath2 = false;
							canSpawnRightPath2 = false;

						} else {
							print ("Turning piece on side");
						}
							

					} else if (lastTilePath2.tag == "StraightPiece") {

						if (lastTilePath2.transform.position.x == columnXs [1]) {
							canSpawnLeftPath2 = false;
							canSpawnRightPath2 = false;
						} else {
							print ("straight piece on side");

						} 



						
					}


				//LAST TWO PIECES WERE ON A DIFFRENT HEIGHT LEVEL
				} else if (lastTilePath2.transform.position.y != secondLastTilePath2.transform.position.y) {
					

					//AND ITS A TURNING PIECE 
					if (lastTilePath2.tag == "TurningPiece") {

						//ON THE SIDE
						if (lastTilePath2.transform.position.x != columnXs [1]) {
							
							canSpawnAbovePath2 = false;
							canSpawnStraightPiecePath2 = false;

						} else if (lastTilePath2.transform.position.x == columnXs [1]) {

							canSpawnAbovePath2 = false;

						}
							

						//AND ITS A STRAIGHT PIECE 
					} else if (lastTilePath2.tag == "StraightPiece") {

						//IF THERE ARE TWO PATHS BEING MADE MAKE SURE THAT THIS ONE DOSNT FALL TOO LOW

							
							if (lastTile.transform.position.y < lastTilePath2.transform.position.y) {
								canSpawnTurningPiece = false;
							}
						

						canSpawnLeftPath2 = false;
						canSpawnRightPath2 = false;
						
					} 
				}


			} 

		    if (canSpawnSecondPath) {

				//ALL THE THINGS THAT WOULD MAKE THE LEFT SPOT NOT VALID
				if (possibleSpotLeftPath2.x == secondLastTilePath2.transform.position.x || possibleSpotLeftPath2.x < leftColumnX || possibleSpotLeftPath2.x > rightColumnX) {

					canSpawnLeftPath2 = false;

				}

				//ALL THE THINGS THAT WOULD MAKE THE RIGHT SPOT NOT VALID
				if (possibleSpotRightPath2.x == secondLastTilePath2.transform.position.x || possibleSpotRightPath2.x < leftColumnX || possibleSpotRightPath2.x > rightColumnX) {

					canSpawnRightPath2 = false;

				}

//				for (int i = 0; i < allTilesGenerated.Count; i++) {
//
//					if (new Vector3(possibleSpotAbovePath2.x, possibleSpotAbovePath2.y,0) == allTilesGenerated [i].transform.position) {
//						canSpawnAbovePath2 = false;
//					}
//
//					if (new Vector3(possibleSpotLeftPath2.x, possibleSpotLeftPath2.y,0) == allTilesGenerated [i].transform.position) {
//						canSpawnLeftPath2 = false;
//					}
//
//					if (new Vector3(possibleSpotRightPath2.x, possibleSpotRightPath2.y,0) == allTilesGenerated [i].transform.position) {
//						canSpawnRightPath2 = false;
//					}
//				}



			}

			//ALL THE THINGS THAT WOULD MAKE THE LEFT SPOT NOT VALID
			if (possibleSpotLeft.x == secondLastTile.transform.position.x || possibleSpotLeft.x < leftColumnX || possibleSpotLeft.x > rightColumnX) {

				print ("Disabled Left");

				canSpawnLeft = false;

			}

			//ALL THE THINGS THAT WOULD MAKE THE RIGHT SPOT NOT VALID
			if (possibleSpotRight.x == secondLastTile.transform.position.x || possibleSpotRight.x < leftColumnX || possibleSpotRight.x > rightColumnX) {
				print ("Disabled Right");
				canSpawnRight = false;

			}

			for (int i = 0; i < allTilesGenerated.Count; i++) {
				
				if (new Vector3(possibleSpotAbove.x, possibleSpotAbove.y,0) == allTilesGenerated [i].transform.position) {
					canSpawnAbove = false;

				}
				if (new Vector3(possibleSpotLeft.x, possibleSpotLeft.y,0) == allTilesGenerated [i].transform.position) {
					canSpawnLeft = false;

				}
				if (new Vector3(possibleSpotRight.x, possibleSpotRight.y,0) == allTilesGenerated [i].transform.position) {
					canSpawnRight = false;

				}
			}

			

			//SECOND TIME SPAWNING A TILE
			//SECOND TIME SPAWNING A TILE
			//SECOND TIME SPAWNING A TILE
		} else if (tilesOnScreen == 1) {

		
				possibleSpotAbove = lastTile.transform.position + new Vector3 (0, 1.6f, 0);

				canSpawnLeft = false;
				canSpawnRight = false;

			
			//FIRST TILE SPAWNED
			//FIRST TILE SPAWNED
			//FIRST TILE SPAWNED

		} else if (tilesOnScreen == 0) {

			canSpawnTurningPiece = false;

			canSpawnLeft = false;
			canSpawnRight = false;



		}

		//DEBUGING
		//DEBUGING
		//DEBUGING

		TESTabovePos.transform.position = possibleSpotAbove;
		TESTleftPos.transform.position = possibleSpotLeft;
		TESTrightPos.transform.position = possibleSpotRight;

		if (!canSpawnAbove) {
			TESTabovePos.GetComponent<SpriteRenderer> ().color = Color.black;
		} else {
			TESTabovePos.GetComponent<SpriteRenderer> ().color = Color.white;
		}
		if (!canSpawnLeft) {
			TESTleftPos.GetComponent<SpriteRenderer> ().color = Color.black;
		}else {
			TESTleftPos.GetComponent<SpriteRenderer> ().color = Color.white;
		}
		if (!canSpawnRight) {
			TESTrightPos.GetComponent<SpriteRenderer> ().color = Color.black;
		}else {
			TESTrightPos.GetComponent<SpriteRenderer> ().color = Color.white;
		}


		if (canSpawnSecondPath) {
			
			TESTabovePosPath2.transform.position = possibleSpotAbovePath2;
			TESTleftPosPath2.transform.position = possibleSpotLeftPath2;
			TESTrightPosPath2.transform.position = possibleSpotRightPath2;

			if (!canSpawnAbovePath2) {
				TESTabovePosPath2.GetComponent<SpriteRenderer> ().color = Color.black;
			} else {
				TESTabovePosPath2.GetComponent<SpriteRenderer> ().color = Color.white;
			}
			if (!canSpawnLeftPath2) {
				TESTleftPosPath2.GetComponent<SpriteRenderer> ().color = Color.black;
			} else {
				TESTleftPosPath2.GetComponent<SpriteRenderer> ().color = Color.white;
			}
			if (!canSpawnRightPath2) {
				TESTrightPosPath2.GetComponent<SpriteRenderer> ().color = Color.black;
			} else {
				TESTrightPosPath2.GetComponent<SpriteRenderer> ().color = Color.white;
			}

		}
	
		Vector2 finialSpawnPos = new Vector2 (0, 0);
		Vector2 finialSpawnPosPath2 = new Vector2 (0, 0);;

		//CHOOSING WHICH POSITION
		//CHOOSING WHICH POSITION
		//CHOOSING WHICH POSITION

		List<Vector2> finialSpawnPosOptions = new List<Vector2> ();
		List<Vector2> finialSpawnPosOptionsPath2 = new List<Vector2> ();

			//ADD THE NECCESSARY POSITIONS TO THE LISTS SO WE CAN LATER PICK RANDOMLY FROM THEM
			if (canSpawnAbove) {
				finialSpawnPosOptions.Add (possibleSpotAbove);
			}
			if (canSpawnLeft) {
				finialSpawnPosOptions.Add (possibleSpotLeft);
			}
			if (canSpawnRight) {
			finialSpawnPosOptions.Add (possibleSpotRight);
			}

			//CHOOSE A RANDOM POSITION IN THE LIST
			finialSpawnPos = finialSpawnPosOptions [Random.Range (0, finialSpawnPosOptions.Count)];

			//FOR THE SECOND PATH CHOOSE ADD ALL ENABLED POSTIIONS TO THE LIST
		if (canSpawnSecondPath) {


			if (canSpawnAbovePath2) {
				finialSpawnPosOptionsPath2.Add (possibleSpotAbovePath2);
			}
			if (canSpawnLeftPath2) {
				finialSpawnPosOptionsPath2.Add (possibleSpotLeftPath2);
			}
			if (canSpawnRightPath2) {
				finialSpawnPosOptionsPath2.Add (possibleSpotRightPath2);
			}

			//THEN CHOOSE FROM THEM
			finialSpawnPosPath2 = finialSpawnPosOptionsPath2 [Random.Range (0, finialSpawnPosOptionsPath2.Count)];

			//IF THE TWO PATHS CONVERGE ON THE SAME POSITION
			if (finialSpawnPosPath2 == finialSpawnPos) {
				
				//UNLESS THIS HAPPENED ON THE FIRST SPLIT PIECE CASE
				if (lastTile.tag == "SplitPiece") {

					//IN WHHICH CASE KEEP CYCLING THROUGH POSSIBLILITIES UNTILL IT ISNT THE SAME
					while (finialSpawnPos == finialSpawnPosPath2) {
						
						finialSpawnPosPath2 = finialSpawnPosOptionsPath2 [Random.Range (0, finialSpawnPosOptionsPath2.Count)];
					}


				//OTHERWISE THIS HAPPENED LATER ON SO CONNECT THE TRACKS INTO ONE
				} else {
					
					canSpawnSecondPath = false;
					mustConnectPaths = true;

				}
			}
		}

			
		//THE TRACK PIECE TO SPAWN
		GameObject TrackPiece = null;
		GameObject TrackPiecePath2 = null;

		if (!mustConnectPaths) {

			//NORMAL CODE FOR NOT CONNECTING TWO PATHS
			List<GameObject> TrackPieceOptions = new List<GameObject> ();
			List<GameObject> TrackPieceOptionsPath2 = new List<GameObject> ();

			//ADD EVERY ENABLED TRACK PIECE TO THE LIST
			//CHOOSE RANDOMLY WHICH ONE TO USE
			if (canSpawnTurningPiece) {
				TrackPieceOptions.Add (turningPiece);
			}
			if (canSpawnStraightPiece) {
				TrackPieceOptions.Add (straightPiece);
			}
			if (canSpawnSplitPiece) {
				TrackPieceOptions.Add (splitPiece);
			}

			TrackPiece = TrackPieceOptions [Random.Range (0, TrackPieceOptions.Count)];

			if (canSpawnSecondPath) {
				if (canSpawnTurningPiecePath2) {
					TrackPieceOptionsPath2.Add (turningPiece);
				}
				if (canSpawnStraightPiecePath2) {
					TrackPieceOptionsPath2.Add (straightPiece);
				}
				if (canSpawnSplitPiecePath2) {
					TrackPieceOptionsPath2.Add (splitPiece);
				}

				TrackPiecePath2 = TrackPieceOptionsPath2 [Random.Range (0, TrackPieceOptionsPath2.Count)];
			}

		} else {

			if (lastTile.transform.position.y == lastTilePath2.transform.position.y) {

				TrackPiece = splitPiece;

			} else {

				if (lastTile.transform.position.x > 0 || lastTilePath2.transform.position.x > 0) {

					TrackPiece = rightJunctionPiece;

				} else if (lastTile.transform.position.x < 0 || lastTilePath2.transform.position.x < 0) {

					TrackPiece = leftJunctionPiece;

				}


			}

		}


		//IF THE TWO PATHS CONVERGED ON ONE SPOT THAN ITS TIME TO CONNECT, UNLESS ITS THE FIRST ONE RIGHT AFTER SPLIT PIECE	
		nextTile = GameObject.Instantiate (TrackPiece, finialSpawnPos, transform.rotation) as GameObject;
		//Add this tile to the master list
		allTilesGenerated.Add (nextTile);

		secondLastTile = lastTile;
		lastTile = nextTile;


		//SPAWN THE PIECE
		if (canSpawnSecondPath) {

			nextTilePath2 = GameObject.Instantiate (TrackPiecePath2, finialSpawnPosPath2, transform.rotation) as GameObject;
			//Add this tile to the master list
			allTilesGenerated.Add (nextTilePath2);


			if (secondLastTile.gameObject.tag == "SplitPiece") {

				secondLastTilePath2 = secondLastTile;

			} else {

				secondLastTilePath2 = lastTilePath2;
			}

			lastTilePath2 = nextTilePath2;


		} 

		tilesOnScreen++;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPieceTouchDetectorScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//HANDLE TOUCH DOWN INPUT
		if ((Input.touchCount > 0) && (Input.GetTouch (0).phase == TouchPhase.Began)) {

			Ray raycast = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);

			RaycastHit raycastHit;

			if (Physics.Raycast(raycast, out raycastHit)){

				if (raycastHit.collider.gameObject.layer == 3){
					transform.parent.GetComponent<TrackPieceScript> ().Rotate ();
				}


			}

		}
	}

	void OnMouseDown(){

		transform.parent.GetComponent<TrackPieceScript> ().Rotate ();
	}
}

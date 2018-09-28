using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	
	public float dampTime = 0.15f;
	public float testvariable;
	[Space]
	[Header("Positions")]

	private Vector2 screenDimensions;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	[Space]
	[Header("Scripts")]

	public GameManagerScript gameManager;

	// Use this for initialization
	void Start () {
		
		screenDimensions  = Camera.main.ScreenToWorldPoint (new Vector2(Screen.width, Screen.height));

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
//		//CAMERA FOLLOW BY SETTING THE CAMERA Y VALUE TO THE TRAIN X VALUE
		if (target && gameManager.gamePlaying)
		{


			Vector3 delta = target.position - transform.position; //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = new Vector3(0,transform.position.y + delta.y + screenDimensions.y/2, 0);


			transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, dampTime);


		}


	
	}
}

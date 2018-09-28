using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {
	
	public GameObject mainTitle;
	public GameObject tapToPlay;

	public float delay;

	public GameManagerScript gameManagerScript;

	// Use this for initialization
	void Start () {
		gameManagerScript = GameObject.FindGameObjectWithTag ("Game Manager").GetComponent<GameManagerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void PlayBtnPressed(){

		StartCoroutine ("MainMenuExit");

		gameManagerScript.StartCoroutine ("BeginGame");

	}

	public IEnumerator MainMenuExit(){


		mainTitle.GetComponent<Animator> ().Play ("Exit");

		yield return new WaitForSeconds (delay);

		tapToPlay.GetComponent<Animator> ().Play ("Exit");
			
		Invoke ("Destroy", 1);

	}

	public void Destroy(){
		Destroy (this.gameObject);

	}
}

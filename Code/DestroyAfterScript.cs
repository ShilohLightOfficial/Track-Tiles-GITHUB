using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterScript : MonoBehaviour {
	public int delay;
	// Use this for initialization
	void Start () {
		Invoke ("Destroy", delay);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Destroy(){

		Destroy (this.gameObject);
	}
}

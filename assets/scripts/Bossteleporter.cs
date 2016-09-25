using UnityEngine;
using System.Collections;

public class Bossteleporter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D (Collider2D other){
		if (other.transform.tag == "speler") {
			Debug.Log ("Dood");
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	}
}

using UnityEngine;
using System.Collections;

public class DoodTrigger : MonoBehaviour {
	
	// Update is called once per frame
	//public void Restart () {
	//	Application.LoadLevel(Application.loadedLevel);
	//}
	void OnTriggerStay2D (Collider2D other){
		if (other.transform.tag == "speler") {
			Debug.Log ("Dood");
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}

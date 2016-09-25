using UnityEngine;
using System.Collections;

public class EnemyMaster : MonoBehaviour {
	//Storing the reference to RagePixelSprite -component
	private IRagePixel ragePixel;
	public enum DeathState {nul= 0, een, twee, drie, vier, vijf, zes, zeven, acht, negen, tien, Dood};
	public DeathState state = DeathState.nul;

	private int levens = 1000;

	private bool swordHit = false;
	private bool SpecialEenHit = false;
	public bool isBoss = false;
	private bool gui = false;

	//muziek
	private float cd_muziekCooldown = 0;
	private float cd_TimeMuziekCooldown = 0.3f;		// cooldown time in seconds 	muziek

	public AudioClip[] attackClips;

	// Use this for initialization
	void Start () {
		ragePixel = GetComponent<RagePixelSprite>();
		if(isBoss){ levens = 2000;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (cd_muziekCooldown > 0) cd_muziekCooldown -= Time.deltaTime;

		if(swordHit)	{
			levens -=  5;
			swordHit = false;
			if(cd_muziekCooldown == 0 || cd_muziekCooldown < 0){
				cd_muziekCooldown += cd_TimeMuziekCooldown;
				int i = Random.Range (0, attackClips.Length);
				AudioSource.PlayClipAtPoint(attackClips[i], transform.position);
			}
		}
		if(SpecialEenHit)		{
			levens -=  3;
			SpecialEenHit = false;
			if(cd_muziekCooldown == 0 || cd_muziekCooldown < 0){
				cd_muziekCooldown += cd_TimeMuziekCooldown;
				int i = Random.Range (0, attackClips.Length);
				AudioSource.PlayClipAtPoint(attackClips[i], transform.position);
			}
		}

		//Debug.Log (levens);
		if(levens < 0){
			state = DeathState.Dood;
		}
		else if(levens < 100){
			state = DeathState.tien;
		}
		else if(levens < 200){
			state = DeathState.negen;
		}
		else if(levens < 300){
			state = DeathState.acht;
		}
		else if(levens < 400){
			state = DeathState.zeven;
		}
		else if(levens < 500){
			state = DeathState.zes;
		}
		else if(levens < 600){
			state = DeathState.vijf;
		}
		else if(levens < 700){
			state = DeathState.vier;
		}
		else if(levens < 800){
			state = DeathState.drie;
		}
		else if(levens < 900){
			state = DeathState.twee;
		}
		else {
			state = DeathState.een;
		}

		switch(state)
		{
		case(DeathState.nul):
			ragePixel.PlayNamedAnimation("NUL", false);
			break;
		case(DeathState.een):
			ragePixel.PlayNamedAnimation("EEN", false);
			break;
		case(DeathState.twee):
			ragePixel.PlayNamedAnimation("TWEE", false);
			break;
		case(DeathState.drie):
			ragePixel.PlayNamedAnimation("DRIE", false);
			break;
		case(DeathState.vier):
			ragePixel.PlayNamedAnimation("VIER", false);
			break;
		case(DeathState.vijf):
			ragePixel.PlayNamedAnimation("VIJF", false);
			break;
		case(DeathState.zes):
			ragePixel.PlayNamedAnimation("ZES", false);
			break;
		case(DeathState.zeven):
			ragePixel.PlayNamedAnimation("ZEVEN", false);
			break;
		case(DeathState.acht):
			ragePixel.PlayNamedAnimation("ACHT", false);
			break;
		case(DeathState.negen):
			ragePixel.PlayNamedAnimation("NEGEN", false);
		     break;
		case(DeathState.tien):
			ragePixel.PlayNamedAnimation("TIEN", false);
		     break;
		case(DeathState.Dood):
			Destroy(gameObject);
			break;
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.transform.tag == "Q" || other.transform.tag == "E") {	
			Debug.Log ("ATTACK0"); 	
			swordHit = true;
		}

		else if (other.transform.tag == "ATTACK1") {
			Debug.Log ("ATTACK1"); 	
			SpecialEenHit = true;
		}
		else {
			Debug.Log ("DAMMMM"); 
		}
	}
}

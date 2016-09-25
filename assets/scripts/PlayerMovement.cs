using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	//Storing the reference to RagePixelSprite -component
	private IRagePixel ragePixel;
	private IRagePixel ragePixel1L;
	private IRagePixel ragePixel1R;
	
	//enum for character state
	public enum WalkingState {Standing = 0, WalkRight, WalkLeft, AttackLeft, AttackRight, Jump, SpecialEen, SpecialTwee, SpecialDrie, Dood};
	public WalkingState state = WalkingState.Standing;
	
	//walking speed (pixels per second)
	private float walkingSpeed = 90f;
	private float jumpForce = 12000f;
	
	private bool CanJump = true;
	private bool Dood = false;
	private bool EersteKeerSpecialEen = true;
	private bool EersteKeerSpecialTwee = true;
	private bool EersteKeerSpecialDrie = true;

	public GameObject spQ;
	public GameObject spE;

	public GameObject sp1L;
	public GameObject sp1R;

	// jump
	private float cd_JumpCooldown = 0; 
	private float cd_TimeJump = 1.2f; 				// cooldown time in seconds 	jump
	// 1
	private float cd_SPEenCooldown = 0;
	private float cd_TimeSpecialEen = 1.2f; 		// cooldown time in seconds 	1
	// 2
	private float cd_SPTweeCooldown = 0;
	private float cd_TimeSpecialTwee = 1.2f; 		// cooldown time in seconds 	2
	// 3
	private float cd_SPDrieCooldown = 0;
	private float cd_TimeSpecialDrie = 1.2f; 		// cooldown time in seconds 	3
	//muziek
	private float cd_muziekCooldown = 0;
	private float cd_TimeMuziekCooldown = 0.3f;		// cooldown time in seconds 	muziek


	private float health = 100f;					// The player's health.
	private float repeatDamagePeriod = 0f;			// How frequently the player can be damaged.					

	private float hurtForce = 0f;					// The force with which the player is pushed when hurt.
	private float damageAmount = 1f;				// The amount of damage to take when enemies touch the player
	private SpriteRenderer healthBar;				// Reference to the sprite renderer of the health bar.
	private float lastHitTime;						// The time at which the player was last hit.
	private Vector3 healthScale;					 // The local scale of the health bar initially (with full health).

	public AudioClip[] hurtClips;
	public AudioClip[] jumpClip;
	public AudioClip[] vuurClip;


	void Awake () {
		healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();

		// Getting the intial scale of the healthbar (whilst the player has full health).
		healthScale = healthBar.transform.localScale;
	}

	void Start () {
		ragePixel = GetComponent<RagePixelSprite>();

		spQ.SetActive(false);
		spE.SetActive(false);
		sp1L.SetActive(false);
		sp1R.SetActive(false);
	}
	
	void Update () {
		if(!Dood){
			if (cd_JumpCooldown > 0) cd_JumpCooldown -= Time.deltaTime;
			if (cd_SPEenCooldown > 0) cd_SPEenCooldown -= Time.deltaTime;
			if (cd_SPTweeCooldown > 0) cd_SPTweeCooldown -= Time.deltaTime;
			if (cd_SPDrieCooldown > 0) cd_SPDrieCooldown -= Time.deltaTime;
			if (cd_muziekCooldown > 0) cd_muziekCooldown -= Time.deltaTime;
		}
		//  Check the keyboard state and set the character state accordingly
		//  1 jump      2 attack     3 specials 		4 lopen    	???    	5 stilstaan 	6 dood
		#region JUMP
		if (Input.GetKey(KeyCode.Space) && CanJump && cd_JumpCooldown < 0.000001f && !Dood) { 		
			state = WalkingState.Jump;
		}
		else if (Input.GetKey(KeyCode.UpArrow) && CanJump && cd_JumpCooldown < 0.000001f && !Dood) {
			state = WalkingState.Jump;
		}
		else if (Input.GetKey(KeyCode.W) && CanJump && cd_JumpCooldown < 0.000001f && !Dood) {
			state = WalkingState.Jump;
		} 
		#endregion

		#region Attack
		else if (Input.GetKey(KeyCode.Q) && !Dood) {
			state = WalkingState.AttackLeft;
			Debug.Log ("attackleft");
		}
		else if (Input.GetKey(KeyCode.E) && !Dood) {
			state = WalkingState.AttackRight;
			Debug.Log ("attackright");
		} 
		#endregion

		#region specials
		else if(Input.GetKey(KeyCode.Alpha1) && !Dood) {
			state = WalkingState.SpecialEen;
			Debug.Log ("1");
		}
		else if(Input.GetKey(KeyCode.Keypad1) && !Dood) {
			state = WalkingState.SpecialEen;
			Debug.Log ("1");
		}
		else if(Input.GetKey(KeyCode.Alpha2) && !Dood) {
			state = WalkingState.SpecialTwee;
			Debug.Log ("2");
		}
		else if(Input.GetKey(KeyCode.Keypad2) && !Dood) {
			state = WalkingState.SpecialTwee;
			Debug.Log ("2");
		}
		else if(Input.GetKey(KeyCode.Alpha3) && !Dood) {
			state = WalkingState.SpecialDrie;
			Debug.Log ("3");
		}
		else if(Input.GetKey(KeyCode.Keypad2) && !Dood) {
			state = WalkingState.SpecialDrie;
			Debug.Log ("3");
		} 
		#endregion

		#region lopen
		else if (Input.GetKey(KeyCode.LeftArrow) && !Dood) {
			state = WalkingState.WalkLeft;
		}
		else if (Input.GetKey(KeyCode.A) && !Dood) {
			state = WalkingState.WalkLeft;
		}
		else if (Input.GetKey(KeyCode.RightArrow) && !Dood) {
			state = WalkingState.WalkRight;
		}
		else if (Input.GetKey(KeyCode.D) && !Dood) {
			state = WalkingState.WalkRight;
		} 
		#endregion

		#region anders
		else if (! Dood) {
			state = WalkingState.Standing;
		} 
		else {
			state = WalkingState.Dood;
		}
		#endregion
		
		Vector3 moveDirection = new Vector3();
		
		switch (state)
		{
		case(WalkingState.Standing):
			ZetAlleKrachtenUit();
			ragePixel.PlayNamedAnimation("STAY", false);
			break;
			
		case (WalkingState.WalkLeft):
			ZetAlleKrachtenUit();
			ragePixel.SetHorizontalFlip(true);
			ragePixel.PlayNamedAnimation("WALK", false);
			moveDirection = new Vector3(-1f, 0f, 0f);
			break;
			
		case (WalkingState.WalkRight):
			ZetAlleKrachtenUit();
			ragePixel.SetHorizontalFlip(false);
			ragePixel.PlayNamedAnimation("WALK", false);
			moveDirection = new Vector3(1f, 0f, 0f);
			break;

		case (WalkingState.AttackLeft):
			//ZetAlleKrachtenUit();
			spE.SetActive(false);
			spQ.SetActive(true);
			sp1L.SetActive(false);
			sp1R.SetActive(false);
			EersteKeerSpecialEen = true;
			ragePixel.SetHorizontalFlip(true);
			ragePixel.PlayNamedAnimation("ATTACK", false);
			moveDirection = new Vector3(0f, 0f, 0f);
			break;

		case (WalkingState.AttackRight):
			//ZetAlleKrachtenUit();
			spE.SetActive(true);
			spQ.SetActive(false);
			sp1L.SetActive(false);
			sp1R.SetActive(false);
			EersteKeerSpecialEen = true;
			ragePixel.SetHorizontalFlip(false);
			ragePixel.PlayNamedAnimation("ATTACK", false);
			moveDirection = new Vector3(0f, 0f, 0f);
			break;

		case (WalkingState.Jump):
			ZetAlleKrachtenUit();
			int i = Random.Range (0, hurtClips.Length);
			AudioSource.PlayClipAtPoint(hurtClips[i], transform.position);
			ragePixel.PlayNamedAnimation("JUMP", false);
			moveDirection = new Vector3(0f, 0f, 0f);
			HandleJumpInput();
			break;

		case (WalkingState.SpecialEen):
			spE.SetActive(false);
			spQ.SetActive(false);
			sp1L.SetActive(true);
			sp1R.SetActive(true);
			int o = Random.Range (0, vuurClip.Length);
			AudioSource.PlayClipAtPoint(vuurClip[o], transform.position);
			ragePixel.PlayNamedAnimation("SPECIALEEN", false);
			moveDirection = new Vector3(0f, 0f, 0f);
			HandleSpecialEen();

			ragePixel1L = sp1L.GetComponent<RagePixelSprite>();
			ragePixel1R = sp1R.GetComponent<RagePixelSprite>();

			if (EersteKeerSpecialEen) {
				ragePixel1L.StopAnimation();
				//Debug.Log (ragePixel1L.isPlaying());

				ragePixel1R.StopAnimation();
				//Debug.Log (ragePixel1R.isPlaying());
				EersteKeerSpecialEen = false;
			}
			else {
				ragePixel1L.PlayNamedAnimation("VUUR", false);
				//Debug.Log (ragePixel1L.isPlaying());

				ragePixel1R.PlayNamedAnimation("VUUR", false);
				//Debug.Log (ragePixel1R.isPlaying());
			}


			break;

		case (WalkingState.SpecialTwee):
			ZetAlleKrachtenUit();
			ragePixel.PlayNamedAnimation("JUMP", false);
			moveDirection = new Vector3(0f, 0f, 0f);
			HandleSpecialTwee();
			break;

		case (WalkingState.SpecialDrie):
			ZetAlleKrachtenUit();
			ragePixel.PlayNamedAnimation("JUMP", false);
			moveDirection = new Vector3(0f, 0f, 0f);
			HandleSpecialDrie();
			break;

		case (WalkingState.Dood):
			ZetAlleKrachtenUit();
			ragePixel.PlayNamedAnimation("DOOD", false);
			moveDirection = new Vector3(0f, 0f, 0f);
			Debug.Log("DOOD");
			break;
		}
		transform.Translate(moveDirection * Time.deltaTime * walkingSpeed);
	}

	void ZetAlleKrachtenUit(){
		spE.SetActive(false);
		spQ.SetActive(false);
		sp1L.SetActive(false);
		sp1R.SetActive(false);
		EersteKeerSpecialEen = true;
	}
	// jump
	void HandleJumpInput () { 
		if(cd_JumpCooldown == 0 || cd_JumpCooldown < 0){
			cd_JumpCooldown += cd_TimeJump;
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
		}
	}
	// 1
	void HandleSpecialEen () { 
		if(cd_SPEenCooldown == 0 || cd_SPEenCooldown < 0){
			cd_SPEenCooldown += cd_TimeSpecialEen;

		}
	}
	// 2
	void HandleSpecialTwee () { 
		if(cd_SPTweeCooldown == 0 || cd_SPTweeCooldown < 0){
			cd_SPTweeCooldown += cd_TimeSpecialTwee;
		}
	}

	// 3
	void HandleSpecialDrie () { 
		if(cd_SPDrieCooldown == 0 || cd_SPDrieCooldown < 0){
			cd_SPDrieCooldown += cd_TimeSpecialDrie;
		}
	}


	
	void OnCollisionStay2D (Collision2D col)
	{
		if(col.gameObject.tag == "Enemy") {
			// ... and if the time exceeds the time of the last hit plus the time between hits...
			if (Time.time > lastHitTime + repeatDamagePeriod) {
				// ... and if the player still has health...
				if(health > 0f) {
					// ... take damage and reset the lastHitTime.
					//TakeDamage(col.transform); 

					lastHitTime = Time.time;
					health -= damageAmount;
					UpdateHealthBar();

					if(cd_muziekCooldown == 0 || cd_muziekCooldown < 0){
						cd_muziekCooldown += cd_TimeMuziekCooldown;
						int i = Random.Range (0, hurtClips.Length);
						AudioSource.PlayClipAtPoint(hurtClips[i], transform.position);
					}
				}

				else {
					CanJump = false;
					Dood = true;
				}
			}
		}
	}

	public void UpdateHealthBar ()
	{
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
		
		// Set the scale of the health bar to be proportional to the player's health.
		healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
	}
}

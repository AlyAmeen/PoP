using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class mainPlayer : MonoBehaviour {
	Animator anim;
	public float speed=2.0f;
	public float rotationSpeed=20.0f;
	public PlayableDirector director;
	public Transform target;
	public Transform player;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		director = GetComponent<PlayableDirector> ();
	}
	void OnTriggerEnter(Collider c){
		
		if (c.gameObject.CompareTag ("floorend")) {
			player.position = new Vector3(target.position.x + 0.001f, 0, target.position.z-1.0f);
			Debug.Log ("position" + player.position + "Target" + target.position);
			director.Play();
		}
	}
	// Update is called once per frame
	void Update () {
		 float x = Input.GetAxis ("Vertical")*speed;
		float rotation = Input.GetAxis ("Horizontal") * rotationSpeed;
		//transform.Translate (0, 0, x*Time.deltaTime);
		//transform.Rotate (0,rotation*Time.deltaTime,0);
		if (Input.GetKeyDown(KeyCode.LeftShift))
			x *= 2;
		anim.SetFloat ("speed", x);
	
		if (Input.GetKeyDown (KeyCode.Space) && !anim.GetBool("jump")) {
			Debug.Log ("TRUEEE");
			anim.SetBool ("jump",true);

		} 
		else if (Input.GetKeyUp (KeyCode.Space) && anim.GetBool("jump")) {
			Debug.Log ("FALSEEEE");
			anim.SetBool ("jump",false);

		} 
		if (Input.GetKeyDown (KeyCode.R)) {
			anim.SetTrigger("roll");

		} 

		if (Input.GetMouseButtonDown (1)) {
			anim.SetTrigger ("block");
		}
		if (Input.GetMouseButtonDown (0)) {
			anim.SetTrigger("attack");
		}

	}
}

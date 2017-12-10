using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ericamovement : MonoBehaviour {
	Animator anim;
	bool r;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		
	}
	
	// Update is called once per frame
	void Update () {

		float x = Input.GetAxis ("Vertical");
		if (Input.GetKey(KeyCode.LeftShift))
			x = x * 2;
		anim.SetFloat ("speed", x);


		if (Input.GetKeyDown (KeyCode.E))
			anim.SetBool ("equip", true);

		if (Input.GetKeyDown (KeyCode.R))
			anim.SetBool ("equip", false);





		if (Input.GetKeyDown(KeyCode.Space))
			anim.SetBool ("drawBow", !(anim.GetBool("drawBow")));
		


	}
}

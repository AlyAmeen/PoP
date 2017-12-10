using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {


	public float  translateSpeed = 3;
	public float rotateSpeed = 180;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {  
		float t = Input.GetAxis ("Vertical"); 

		float R = Input.GetAxis ("Horizontal");
		t = t * translateSpeed * Time.deltaTime;   //when you multiply floating must add f     /   Delta time is time since last update
		R= R*rotateSpeed*Time.deltaTime;	
		transform.Translate (0,0,t);    //z is the forward and backward
		transform.Rotate(0,R,0);
			//print (t);
			//print (R);
	}
}

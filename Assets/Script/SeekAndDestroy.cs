using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeekAndDestroy : MonoBehaviour {


	public GameObject safe;
	public int counter;
	//public Text countText;
	public Text winText;
	// Use this for initialization
	void Start () {
		counter = 0;
	//	SetCountText ();
		winText.text = "";
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider c){
	
		/*foreach(GameObject o in GameObject.FindGameObjectsWithTag("Destroyable"))
			GameObject.Destroy(o);
			

*/
		if (c.attachedRigidbody	) {
			
			GetComponent<Rigidbody>().useGravity = false;
			GetComponent<Rigidbody>().AddForce (0,60,0,ForceMode.Force);

		}
	}

	void OnCollisionEnter(Collision c){
		

		//foreach(GameObject o in GameObject.FindGameObjectsWithTag("Destroyable"))
		//GameObject.Destroy(o);




		//if(!c.gameObject.Equals(safe))
		if (c.gameObject.CompareTag ("Destroyable")) {

			GameObject.Destroy (c.gameObject);
			counter = counter + 1;
		}
		if (c.gameObject.CompareTag ("DestroyableRed")) {
			GameObject.Destroy (c.gameObject);
			counter = counter + 10;
		}
		if(counter == 32)
			winText.text= "You scored 32, You win !";

		}





}

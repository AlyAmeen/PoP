using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Boss : MonoBehaviour {

	Animator anim;
	public float traSpeed= 10f;
	public float rotSpeed= 100f;
	public float jumpSpeed= 1f;
	public int Bosslife = 0;
	public Text life;
	private NavMeshAgent agent;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent> ();
		agent.autoBraking = false;
		life.text = "Boss Life "+Bosslife.ToString ();

	}

	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyDown(KeyCode.Space)) {
//			anim.SetBool ("shield", !anim.GetBool ("shield"));
//		}

		float t = Input.GetAxis ("Vertical");
		t = t * traSpeed * Time.deltaTime;
		transform.Translate(0, 0, t);

		float r = Input.GetAxis ("Horizontal");
		r = r * rotSpeed * Time.deltaTime;
		transform.Rotate(0, r, 0);

		bool j= Input.GetButtonDown("Jump");
		if (j) {
			transform.Translate(0, 1, 0);
		}

		if (Bosslife == 0) {
			anim.SetBool ("die", true);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;


public class patrol : MonoBehaviour {

	public Transform[] points;
	private int destPoint = 0;
	private NavMeshAgent agent;
	public GameObject player;
	private bool chasing = false;



	public float giveUpThreshHold= 25f;
	private float attackRepeat = 2;
	private float attackThreshHold=3f;
	//private float attackSpeed= 3f;

	private GameObject target;

	float attackTimer  = 0;
	public float playerHp=100;
	private float enemyHp=80;
	private bool isDead=false;


	void Start () {
		agent = GetComponent<NavMeshAgent>();
		//player = GameObject.FindGameObjectsWithTag ("pp ");

		// Disabling auto-braking allows for continuous movement
		// between points (ie, the agent doesn't slow down as it
		// approaches a destination point).
		agent.autoBraking = false;

		GotoNextPoint();
	}


	void GotoNextPoint() {
		// Returns if no points have been set up
		if (enemyHp <= 0) {
		
			isDead = true;





		
		}
		if (points.Length == 0 || chasing)
			return;

		// Set the agent to go to the currently selected destination.
		agent.destination = points[destPoint].position;

		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		destPoint = (destPoint + 1) % points.Length;
	}


	void Update () {
		// Choose the next destination point when the agent gets
		// close to the current one.

		
		if (!agent.pathPending && agent.remainingDistance < 0.5f)
			GotoNextPoint();
		Debug.Log ("F1");
		if (!chasing)
			return;

		Vector3 p = target.transform.position;
		Vector3 c = agent.transform.position;
		float pDistance = Mathf.Sqrt (Mathf.Pow (p.x, 2) + Mathf.Pow (p.y, 2) + Mathf.Pow (p.z, 2));
		float cDistance = Mathf.Sqrt (Mathf.Pow (c.x, 2) + Mathf.Pow (c.y, 2) + Mathf.Pow (c.z, 2));

		Debug.Log ("F2");
		float distance = (pDistance - cDistance);
		if (distance > giveUpThreshHold) {
			chasing = false;
			Debug.Log ("F3");
		}
		if (distance < giveUpThreshHold && isplayerVis()) {

			attackTimer -= Time.deltaTime*2f;
			if (attackTimer <= 0 && playerHp != 0) {
				playerHp -= 10f;

				attackTimer = attackRepeat;
			}

		}




	}

	void OnTriggerStay(Collider c){
	
		if (c.gameObject.CompareTag ("pp")) {
			agent.isStopped = true;
			transform.LookAt (player.transform);
			if (isplayerVis()) {
			//chase
				chasing= true;
				target = player;
			

				agent.destination = player.transform.position;
				agent.isStopped = false;

			}

		
	}
}
	bool isplayerVis(){
		RaycastHit hit;
		Vector3 forward = transform.TransformDirection (Vector3.forward) * 10;

		if (Physics.Raycast (transform.position, forward, out hit)) {
		
			return (hit.collider.gameObject.CompareTag ("pp"));
		
		} else

			return false;
	}
}


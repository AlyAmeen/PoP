using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;


public class Boss : MonoBehaviour {

	public Transform[] points;
	private int destPoint = 0;
	private NavMeshAgent agent;
	public mainPlayer player;
	enum State { Chasing, Attacking, IdleAttack,IdleShield,Dead, Shield }
	State currentState = State.Chasing;

	public bool IsDead
	{
		get { return currentState == State.Dead; }
	}

	public float giveUpThreshHold= 25f;
	private float attackRepeat = 2;
	private float attackThreshHold=20f;
	//private float attackSpeed= 3f;

	private GameObject target;

	float attackTimer  = 0;
	float attackIdle  = 0;
	float idleTimer  = 0;
	float shieldTimer =0;
	private float hp=30;
	//private bool isDead=false;
	public Animator anim;
	Vector3 startingPos;

	//	public GameObject sandCollect;
	//private bool isAattacking=false;
	//private bool isRunning = false;
	//private bool isWatchingPlayer= false;

	bool timeWasStopped = true;
	bool saveAgentState = false;



	void Start () {
		startingPos = transform.position;
		target = player.gameObject;
		agent = GetComponent<NavMeshAgent>();

		anim.SetBool("shield",false);
		anim.SetBool("die",false);
		anim.SetBool("attack",false);
		anim.SetBool("finalAttack",false);
		anim.SetBool("walking",true);
		agent.autoBraking = false;

		GotoNextPoint();
	}


	void GotoNextPoint() {

		if (points.Length == 0 )
			return;
		anim.SetBool("isAattacking", false);
		anim.SetBool("isRunning", false);
		// Set the agent to go to the currently selected destination.
		agent.destination = points[destPoint].position;



		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		destPoint = (destPoint + 1) % points.Length;
	}


	void Update () {

		if (mainPlayer.stopTime && currentState != State.Dead)
		{
			saveAgentState = agent.isStopped;
			agent.isStopped = true;
			anim.enabled = false;
			timeWasStopped = true;
			return;
		}
		if (timeWasStopped)
		{
			timeWasStopped = false;
			anim.enabled = true;
			agent.isStopped = saveAgentState;
		}
		if (currentState == State.Dead)
		{

			anim.SetBool("walking", false);
			anim.SetBool("shield", false);
			anim.SetBool("attack", false);
			anim.SetBool("finalattack", false);
			anim.SetBool("die", true);
			enabled = false;
			return;
		}
		// Choose the next destination point when the agent gets
		// close to the current one.
		// Debug.Log(currentState);
		if (currentState == State.IdleAttack) {
			idleTimer += Time.deltaTime;
			if (idleTimer >= 1) {
				anim.SetBool ("walking", false);
				anim.SetBool ("shield", true);
				anim.SetBool ("attack", false);
				anim.SetBool ("finalattack", false);
				currentState = State.Shield;
				idleTimer = 0;
			}

		} else if (currentState == State.Chasing) {

			anim.SetBool ("walking", true);
			agent.isStopped = false;
			Vector3 p = target.transform.position;
			Vector3 c = agent.transform.position;
			float distance = Vector3.Distance (p, c);
			agent.destination = player.transform.position;
			transform.LookAt (player.transform);
			Vector3 angle = transform.rotation.eulerAngles;
			transform.rotation = Quaternion.Euler (new Vector3 (0, angle.y, angle.z));
//			if (distance > giveUpThreshHold)
//			{
//				//isAattacking=false;
//				//isRunning = false;
//
//				anim.SetBool("attack", false);
//				anim.SetBool("walking", false);
//				//anim.SetBool("isDead",false);
//				//isWatchingPlayer= false;
//				currentState = State.Idle;
//				Debug.Log("F3");
//			}

			Debug.Log (distance);
			if (distance < attackThreshHold && isplayerVis ()) {
				currentState = State.Attacking;

			}

		} else if (currentState == State.Attacking) {
			agent.isStopped = true;

			transform.LookAt (player.transform);
			Vector3 angle = transform.rotation.eulerAngles;
			transform.rotation = Quaternion.Euler (new Vector3 (0, angle.y, angle.z));
			Vector3 p = target.transform.position;
			Vector3 c = agent.transform.position;
			float distance = Vector3.Distance (p, c);
			attackIdle = attackIdle + Time.deltaTime;
			if (attackIdle >= 2) {
				attackIdle = 0;
				currentState = State.IdleAttack;
				anim.SetBool ("attack", false);
				anim.SetBool ("walking", false);
				anim.SetBool ("shield", false);
				anim.SetBool ("finalattack", false);
			}
			anim.SetBool ("attack", true);
			anim.SetBool ("walking", false);
			anim.SetBool ("shield", false);
			anim.SetBool ("finalattack", false);


		} else if (currentState == State.Shield) {
			
			agent.isStopped = true;

			transform.LookAt (player.transform);
			Vector3 angle = transform.rotation.eulerAngles;
			transform.rotation = Quaternion.Euler (new Vector3 (0, angle.y, angle.z));
			shieldTimer += Time.deltaTime;
			if (shieldTimer >= 2) {
				anim.SetBool ("attack", false);
				anim.SetBool ("walking", false);
				anim.SetBool ("shield", true);
				anim.SetBool ("finalattack", false);
				currentState = State.IdleShield;
				shieldTimer = 0;
			}

		} else if (currentState == State.IdleShield) {
			idleTimer += Time.deltaTime;
			if (idleTimer >= 1) {

				Vector3 p = target.transform.position;
				Vector3 c = agent.transform.position;
				float distance = Vector3.Distance (p, c);

				Debug.Log (distance);
				if (distance > attackThreshHold) {
					currentState = State.Chasing;

				} else {
					currentState = State.Attacking;
				}
				anim.SetBool ("walking", false);
				anim.SetBool ("shield", false);
				anim.SetBool ("attack", false);
				anim.SetBool ("finalattack", false);
				idleTimer = 0;
			}
			
		}

	}
	public void HitPlayer()
	{
		Debug.Log("neber");
		player.GetHit();
	}
	public void GetHit()
	{
		hp -= 10;
		if (hp <= 0)
		{
			currentState = State.Dead;
			anim.enabled = true;
			//GameObject sandObject = GameObject.Instantiate(sandCollect);
			//sandObject.transform.position = transform.position + new Vector3(0,0.5f,0);
			GetComponent<CharacterController>().enabled = false;

		}
	}
//	void OnTriggerStay(Collider c){
//
//		if (currentState == State.Dead || mainPlayer.stopTime)
//			return;
//		if (c.gameObject.CompareTag ("Player")) {
//			Debug.Log("Happens");
//			if (isplayerVis() && currentState != State.Attacking && !player.isDead) {
//				//chase
//				//isAattacking=false;
//				//isRunning = true;
//				//isWatchingPlayer= false;
//				Debug.Log("ana da5lt");
//				anim.SetBool("isAattacking",false);
//				anim.SetBool("isRunning",true);
//				Debug.Log ("ana 5aragt");
//
//				currentState = State.Chasing;
//				target = player.gameObject;
//
//				Debug.Log("is here");
//				agent.destination = player.transform.position;
//				agent.isStopped = false;
//
//			}
//
//
//		}
//	}
	bool isplayerVis(){

		return true;
		RaycastHit hit;

		Vector3 dir = player.transform.position - transform.position;
		dir.y = 0;
		dir.Normalize();
		Debug.DrawLine(transform.position, transform.position + (dir * 10));
		if (Physics.Raycast (transform.position + new Vector3(0,1,0), dir , out hit)) {

			return (hit.collider.gameObject.CompareTag ("Player"));

		} else

			return false;
	}
}


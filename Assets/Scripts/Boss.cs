using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;


public class Boss : MonoBehaviour {

	public Transform[] points;
	private int destPoint = 0;
	private NavMeshAgent agent;
	public mainPlayer player;
	enum State { Chasing, Attacking, IdleAttack,IdleShield,Dead, Shield, Hit }
	State currentState = State.Chasing;
	public float life =200;
	int i=0;
	public bool IsDead
	{
		get { return currentState == State.Dead; }
	}

	public float giveUpThreshHold= 23f;
	private float attackRepeat = 2;
	private float attackThreshHold=30;
    //private float attackSpeed= 3f;

    bool attackedPlayer = false;
	private GameObject target;

	float attackTimer  = 0;
	float attackIdle  = 0;
	float idleTimer  = 0;
	float shieldTimer =0;
	float hitTimer =0;
	//private bool isDead=false;
	public Animator anim;
	Vector3 startingPos;

	//	public GameObject sandCollect;
	//private bool isAattacking=false;
	//private bool isRunning = false;
	//private bool isWatchingPlayer= false;

	bool timeWasStopped = true;
	bool saveAgentState = false;

    public AudioClip walking;
    public AudioClip hitClip;
    public AudioClip dieClip;
    public AudioClip fightClip;

    AudioSource mySrc;

    public UnityEngine.UI.Text hpText;
    void Start () {
		startingPos = transform.position;
		target = player.gameObject;
		agent = GetComponent<NavMeshAgent>();

		anim.SetBool("shield",false);


		anim.SetBool("attack",false);
		anim.SetBool("finalAttack",false);
		anim.SetBool("walking",true);
		agent.autoBraking = false;

		GotoNextPoint();

        mySrc = GetComponent<AudioSource>();
        mySrc.volume = PlayerPrefs.GetFloat("effect1");
        mySrc.clip = walking;
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

        hpText.text = "Boss hp " + life;
        if (!agent.isStopped)
        {
            // mySrc.pitch = 3;
            if (!mySrc.isPlaying)
                mySrc.Play();
        }
        else
        {
            if (mySrc.isPlaying)
            mySrc.Pause();
        }
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
			anim.SetBool("finalAttack", false);

			anim.SetTrigger ("die 0");
			enabled = false;
			return;
		}


		if (currentState == State.IdleAttack) {
			idleTimer += Time.deltaTime;
			if (idleTimer >= 2) {

//				Debug.Log (anim.GetBool ("hit"));
				if (anim.GetBool ("hit")) {
					currentState = State.Hit;
					anim.SetBool ("walking", false);
					anim.SetBool ("shield", false);
					anim.SetBool ("attack", false);
					anim.SetBool ("finalAttack", false);
					anim.SetBool ("hit", true);
				} else {
					currentState = State.Shield;
					anim.SetBool ("walking", false);
					anim.SetBool ("shield", true);
					anim.SetBool ("attack", false);
					anim.SetBool ("finalAttack", false);
					anim.SetBool ("hit", false);
				}
				idleTimer = 0;
			}

		} else if (currentState == State.Chasing) {
			Vector3 p = target.transform.position;
			Vector3 c = agent.transform.position;
			float distance = Vector3.Distance (p, c);
			agent.destination = player.transform.position;
			transform.LookAt (player.transform);
			Vector3 angle = transform.rotation.eulerAngles;
			transform.rotation = Quaternion.Euler (new Vector3 (0, angle.y, angle.z));
            //			Debug.Log (distance);
            Debug.Log(distance);
			if (distance < attackThreshHold && isplayerVis ()) {
				currentState = State.Attacking;
			} else {
				anim.SetBool ("walking", true);
				agent.isStopped = false;
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
			Debug.Log ("AttackLife " + life);
			if (life > 110) {
				anim.SetBool ("attack", true);
				anim.SetBool ("walking", false);
				anim.SetBool ("shield", false);
				anim.SetBool ("finalAttack", false);
				anim.SetBool ("hit", false);
                if (attackIdle >= 1)
                {

                    HitPlayer(10);
                }
                    if (attackIdle >= 2) {

                    attackedPlayer = false;
                    attackIdle = 0;
					currentState = State.IdleAttack;
					anim.SetBool ("attack", false);
					anim.SetBool ("walking", false);
					anim.SetBool ("shield", false);
					anim.SetBool ("finalAttack", false);
				}
			} else {
                if (attackIdle >= 1)
                {
                    HitPlayer(20);
                }
                anim.SetBool ("attack", false);
				anim.SetBool ("walking", false);
				anim.SetBool ("shield", false);
				anim.SetBool ("finalAttack", true);
				anim.SetBool ("hit", false);
				if (attackIdle >= 2) {
                    attackedPlayer = false;
					attackIdle = 0;
					currentState = State.IdleAttack;
					anim.SetBool ("attack", false);
					anim.SetBool ("walking", false);
					anim.SetBool ("shield", false);
					anim.SetBool ("finalAttack", false);
				}
			}


		} else if (currentState == State.Shield) {
			
			agent.isStopped = true;
			Debug.Log("Our state is shielding");
			transform.LookAt (player.transform);
			Vector3 angle = transform.rotation.eulerAngles;
			transform.rotation = Quaternion.Euler (new Vector3 (0, angle.y, angle.z));
			shieldTimer += Time.deltaTime;
			if (shieldTimer >= 1.5) {
				anim.SetBool ("attack", false);
				anim.SetBool ("walking", false);
				anim.SetBool ("shield", false);
				anim.SetBool ("finalAttack", false);
				currentState = State.IdleShield;
				shieldTimer = 0;
			} else {
				anim.SetBool ("shield", true);
				anim.SetBool ("attack", false);
				anim.SetBool ("walking", false);
				anim.SetBool ("finalAttack", false);
				anim.SetBool ("hit", false);
			}


		} else if (currentState == State.IdleShield) {
			idleTimer += Time.deltaTime;
			if (idleTimer >=2) {
				if (anim.GetBool ("hit")) {
					currentState = State.Hit;
					anim.SetBool ("attack", false);
					anim.SetBool ("walking", false);
					anim.SetBool ("shield", false);
					anim.SetBool ("finalAttack", false);
					anim.SetBool ("hit",true);
				} else {
					anim.SetBool ("walking", false);
					anim.SetBool ("shield", false);
					anim.SetBool ("attack", false);
					anim.SetBool ("finalAttack", false);

					Vector3 p = target.transform.position;
					Vector3 c = agent.transform.position;
					float distance = Vector3.Distance (p, c);
//					Debug.Log (distance);

					if (distance > attackThreshHold) {
						currentState = State.Chasing;

					} else {
						currentState = State.Attacking;
					}
				}

				idleTimer = 0;
			}
			
		} else { if(currentState == State.Hit){


				if (life <= 0) {
					currentState = State.Dead;
				} else {
					hitTimer += Time.deltaTime;
					if (i == 0) {
					//	life -= 50;
						i = 1;
					}

					Debug.Log ("Life " + life);
					if (hitTimer >= 2.5) {
						anim.SetBool ("hit", false);
						anim.SetBool ("shield", false);
						anim.SetBool ("attack", false);
						anim.SetBool ("finalAttack", false);
						currentState = State.Chasing;
						hitTimer = 0;
						i = 0;
					}
				}
			}
		}

	}
	public void HitPlayer(float hit)
	{
        if (attackedPlayer)
            return;

        attackedPlayer = true;
        Vector3 p = target.transform.position;
        Vector3 c = agent.transform.position;
        float distance = Vector3.Distance(p, c);
        if (distance < 20)
        {
            Debug.Log("hit  " + hit);
            Camera.main.GetComponent<AudioSource>().PlayOneShot(hitClip);
            player.GetHit(hit);

        }   
	}
	public void GetHit()
	{
        if (currentState == State.IdleAttack || currentState == State.IdleShield)
        {
     
            idleTimer = 2;
            life -= 10;
            if (life <= 0)
            {
                currentState = State.Dead;
                anim.enabled = true;
                //GameObject sandObject = GameObject.Instantiate(sandCollect);
                //sandObject.transform.position = transform.position + new Vector3(0,0.5f,0);
                GetComponent<CharacterController>().enabled = false;
                mySrc.PlayOneShot(dieClip);

            }
            else
            {
                anim.SetBool("hit", true);
                anim.SetTrigger("hit 2");
            }
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


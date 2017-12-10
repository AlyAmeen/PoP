using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;


public class patrolGuard : MonoBehaviour {

	public Transform[] points;
	private int destPoint = 0;
	private NavMeshAgent agent;
	public mainPlayer player;
    enum State { Chasing, Attacking, Idle,Dead }
    State currentState = State.Idle;

    public bool IsDead
    {
        get { return currentState == State.Dead; }
    }

	public float giveUpThreshHold= 25f;
	private float attackRepeat = 2;
	private float attackThreshHold=2.5f;
	//private float attackSpeed= 3f;

	private GameObject target;

	float attackTimer  = 0;
	private float hp=30;
	//private bool isDead=false;
	public Animator anim;
    Vector3 startingPos;

    public GameObject sandCollect;
    //private bool isAattacking=false;
    //private bool isRunning = false;
    //private bool isWatchingPlayer= false;

    bool timeWasStopped = true;
    bool saveAgentState = false;
	void Start () {
        startingPos = transform.position;
        agent = GetComponent<NavMeshAgent>();
		//player = GameObject.FindGameObjectsWithTag ("pp ");

		// Disabling auto-braking allows for continuous movement
		// between points (ie, th e agent doesn't slow down as it
		// approaches a destination point).
		anim.SetBool("isAattacking",false);
		anim.SetBool("isRunning",false);
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
            
            anim.SetBool("isAattacking", false);
            anim.SetBool("isRunning", false);
            anim.SetTrigger("isDead2");
            enabled = false;
            return;
        }
        // Choose the next destination point when the agent gets
        // close to the current one.
        // Debug.Log(currentState);
        if (currentState == State.Idle)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }
        else if (currentState == State.Chasing)
        {
         
                Vector3 p = target.transform.position;
                Vector3 c = agent.transform.position;
                float distance = Vector3.Distance(p, c);

                transform.LookAt(player.transform);
                if (distance > giveUpThreshHold)
                {
                    //isAattacking=false;
                    //isRunning = false;

                    anim.SetBool("isAattacking", false);
                    anim.SetBool("isRunning", false);
                    //anim.SetBool("isDead",false);
                    //isWatchingPlayer= false;
                    currentState = State.Idle;
                    Debug.Log("F3");
                }
                if (distance < attackThreshHold && isplayerVis())
                {
                    currentState = State.Attacking;
                }
            
        }
        else if (currentState == State.Attacking)
        {
            agent.isStopped = true;

            transform.LookAt(player.transform);
            Vector3 p = target.transform.position;
            Vector3 c = agent.transform.position;
            float distance = Vector3.Distance(p, c);

         
            if (distance > attackThreshHold )
            {
                //  currentState = State.Chasing;
                anim.SetBool("isAattacking", false);
                anim.SetBool("isRunning", true);
                currentState = State.Chasing;
            }
            else
            {
                attackTimer -= Time.deltaTime ;
                if (attackTimer <= 0 && !player.isDead )
                {
                    //isAattacking = true;
                    //isRunning = false;
                    anim.SetBool("isAattacking", true);
                    anim.SetBool("isRunning", false);
                    //anim.SetBool("isDead",false);
                    //isWatchingPlayer = false;
                  

                    attackTimer = attackRepeat;
                }
               if (player.isDead)
                {
                    currentState = State.Idle;
                }
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
            GameObject sandObject = GameObject.Instantiate(sandCollect);
            sandObject.transform.position = transform.position + new Vector3(0,0.5f,0);
            GetComponent<CapsuleCollider>().enabled = false;

        }
    }
	void OnTriggerStay(Collider c){

        if (currentState == State.Dead || mainPlayer.stopTime)
            return;
		if (c.gameObject.CompareTag ("Player")) {
            Debug.Log("Happens");
			if (isplayerVis() && currentState != State.Attacking && !player.isDead) {
				//chase
				//isAattacking=false;
				//isRunning = true;
				//isWatchingPlayer= false;
				Debug.Log("ana da5lt");
				anim.SetBool("isAattacking",false);
				anim.SetBool("isRunning",true);
				Debug.Log ("ana 5aragt");

                currentState = State.Chasing;
				target = player.gameObject;

                Debug.Log("is here");
				agent.destination = player.transform.position;
				agent.isStopped = false;
               
			}


		}
	}
	bool isplayerVis(){
		RaycastHit hit;

        Vector3 dir = player.transform.position - transform.position;
        dir.y = 0;
        dir.Normalize();
		if (Physics.Raycast (transform.position, dir , out hit)) {

			return (hit.collider.gameObject.CompareTag ("Player"));

		} else

			return false;
	}
}


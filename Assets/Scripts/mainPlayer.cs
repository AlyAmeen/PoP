using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.Timeline;
public class mainPlayer : MonoBehaviour {
	Animator anim;
	public float speed=2.0f;
	public float rotationSpeed=50.0f;
	public PlayableDirector director;
	public Transform target;
	public Transform player;
    public static bool stopTime = false;

    float health = 100;
    int sandOfTimes =5;

    float sandOfTimeTimer = 0;

    float stopRotation = 0;
    public bool isDead = false;
    bool attack = false;

    Vector3 savedWallNormal;
    public LayerMask mask;
    public PlayableAsset wallRunLeft;

    public TimelineAsset TimlineAsset;

    float stopControlsAttack = 0;

    bool isBlocking = false;
    bool hitEnemyFrame = false;

    GameObject[] enemies;
    // Use this for initialization
    void Start () {
		anim = GetComponent<Animator> ();
		director = GetComponent<PlayableDirector> ();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}
	void OnTriggerEnter(Collider c){
		
		if (c.gameObject.CompareTag ("floorend")) {
			player.position = new Vector3(target.position.x + 0.001f, 0, target.position.z-1.0f);
			Debug.Log ("position" + player.position + "Target" + target.position);
			director.Play();
		}
        if (c.gameObject.CompareTag("Collectible"))
        {
            GameObject.Destroy(c.gameObject);
            sandOfTimes++;  
        }
    }
    public void Die()
    {
        if (isDead)
            return;
        Debug.Log("dead");
        isDead = true;
        anim.SetTrigger("Dead");
    }
    public void GetHit()
    {

        if (isBlocking)
            return;
        health -= 10;
        if (health <= 0)
            Die();

    }
    void HitEnemy()
    {
        foreach (GameObject enemy in enemies)
        {
            patrolGuard g = enemy.GetComponent<patrolGuard>();
            // Calculate the vector pointing from the player to the enemy
            Vector3 enemyDir = enemy.transform.position - transform.position;

            float dis = Vector3.Distance(enemy.transform.position, transform.position);
            // Calculate the angle between the forward vector of the player and the vector pointing to the enemy
            float angle = Vector3.Angle(transform.forward, enemyDir);
            if (angle <= 90 && dis <= 4 && !g.IsDead)
            {
                g.GetHit();
            }
        }

    }
    // Update is called once per frame
    void Update() {

        if (isDead)
            return;
        stopControlsAttack -= Time.deltaTime;
        if (stopControlsAttack > 0)
        {
            if (stopControlsAttack < 0.4f && !hitEnemyFrame)
            {
                hitEnemyFrame = true;
                HitEnemy();


            }
            anim.SetBool("attack2", false);
            return;
        }
      
        float x = Input.GetAxis("Vertical") * speed;
        //x = Input.GetAxis ("Horizontal") * rotationSpeed;
        //transform.Translate (0, 0, x*Time.deltaTime);
        var CharacterRotation = Camera.main.transform.rotation;


        float z = Input.GetAxis("Horizontal");

        CharacterRotation.x = 0;
        CharacterRotation.z = 0;
        Vector3 angle = CharacterRotation.eulerAngles;
        stopRotation -= Time.deltaTime;

        CharacterRotation.eulerAngles = new Vector3(angle.x, angle.y + (z * 90), angle.z);



        if (Input.GetKey(KeyCode.LeftShift))
            x *= 3;

        if (x > 0 && Mathf.Abs(z) > 0)
        {
            x /= 2f;
            z /= 2f;
        }

        anim.SetFloat("speed", x + Mathf.Abs(speed * z));

        if (director.state == PlayState.Paused)
        {
            transform.rotation = CharacterRotation;

        }
        else
            transform.forward = savedWallNormal;
        RaycastHit wallHit;
        if (Physics.Linecast(transform.position + new Vector3(0, 5, 0), transform.position + new Vector3(0, 5, 0) + (transform.forward * 5), out wallHit, mask))
        {
            Debug.Log(wallHit.collider.gameObject.name);
        }

            Debug.DrawLine(transform.position + new Vector3(0, 1, 0), transform.position + new Vector3(0, 1, 0) + (transform.forward * 5));
        if (Input.GetKeyDown(KeyCode.C))
        {
         
        
            if (Physics.Linecast(transform.position + new Vector3(0, 5, 0), transform.position + new Vector3(0, 5, 0) + (transform.forward * 2), out wallHit, mask))
            {
                Debug.Log("hIT");
                Vector3 normal = wallHit.normal;
                //if optuse or acute using dot product
                float ag = Mathf.PI / 2f;
                normal.z = wallHit.normal.z * Mathf.Cos(ag) - wallHit.normal.x * Mathf.Sin(ag);
                normal.x = wallHit.normal.z * Mathf.Sin(ag) + wallHit.normal.x * Mathf.Cos(ag);
                transform.position = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, transform.position.y, wallHit.point.z + wallHit.normal.z * 0.5f);
                float dotProduct = Vector3.Dot(normal, transform.forward);
                if (dotProduct < 0)
                {
     
                    ag = -ag;

                    normal.z = wallHit.normal.z * Mathf.Cos(ag) - wallHit.normal.x * Mathf.Sin(ag);
                    normal.x = wallHit.normal.z * Mathf.Sin(ag) + wallHit.normal.x * Mathf.Cos(ag);
                
                    director.playableAsset = wallRunLeft;
                    director.SetGenericBinding(wallRunLeft, gameObject);
                    director.Play();
                    transform.forward = normal;
                    savedWallNormal = normal;
                }
                else
                {
                    director.playableAsset = TimlineAsset;
                    foreach (var playableAssetOutput in director.playableAsset.outputs)
                    {
                            director.SetGenericBinding(playableAssetOutput.sourceObject, gameObject );
                    }
                    director.Play();
                    director.Play();
                    transform.forward = normal;
                    savedWallNormal = normal;
                }
                wallHit.normal = normal;
                Debug.DrawLine(wallHit.point, new Vector3(wallHit.point.x + wallHit.normal.x * 3,
       wallHit.point.y, wallHit.point.z + wallHit.normal.z * 3));
             
            }
        }
      
            if (Input.GetKeyDown(KeyCode.Q))
        {
            if (sandOfTimes >0)
            {
                sandOfTimes -= 1;
                sandOfTimeTimer = 3;
                stopTime = true;
            }
        }
        if (stopTime)
        {
            sandOfTimeTimer -= Time.deltaTime;
            if (sandOfTimeTimer <= 0)
            {
                stopTime = false;
            }
        }
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
            stopRotation = 3;


        } 

		if (Input.GetMouseButtonDown (1)) {
            isBlocking = true;
            anim.SetBool("block",true);
		}
        if (Input.GetMouseButtonUp(1))
        {
            isBlocking = false;
            anim.SetBool("block",false);
        }
        attack = false;
        if (Input.GetMouseButtonDown (0)) {
            stopControlsAttack = 0.65f ;
            hitEnemyFrame = false;
            anim.SetBool("attack2", true);

        }
    }
}

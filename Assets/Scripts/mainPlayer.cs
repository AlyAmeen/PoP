using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
public class mainPlayer : MonoBehaviour {
	Animator anim;
	public float speed=2.0f;
	public float rotationSpeed=50.0f;
	public PlayableDirector director;
	public Transform target;
	public Transform player;
    public static bool stopTime = false;

    float health = 100;
    int sandOfTimes = 0;

    float sandOfTimeTimer = 0;

    float rollTime = 0;
    public bool isDead = false;
    bool attack = false;

    Vector3 savedWallNormal;
    public LayerMask mask;
    public PlayableAsset wallRunLeft;

    public TimelineAsset TimlineAsset;

    float stopControlsAttack = 0;

    bool isBlocking = false;
    bool isRolling = false;
    bool hitEnemyFrame = false;

    GameObject[] enemies;

    bool jumping = false;
    float jumpingTimer = 0.5f;
    public HitEffect hitEffect;

    float gameOverTimer = 0;
    public Text playerHealthText;
    public Text sandOfTimeText;

    public AudioClip footSteps;
    public AudioClip die;
    public AudioClip collectible;

    AudioSource mySource;
    // Use this for initialization
    void Start () {
		anim = GetComponent<Animator> ();
		director = GetComponent<PlayableDirector> ();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        mySource = GetComponent<AudioSource>();

        mySource.clip = footSteps;

    }
	void OnTriggerEnter(Collider c){
		
	
        if (c.gameObject.CompareTag("Collectible"))
        {
            GameObject.Destroy(c.gameObject);
            sandOfTimes++;  
        }
        bool allEnemiesDead = true;
        Debug.Log(enemies.Length);
        if (c.gameObject.CompareTag("GameEnd"))
        {
            foreach (GameObject e in enemies)
            {
                if (e.GetComponent<patrolGuard>().enabled == true)
                {
                    Debug.Log("Shit");
                    allEnemiesDead = false;
                    break;
                }


            }
            if (allEnemiesDead)
            {

                Application.LoadLevel(Application.loadedLevel + 1);
            }
        }
      
    }
    public void Die()
    {
        if (isDead)
            return;

        mySource.PlayOneShot(die);
        Debug.Log("dead");
        isDead = true;
        anim.SetTrigger("Dead");
    }
    public void GetHit()
    {

        if (isBlocking || rollTime > 0)
            return;

        hitEffect.GotHit();
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
        {
            gameOverTimer += Time.deltaTime;
            if (gameOverTimer >= 5)
                Application.LoadLevel("gameOver");
            return;
        }
        playerHealthText.text = "Player Health " + health;
        sandOfTimeText.text = "Sands of time " + sandOfTimes;

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
        if (jumping)
        {
            jumpingTimer -= Time.deltaTime;
            if (jumpingTimer <= 0)
                jumping = false;
        }
        if (!GetComponent<CharacterController>().isGrounded && director.state == PlayState.Paused
            && !jumping)
        {
            Vector3 vel = Vector3.zero;
            float vSpeed = 0;
            // apply gravity acceleration to vertical speed:
            vSpeed -= 64 * Time.deltaTime;
            vel.y = vSpeed; // include vertical speed in vel
                            // convert vel to displacement and Move the character:
            GetComponent<CharacterController>().Move(vel * Time.deltaTime);
        }
        float x = Input.GetAxis("Vertical") * speed;
        //x = Input.GetAxis ("Horizontal") * rotationSpeed;
        //transform.Translate (0, 0, x*Time.deltaTime);
        var CharacterRotation = Camera.main.transform.rotation;

       
        float z = Input.GetAxis("Horizontal");

        CharacterRotation.x = 0;
        CharacterRotation.z = 0;
        Vector3 angle = CharacterRotation.eulerAngles;
        rollTime -= Time.deltaTime;
     
        CharacterRotation.eulerAngles = new Vector3(angle.x, angle.y + (z * 90), angle.z);



        if (Input.GetKey(KeyCode.LeftShift))
            x *= 3;

        mySource.pitch = Mathf.Clamp(x,2.5f,4);
        if (x > 0 && !mySource.isPlaying)
            mySource.Play();
        else
            mySource.Pause();
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
        if (Physics.Linecast(transform.position + new Vector3(0, 1.5f, 0),
            transform.position + new Vector3(0, 1.5f, 0) + (transform.forward * 5), out wallHit, mask))
        {
            Debug.Log(wallHit.collider.gameObject.name);
        }

            Debug.DrawLine(transform.position + new Vector3(0, 1, 0), transform.position + new Vector3(0, 1, 0) + (transform.forward * 5));
        if (Input.GetKeyDown(KeyCode.C))
        {
         
        
            if (Physics.Linecast(transform.position + new Vector3(0, 1.5f, 0), transform.position + new Vector3(0, 1.5f, 0) + (transform.forward * 1.5f), out wallHit, mask))
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
                sandOfTimeTimer = 5;
                stopTime = true;
                mySource.PlayOneShot(collectible);
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
            jumping = true;
            jumpingTimer = 0.4f;

		} 
		else if (Input.GetKeyUp (KeyCode.Space) && anim.GetBool("jump")) {
			Debug.Log ("FALSEEEE");
			anim.SetBool ("jump",false);

		} 
		if (Input.GetKeyDown (KeyCode.R)) {
            isRolling = true;

            anim.SetTrigger("roll");
            rollTime = 0.7f;


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
            stopControlsAttack = 0.8f ;
            hitEnemyFrame = false;
            anim.SetTrigger("attack");

        }
    }
}

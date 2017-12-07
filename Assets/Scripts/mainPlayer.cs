using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class mainPlayer : MonoBehaviour {
	Animator anim;
	public float speed=2.0f;
	public float rotationSpeed=50.0f;
	public PlayableDirector director;
	public Transform target;
	public Transform player;
    public static bool stopTime = false;

    float health = 100;
    float sandOfTimes =5;

    float sandOfTimeTimer = 0;

    float stopRotation = 0;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		director = GetComponent<PlayableDirector> ();
	}
	void OnTriggerEnter(Collider c){
		
		if (c.gameObject.CompareTag ("floorend")) {
			player.position = new Vector3(target.position.x + 0.001f, 0, target.position.z-1.0f);
			Debug.Log ("position" + player.position + "Target" + target.position);
			director.Play();
		}
	}
    public void Die()
    {
        //player should die here
        Debug.Log("Dead");  
    }
	// Update is called once per frame
	void Update () {
     
        float x = Input.GetAxis ("Vertical")*speed;
        //x = Input.GetAxis ("Horizontal") * rotationSpeed;
        //transform.Translate (0, 0, x*Time.deltaTime);
        var CharacterRotation = Camera.main.transform.rotation;
        

        float z = Input.GetAxis("Horizontal") ;

        CharacterRotation.x = 0;
        CharacterRotation.z = 0;
        Vector3 angle = CharacterRotation.eulerAngles;
        stopRotation -= Time.deltaTime;

        CharacterRotation.eulerAngles = new Vector3(angle.x, angle.y + (z * 90), angle.z);

        
            transform.rotation = CharacterRotation;
        if (Input.GetKey(KeyCode.LeftShift))
			x *= 3;

        if (x > 0 && Mathf.Abs(z)> 0)
        {
            x /= 2f;
            z /= 2f;    
        }
     
		anim.SetFloat ("speed", x + Mathf.Abs(speed * z));
	
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
            anim.SetBool("block",true);
		}
        if (Input.GetMouseButtonUp(1))
        {
            anim.SetBool("block",false);
        }
        if (Input.GetMouseButtonDown (0)) {
			anim.SetTrigger("attack");
		}

	}
}

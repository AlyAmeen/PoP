using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    enum State { Idle,GoingUp,IdleUp,GoingDown}
    State state = State.Idle;
    float timer = 0;

    const float timeToRaise = 4;    
    const float timeGoingUp = 0.2f;
    const float timeStayingUp = 1;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (mainPlayer.stopTime)
            return;
        timer += Time.deltaTime;

        if (state == State.Idle)
        {
            if (timer >= timeToRaise)
            {
                if (transform.localPosition.y < 5)
                    transform.position = transform.position + new Vector3(0, 10 * Time.deltaTime, 0);
            else
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, 5, transform.localPosition.z);
                }
            }
            if (timer >= timeToRaise + timeGoingUp)
            {
                state = State.IdleUp;
                timer = 0;
            }
        }
        if (state == State.IdleUp)
        {
            if (timer >= timeStayingUp )
            {
                if (transform.localPosition.y > -5)
                    transform.position = transform.position - new Vector3(0, 10 * Time.deltaTime, 0);
            }
            if (timer >= timeStayingUp + timeGoingUp)
            {
                state = State.Idle;
                timer = 0;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<mainPlayer>().Die();
        }
    }
}

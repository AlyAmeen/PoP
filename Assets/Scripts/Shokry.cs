using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shokry : MonoBehaviour {

    public bool invert = false;
    bool right = false;
    public float range = 5;
    float speed = 5;
    Vector3 startingPos;
	// Use this for initialization
	void Start () {
        startingPos = transform.position;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<mainPlayer>().Die();
        }
    }
    // Update is called once per frame
    void Update () {
        if (mainPlayer.stopTime)
            return;
        if (invert)
        {
            if (right)
            {
                transform.position += new Vector3(0, 0, speed * Time.deltaTime);
                if (transform.position.z - startingPos.z > range)
                    right = !right;
            }
            else
            {
                transform.position -= new Vector3(0,0,speed * Time.deltaTime);
                if (transform.position.z < startingPos.z)
                    right = !right;
            }
        }
        else
        {
            if (right)
            {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                if (transform.position.x - startingPos.x > range)
                    right = !right;
            }
            else
            {
                transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                if (transform.position.x < startingPos.x)
                    right = !right;
            }
        }
        transform.Rotate(new Vector3(0, 0, 20 * Time.deltaTime));
	}
}

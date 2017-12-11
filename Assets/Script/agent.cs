using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class agent : MonoBehaviour {
	UnityEngine.AI.NavMeshAgent agenT;
	public GameObject target;
	// Use this for initialization
	void Start () {
		agenT = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		agenT.destination = target.transform.position;
	}
}
